using LearnLink.Application.Helpers;
using LearnLink.Application.Mappers;
using LearnLink.Application.Transaction;
using LearnLink.Core.Entities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Interactors
{
    public class ObjectiveInteractor(IUnitOfWork unitOfWork, DirectoryStore directoryStore, ContentInteractor contentInteractor)
    {
        public async Task<Response<ObjectiveDto?>> GetObjectiveAsync(int lessonId, int objectiveId)
        {
            try
            {
                var objective = await unitOfWork.Objectives.FindAsync(objectiveId);

                return new Response<ObjectiveDto?>()
                {
                    Success = true,
                    StatusCode = 200,
                    Value = objective?.ToDto(lessonId)
                };
            }
            catch (CustomException exception)
            {
                return new Response<ObjectiveDto?>()
                {
                    Success = false,
                    StatusCode = exception.StatusCode,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new Response<ObjectiveDto?>()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Не удалось получить задание",
                    InnerErrorMessages = [exception.Message],
                };
            }
        }


        public async Task<Response<ObjectiveDto[]>> GetObjectivesFromLessonAsync(int lessonId)
        {
            try
            {

                var objectives = await unitOfWork.LessonObjectives
                    .AsNoTracking()
                    .Where(lessonObjective => lessonObjective.LessonId == lessonId)
                    .Include(lessonObjective => lessonObjective.Objective)
                    .ThenInclude(objective => objective.FileContent)
                    .Select(lessonObjective => lessonObjective.Objective)
                    .ToArrayAsync();

                return new()
                {
                    Success = true,
                    StatusCode = 200,
                    Value = objectives.Select(objective => objective.ToDto(lessonId)).ToArray()
                };
            }
            catch (CustomException exception)
            {
                return new()
                {
                    Success = false,
                    StatusCode = exception.StatusCode,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Не удалось получить задания",
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        public async Task<Response> CreateObjectiveAsync(int lessonId, ObjectiveDto objectiveDto)
        {
            try
            {
                if (!ValidationHelper.ValidateToEmptyStrings(objectiveDto.Title, objectiveDto.Text))
                {
                    throw new ValidationException("Не все поля были заполнены");
                }

                var lesson = await unitOfWork.Lessons.FindAsync(lessonId);
                NotFoundException.ThrowIfNotFound(lesson, "Урок не был найден");

                var objective = objectiveDto.ToEntity();

                await unitOfWork.Objectives.AddAsync(objective);
                await unitOfWork.CommitAsync();

                var lessonObjective = new LessonObjective()
                {
                    Lesson = lesson,
                    Objective = objective
                };

                await unitOfWork.LessonObjectives.AddAsync(lessonObjective);
                await unitOfWork.CommitAsync();

                if (objectiveDto.FormFile != null)
                {
                    await SaveObjectiveFileContentAsync(lessonId, objective, objectiveDto.FormFile);
                }

                return new Response()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Задание успешно создано"
                };
            }
            catch (CustomException exception)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = exception.StatusCode,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Не удалось создать задание",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }


        public async Task<Response> UpdateObjectiveAsync(ObjectiveDto objectiveDto, bool removeFileContent)
        {
            try
            {
                var lessonObjective = await unitOfWork.LessonObjectives
                    .FirstOrDefaultAsync(lessonObjective => lessonObjective.ObjectiveId == objectiveDto.Id);

                NotFoundException.ThrowIfNotFound(lessonObjective, "Урок, к которому прикреплено задание не найден");

                await unitOfWork.LessonObjectives.Entry(lessonObjective)
                    .Reference(lessonObjective => lessonObjective.Objective)
                    .LoadAsync();

                await unitOfWork.Objectives.Entry(lessonObjective.Objective)
                    .Reference(objective => objective.FileContent)
                    .LoadAsync();

                var objective = lessonObjective.Objective;
                var prevFileState = objective.FileContent;

                objective = objective.Assign(objectiveDto);

                if (prevFileState != null && (objective.FileContent != null || removeFileContent))
                {
                    contentInteractor.RemoveObjectiveFileContent(
                        lessonObjective.LessonId,
                        objective.Id,
                        prevFileState.Id,
                        prevFileState.FileName
                        );
                }

                unitOfWork.Objectives.Update(objective);
                await unitOfWork.CommitAsync();

                if (objectiveDto.FormFile != null)
                {
                    await SaveObjectiveFileContentAsync(lessonObjective.LessonId, objective, objectiveDto.FormFile);
                }

                return new Response()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Задание успешно обновлено"
                };
            }
            catch (CustomException exception)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = exception.StatusCode,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Не удалось обновить задание",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }


        public async Task<Response> RemoveObjectiveAsync(int objectiveId)
        {
            try
            {
                var lessonObjective = await unitOfWork.LessonObjectives.FirstOrDefaultAsync(lessonObjective => lessonObjective.ObjectiveId == objectiveId);

                NotFoundException.ThrowIfNotFound(lessonObjective, "Урок, к которому прикреплено задание, не найден");

                await unitOfWork.LessonObjectives.Entry(lessonObjective)
                    .Reference(lessonObjective => lessonObjective.Objective)
                    .LoadAsync();

                await unitOfWork.Objectives.Entry(lessonObjective.Objective)
                    .Reference(objective => objective.FileContent)
                    .LoadAsync();

                if (lessonObjective.Objective.FileContent != null)
                {
                    contentInteractor.RemoveObjectiveFileContent(
                        lessonObjective.LessonId,
                        lessonObjective.Objective.Id,
                        lessonObjective.Objective.FileContent.Id,
                        lessonObjective.Objective.FileContent.FileName
                     );
                }


                unitOfWork.Objectives.Remove(lessonObjective.Objective);
                unitOfWork.LessonObjectives.Remove(lessonObjective);

                return new Response()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Задание успешно удалено",
                };
            }
            catch (CustomException exception)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = exception.StatusCode,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Не удалось удалить задание",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        private async Task SaveObjectiveFileContentAsync(int lessonId, Objective objective, IFormFile file)
        {
            if (objective.FileContent == null)
            {
                return;
            }

            await using var stream = file.OpenReadStream();

            if (stream == null || string.IsNullOrWhiteSpace(objective.FileContent.FileName))
            {
                throw new ValidationException("Файл или его название было пустое");
            }

            var directory = directoryStore.GetDirectoryPathToLessonObjectiveContent(lessonId, objective.Id, objective.FileContent.Id);
            var contentPath = Path.Combine(directory, objective.FileContent.FileName);

            Directory.CreateDirectory(Path.GetDirectoryName(contentPath)!);
            await using var fileStream = new FileStream(contentPath, FileMode.Create);
            await stream.CopyToAsync(fileStream);
        }

        public async Task RemoveObjectivesFromLessonAsyncNoResponse(int lessonId)
        {
            var lessonObjectives = unitOfWork.LessonObjectives
                .Where(lessonObjective => lessonObjective.LessonId == lessonId)
                .Include(lessonObjective => lessonObjective.Objective);

            var objectives = await lessonObjectives
                .Select(lessonSection => lessonSection.Objective)
                .ToListAsync();

            foreach (var objective in objectives)
            {
                await unitOfWork.Objectives.Entry(objective)
                    .Reference(section => section.FileContent)
                    .LoadAsync();

                if (objective.FileContent != null)
                {
                    contentInteractor.RemoveObjectiveFileContent(
                        lessonId, 
                        objective.Id, 
                        objective.FileContent.Id, 
                        objective.FileContent.FileName
                     );
                }

            }

            unitOfWork.LessonObjectives.RemoveRange(lessonObjectives);
            unitOfWork.Objectives.RemoveRange(objectives);
        }
    }
}
