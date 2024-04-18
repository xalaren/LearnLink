using LearnLink.Application.Mappers;
using LearnLink.Application.Transaction;
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

        public async Task<Response> CreateSectionAsync(int lessonId, SectionDto sectionDto)
        {
            try
            {
                var lesson = await unitOfWork.Lessons.FirstOrDefaultAsync(lesson => lesson.Id == lessonId);

                if (lesson == null) throw new NotFoundException("Урок не был найден");

                var content = sectionDto.ContentDto;
                var section = sectionDto.ToEntity(lesson);

                if(section.IsText)
                {
                    section.IsCodeBlock = false;
                    section.IsFile = false;
                }
                else if (section.IsCodeBlock)
                {
                    section.IsText = false;
                    section.IsFile = false;
                }
                else if (section.IsFile)
                {
                    section.IsText = false;
                    section.IsCodeBlock = false;
                }

                await unitOfWork.Sections.AddAsync(section);
                await UpdateSectionOrders(section.LessonId);
                await unitOfWork.CommitAsync();

                await contentInteractor.SaveLessonContentAsync(content, section.LessonId, section.Id);


                return new Response()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Раздел успешно создан",
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

        public async Task RemoveSectionAsyncNoResponse(int sectionId, bool strictMode)
        {
            var section = await unitOfWork.Sections.FindAsync(sectionId);

            if (section == null && strictMode) throw new NotFoundException("Раздел не найден");

            if (section == null) return;


            unitOfWork.Sections.Remove(section);
            contentInteractor.RemoveLessonContent(section.LessonId, section.Id, section.FileName);
        }

        public async Task RemoveLessonSectionsAsyncNoResponse(int lessonId)
        {
            var sections = await unitOfWork.Sections
                .Where(section => section.LessonId == lessonId)
                .ToListAsync();

            foreach (var section in sections)
            {
                contentInteractor.RemoveLessonContent(section.LessonId, section.Id, section.FileName);
            }


            unitOfWork.Sections.RemoveRange(sections);
        }

        public async Task<Response<SectionDto[]>> GetLessonSections(int lessonId)
        {
            try
            {
                var sections = 
                    await unitOfWork.Sections.Where(section => section.LessonId == lessonId)
                    .OrderBy(section => section.Order)
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
