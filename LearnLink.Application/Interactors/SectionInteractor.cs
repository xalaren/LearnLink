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

        public async Task<Response<SectionDto[]>> GetSectionsFromLessonAsync(int lessonId)
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

        public async Task<Response<SectionDto>> GetSectionByLessonAndOrderAsync(int lessonId, int order)
        {
            try
            {
                var section = await unitOfWork.Sections.FirstOrDefaultAsync(section => section.LessonId == lessonId && section.Order == order);

                if (section == null)
                {
                    throw new NotFoundException("Раздел не найден");
                }

                return new Response<SectionDto>()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Раздел получен успешно",
                    Value = section.ToDto()
                };
            }
            catch (CustomException exception)
            {
                return new Response<SectionDto>()
                {
                    Success = false,
                    StatusCode = exception.StatusCode,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new Response<SectionDto>()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Не удалось получить раздел",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response> CreateSectionAsync(int lessonId, SectionDto sectionDto)
        {
            try
            {
                var lesson = await unitOfWork.Lessons.FirstOrDefaultAsync(lesson => lesson.Id == lessonId);

                if (lesson == null) throw new NotFoundException("Урок не был найден");

                var content = sectionDto.Content;
                var section = sectionDto.ToEntity(lesson);

                if (section.IsText)
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

                var maxOrder = await unitOfWork.Sections
                    .Where(s => s.LessonId == lessonId)
                    .MaxAsync(s => (int?)s.Order) ?? 0;

                section.Order = maxOrder + 1;

                await unitOfWork.Sections.AddAsync(section);
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

        public async Task<Response> UpdateSectionAsync(int lessonId, SectionDto sectionDto)
        {
            try
            {
                var lesson = await unitOfWork.Lessons.FirstOrDefaultAsync(lesson => lesson.Id == lessonId);

                if (lesson == null) throw new NotFoundException("Урок не был найден");

                var section = await unitOfWork.Sections.FindAsync(sectionDto.Id);

                if (section == null) throw new NotFoundException("Раздел не был найден");

                

                var content = sectionDto.Content;
                var prevSectionFileState = section.IsFile;
                var prevSectionFileName = section.FileName;

                section.Assign(sectionDto);
                

                if ((prevSectionFileState && !section.IsFile) || (prevSectionFileState && section.IsFile && sectionDto.Content.FormFile != null))
                {
                    contentInteractor.RemoveLessonContent(section.LessonId, section.Id, prevSectionFileName);
                }

                if (section.IsText)
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

        public async Task<Response> RemoveSectionAsync(int sectionId)
        {
            try
            {
                await RemoveSectionAsyncNoResponse(sectionId, true);
                await unitOfWork.CommitAsync();

                return new Response()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Раздел успешно удален"
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
                    Message = "Не удалось удалить раздел",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        //TODO: make remove file content from section

        public async Task RemoveSectionAsyncNoResponse(int sectionId, bool strictMode)
        {
            var section = await unitOfWork.Sections.FindAsync(sectionId);

            if (section == null && strictMode) throw new NotFoundException("Раздел не найден");

            if (section == null) return;

            await UpdateSectionOrders(section.LessonId, section.Order);
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

        public async Task<Response> ChangeOrder(int sectionId, int lessonId, bool increase)
        {
            try
            {
                var foundSection = await unitOfWork.Sections.FindAsync(sectionId);

                if (foundSection == null)
                {
                    throw new NotFoundException("Раздел не найден");
                }

                int nextOrderModifier = 1;

                if (increase)
                {
                    nextOrderModifier = -1;
                }

                var next = unitOfWork.Sections.FirstOrDefault(section => section.LessonId == lessonId && section.Order == foundSection.Order + nextOrderModifier);

                if (next == null)
                {
                    throw new OrderRangeEndException($"Раздел находится в {(increase ? "начале списка" : "конце списка")}");
                }

                foundSection.Order += nextOrderModifier;
                next.Order -= nextOrderModifier;

                unitOfWork.Sections.Update(foundSection);
                unitOfWork.Sections.Update(next);

                await unitOfWork.CommitAsync();

                return new Response()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Порядок разделов успешно изменен",
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
                    Message = "Не удалось изменить порядок раздела",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task UpdateSectionOrders(int lessonId, int order = 0)
        {
            var lessonSections = await unitOfWork.Sections.Where(section => section.LessonId == lessonId).ToListAsync();

            for (int i = order; i < lessonSections.Count; i++)
            {
                lessonSections[i].Order = i;
                unitOfWork.Sections.Update(lessonSections[i]);
            }
        }
    }
}
