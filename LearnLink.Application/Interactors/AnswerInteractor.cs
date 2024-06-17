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
    public class AnswerInteractor(
        IUnitOfWork unitOfWork,
        ContentInteractor contentInteractor,
        PermissionService permissionService,
        DirectoryStore directoryStore)
    {

        public async Task<Response<AnswerDto>> RequestGetAnswerAsync(int requesterUserId, int courseId, int lessonId, int answerId)
        {
            try
            {
                var manageInternalPermission = await permissionService.GetPermissionAsync(
                    userId: requesterUserId,
                    courseId: courseId,
                    toManageInternal: true);

                var answer = await unitOfWork.Answers.FindAsync(answerId);

                NotFoundException.ThrowIfNotFound(answer, "Ответ к заданию не найден");

                if (answer.UserId != requesterUserId && !manageInternalPermission.AccessGranted)
                {
                    throw new AccessLevelException("Недостаточный уровень прав для просмотра ответа");
                }

                await unitOfWork.Answers.Entry(answer)
                    .Reference(answer => answer.TextContent)
                    .LoadAsync();

                await unitOfWork.Answers.Entry(answer)
                    .Reference(answer => answer.FileContent)
                    .LoadAsync();

                await unitOfWork.Answers.Entry(answer)
                    .Reference(answer => answer.User)
                    .LoadAsync();

                return new Response<AnswerDto>()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Ответ к заданию получен успешно",
                    Value = answer.ToDto(lessonId)
                };
            }
            catch (CustomException exception)
            {
                return new Response<AnswerDto>()
                {
                    Success = false,
                    StatusCode = exception.StatusCode,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new Response<AnswerDto>()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Не удалось получить ответ к заданию",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }


        public async Task<Response<DataPage<AnswerDto[]>>> RequestGetObjectiveAnswers(
            int requesterUserId,
            int courseId,
            int lessonId,
            int objectiveId,
            DataPageHeader pageHeader
            )
        {
            try
            {
                var manageInternalPermission = await permissionService.GetPermissionAsync(
                    userId: requesterUserId,
                    courseId: courseId,
                    toManageInternal: true);

                int total = 0;

                List<Answer> answers = new List<Answer>();

                if (manageInternalPermission.AccessGranted)
                {
                    total = await unitOfWork.Answers
                        .AsNoTracking()
                        .Where(answer => answer.ObjectiveId == objectiveId)
                        .CountAsync();

                    answers = await unitOfWork.Answers
                        .AsNoTracking()
                        .Where(answer => answer.ObjectiveId == objectiveId)
                        .Skip((pageHeader.PageNumber - 1) * pageHeader.PageSize)
                        .Take(pageHeader.PageSize)
                        .OrderByDescending(answer => answer.UploadDate)
                        .ToListAsync();
                }
                else
                {
                    answers = await unitOfWork.Answers
                        .AsNoTracking()
                        .Where(answer =>
                            answer.ObjectiveId == objectiveId &&
                            answer.UserId == requesterUserId
                        )
                        .ToListAsync();

                    total = answers.Count;
                }

                foreach (var answer in answers)
                {
                    await unitOfWork.Answers.Entry(answer)
                        .Reference(answer => answer.TextContent)
                        .LoadAsync();

                    await unitOfWork.Answers.Entry(answer)
                        .Reference(answer => answer.FileContent)
                        .LoadAsync();

                    await unitOfWork.Answers.Entry(answer)
                        .Reference(answer => answer.User)
                        .LoadAsync();
                }

                var dataPage = new DataPage<AnswerDto[]>()
                {
                    ItemsCount = total,
                    PageNumber = pageHeader.PageNumber,
                    PageSize = pageHeader.PageSize,
                    Values = answers.Select(answer => answer.ToDto(lessonId)).ToArray()
                };

                return new()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Ответ к заданию получен успешно",
                    Value = dataPage
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
                    Message = "Не удалось получить ответ к заданию",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response> CreateAnswerAsync(int lessonId, AnswerDto answerDto)
        {
            try
            {
                var existingAnswer = await unitOfWork.Answers.FirstOrDefaultAsync(answer =>
                        answer.UserId == answerDto.UserId &&
                        answer.ObjectiveId == answerDto.ObjectiveId
                );

                if (existingAnswer != null)
                {
                    throw new ValidationException("Вы уже добавили ответ на это задание");
                }

                var answer = answerDto.ToEntity();

                await unitOfWork.Answers.AddAsync(answer);
                await unitOfWork.CommitAsync();

                if (answerDto.FormFile != null)
                {
                    await SaveAnswerFileContentAsync(lessonId, answer, answerDto.FormFile);
                }

                return new Response()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Ответ на задание успешно записан"
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
                    InnerErrorMessages = [exception.Message],
                };
            }
        }

        public async Task<Response> UpdateAnswerAsync(int lessonId, AnswerDto answerDto, bool removeFileContent)
        {
            try
            {
                var answer = await unitOfWork.Answers.FirstOrDefaultAsync(answer =>
                     answer.Id == answerDto.Id
                 );

                NotFoundException.ThrowIfNotFound(answer, "Ответ к заданию не найден");

                await unitOfWork.Answers.Entry(answer)
                    .Reference(answer => answer.FileContent)
                    .LoadAsync();

                await unitOfWork.Answers.Entry(answer)
                    .Reference(answer => answer.TextContent)
                    .LoadAsync();

                var prevFileState = answer.FileContent;
                var prevTextState = answer.TextContent;

                if (removeFileContent)
                {
                    answer.FileContent = null;
                }

                answer = answer.Assign(answerDto);

                if (prevFileState != null && (answer.FileContent != null || removeFileContent))
                {
                    contentInteractor.RemoveAnswerFileContent(
                        lessonId,
                        answer.ObjectiveId,
                        answer.Id,
                        prevFileState.Id,
                        prevFileState.FileName);

                    unitOfWork.FileContents.Remove(prevFileState);
                    await unitOfWork.CommitAsync();
                }

                if (prevTextState != null && answer.TextContent == null)
                {
                    unitOfWork.TextContents.Remove(prevTextState);
                    await unitOfWork.CommitAsync();
                }

                unitOfWork.Answers.Update(answer);
                await unitOfWork.CommitAsync();

                if (answerDto.FormFile != null)
                {
                    await SaveAnswerFileContentAsync(lessonId, answer, answerDto.FormFile);
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

        public async Task<Response> RemoveAnswerAsync(int lessonId, int answerId)
        {
            try
            {
                var answer = await unitOfWork.Answers.FirstOrDefaultAsync(answer =>
                    answer.Id == answerId
                );

                NotFoundException.ThrowIfNotFound(answer, "Ответ к заданию не найден");

                await unitOfWork.Answers.Entry(answer)
                    .Reference(answer => answer.FileContent)
                    .LoadAsync();

                await unitOfWork.Answers.Entry(answer)
                    .Reference(answer => answer.TextContent)
                    .LoadAsync();

                if (answer.FileContent != null)
                {
                    contentInteractor.RemoveAnswerFileContent(
                        lessonId,
                        answer.ObjectiveId,
                        answer.Id,
                        answer.FileContent.Id,
                        answer.FileContent.FileName);

                    unitOfWork.FileContents.Remove(answer.FileContent);
                }

                if (answer.TextContent != null)
                {
                    unitOfWork.TextContents.Remove(answer.TextContent);
                }

                unitOfWork.Answers.Remove(answer);
                await unitOfWork.CommitAsync();

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

        public async Task RemoveAnswersFromObjective(int lessonId, int objectiveId)
        {
            var answers = await unitOfWork.Answers
                .Where(answer => answer.ObjectiveId == objectiveId)
                .ToListAsync();

            foreach (var answer in answers)
            {
                await unitOfWork.Answers.Entry(answer)
                    .Reference(answer => answer.FileContent)
                    .LoadAsync();

                await unitOfWork.Answers.Entry(answer)
                    .Reference(answer => answer.TextContent)
                    .LoadAsync();

                if (answer.FileContent != null)
                {
                    contentInteractor.RemoveAnswerFileContent(
                        lessonId,
                        answer.ObjectiveId,
                        answer.Id,
                        answer.FileContent.Id,
                        answer.FileContent.FileName);

                    unitOfWork.FileContents.Remove(answer.FileContent);
                }

                if (answer.TextContent != null)
                {
                    unitOfWork.TextContents.Remove(answer.TextContent);
                }
            }
        }

        private async Task SaveAnswerFileContentAsync(int lessonId, Answer answer, IFormFile file)
        {
            if (answer.FileContent == null)
            {
                return;
            }

            await using var stream = file.OpenReadStream();

            if (stream == null || string.IsNullOrWhiteSpace(answer.FileContent.FileName))
            {
                throw new ValidationException("Файл или его название было пустое");
            }

            var directory = directoryStore.GetDirectoryPathToObjectiveAnswerContent(lessonId, answer.ObjectiveId, answer.Id, answer.FileContent.Id);
            var contentPath = Path.Combine(directory, answer.FileContent.FileName);

            Directory.CreateDirectory(Path.GetDirectoryName(contentPath)!);
            await using var fileStream = new FileStream(contentPath, FileMode.Create);
            await stream.CopyToAsync(fileStream);
        }

    }
}
