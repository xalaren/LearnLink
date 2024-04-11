using System.Runtime.CompilerServices;
using LearnLink.Application.Helpers;
using LearnLink.Application.Mappers;
using LearnLink.Application.Transaction;
using LearnLink.Core.Entities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Interactors
{
    public class SectionInteractor(IUnitOfWork unitOfWork, ContentInteractor contentInteractor)
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly ContentInteractor contentInteractor = contentInteractor;

        public async Task<Response> CreateSectionAsync(SectionDto sectionDto)
        {
            try
            {
                if(sectionDto.LessonId == 0)
                {
                    throw new ValidationException("Идентификатор урока не был заполнен");
                }

                var sectionEntity = sectionDto.ToEntity();

                if(sectionEntity.Content == null)
                {
                    throw new ValidationException("Содержимое не было заполнено");
                }

                var content = await contentInteractor.CreateContentWithResult(sectionDto.Content);

                sectionEntity.Content = content;

                await unitOfWork.Sections.AddAsync(sectionEntity);
                await UpdateSectionOrders(sectionEntity.LessonId);

                await unitOfWork.CommitAsync();

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
                    Message = "Не удалось создать раздел",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response<SectionDto[]>> GetLessonSections(int lessonId)
        {
            try
            {
                var sections = 
                    await unitOfWork.Sections.Where(section => section.LessonId == lessonId)
                    .OrderBy(section => section.Order)
                    .Include(section => section.Content)
                    .Select(section => section.ToDto())
                    .ToArrayAsync();

                return new Response<SectionDto[]>()
                {
                    Success = true,
                    StatusCode = 200,
                    Value = sections
                };
            }
            catch (CustomException exception)
            {
                return new Response<SectionDto[]>()
                {
                    Success = false,
                    StatusCode = exception.StatusCode,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new Response<SectionDto[]>()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Не удалось получить разделы урока",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        private async Task UpdateSectionOrders(int lessonId)
        {
            var lessonSections = await unitOfWork.Sections.Where(section => section.LessonId == lessonId).ToListAsync();

            for(int i = 0; i < lessonSections.Count; i++) 
            {
                lessonSections[i].Order++;
                unitOfWork.Sections.Update(lessonSections[i]);
            }
        }
    }
}
