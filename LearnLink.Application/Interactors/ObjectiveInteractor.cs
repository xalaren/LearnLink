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
    public class ObjectiveInteractor(IUnitOfWork unitOfWork, DirectoryStore directoryStore)
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
                    Message = "errorMessage",
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
                NotFoundException.ThrowIfNull(lesson, "Урок не был найден");

                var objective = objectiveDto.ToEntity();

                await unitOfWork.Objectives.AddAsync(objective);
                await unitOfWork.CommitAsync();

                var lessonObjective = new LessonObjective()
                {
                    Lesson = lesson,
                    Objective = objective
                };

                await unitOfWork.CommitAsync();

                if (objectiveDto.FormFile != null)
                {
                    await SaveObjectiveFileContentAsync(lessonId, objective, objectiveDto.FormFile);
                }

                return new Response()
                {
                    Success = true,
                    StatusCode = 200,
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
    }
}
