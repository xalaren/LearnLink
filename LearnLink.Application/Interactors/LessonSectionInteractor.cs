using LearnLink.Application.Mappers;
using LearnLink.Application.Transaction;
using LearnLink.Core.Entities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

namespace LearnLink.Application.Interactors
{
    public class LessonSectionInteractor(IUnitOfWork unitOfWork, ContentInteractor contentInteractor)
    {

        public async Task<Response<SectionDto[]>> GetFromLessonAsync(int lessonId)
        {
            try
            {
                var sections =
                    await unitOfWork.LessonSections
                        .Where(lessonSection => lessonSection.LessonId == lessonId)
                        .Include(lessonSection => lessonSection.Section)
                        .Select(lessonSection => lessonSection.Section)
                        .OrderBy(section => section.Order)
                        .ToArrayAsync();

                foreach(var section in sections)
                {
                    await unitOfWork.Sections.Entry(section)
                        .Reference(section => section.TextContent)
                        .LoadAsync();

                    await unitOfWork.Sections.Entry(section)
                        .Reference(section => section.CodeContent)
                        .LoadAsync();

                    await unitOfWork.Sections.Entry(section)
                        .Reference(section => section.FileContent)
                        .LoadAsync();
                }

                return new Response<SectionDto[]>()
                {
                    Success = true,
                    StatusCode = 200,
                    Value = sections.Select(section => section.ToDto()).ToArray()
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
                var section = await unitOfWork
                    .LessonSections
                    .Where(lessonSection => lessonSection.LessonId == lessonId)
                    .Include(lessonSection => lessonSection.Section)
                    .Select(lessonSection => lessonSection.Section)
                    .FirstOrDefaultAsync(section => section.Order == order);

                if (section == null)
                {
                    throw new NotFoundException("Раздел не найден");
                }

                await unitOfWork.Sections.Entry(section)
                    .Reference(section => section.TextContent)
                    .LoadAsync();

                await unitOfWork.Sections.Entry(section)
                    .Reference(section => section.CodeContent)
                    .LoadAsync();

                await unitOfWork.Sections.Entry(section)
                    .Reference(section => section.FileContent)
                    .LoadAsync();

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

        public async Task<Response> CreateLessonSectionAsync(int lessonId, SectionDto sectionDto)
        {
            try
            {
                var lesson = await unitOfWork.Lessons.FirstOrDefaultAsync(lesson => lesson.Id == lessonId);

                if (lesson == null) throw new NotFoundException("Урок не был найден");

                var content = sectionDto.Content;
                var section = sectionDto.ToEntity();

                var maxOrder = await unitOfWork
                    .LessonSections
                    .Where(s => s.LessonId == lessonId)
                    .Include(s => s.Section)
                    .Select(s => s.Section)
                    .MaxAsync(s => (int?)s.Order) ?? 0;

                section.Order = maxOrder + 1;

                await unitOfWork.Sections.AddAsync(section);
                await unitOfWork.CommitAsync();

                var lessonSection = new LessonSection()
                {
                    Lesson = lesson,
                    Section = section
                };

                await unitOfWork.LessonSections.AddAsync(lessonSection);
                await unitOfWork.CommitAsync();

                if(content != null && section.FileContent != null)
                {
                    await contentInteractor.SaveLessonFileContent(content, lessonId, section.Id, section.FileContent.Id);
                }

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

        public async Task<Response> UpdateLessonSectionAsync(int lessonId, SectionDto sectionDto)
        {
            try
            {
                var lesson = await unitOfWork.Lessons.FirstOrDefaultAsync(lesson => lesson.Id == lessonId);

                if (lesson == null) throw new NotFoundException("Урок не был найден");

                var section = await unitOfWork.Sections.FindAsync(sectionDto.Id);

                if (section == null) throw new NotFoundException("Раздел не был найден");

                await unitOfWork.Sections.Entry(section)
                    .Reference(section => section.TextContent)
                    .LoadAsync();

                await unitOfWork.Sections.Entry(section)
                    .Reference(section => section.CodeContent)
                    .LoadAsync();

                await unitOfWork.Sections.Entry(section)
                    .Reference(section => section.FileContent)
                    .LoadAsync();


                if (sectionDto.Content == null)
                {
                    var updatedSection = section.Assign(sectionDto);
                    
                    unitOfWork.Sections.Update(updatedSection);
                    await unitOfWork.CommitAsync();
                }
                else
                {
                    var content = sectionDto.Content;
                    
                    bool prevSectionFileState = section.FileContent == null;
                    string? prevSectionFileName = section.FileContent?.FileName;
                    int prevContentId = section.FileContent?.Id ?? 0;
                    
                    var updatedSection = section.Assign(sectionDto);


                    if ((prevSectionFileState && updatedSection.FileContent != null) || (prevSectionFileState && updatedSection.FileContent != null && sectionDto.Content.FormFile != null))
                    {
                        contentInteractor.RemoveLessonFileContent(lessonId, section.Id, prevContentId, prevSectionFileName);
                    }

                    unitOfWork.Sections.Update(updatedSection);
                    await unitOfWork.CommitAsync();

                    if (updatedSection.FileContent != null)
                    {
                        await contentInteractor.SaveLessonFileContent(content, lessonId, updatedSection.Id, updatedSection.FileContent.Id);
                    }
                }

                return new Response()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Раздел успешно обновлен",
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
                    Message = "Не удалось обновить раздел",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response> RemoveLessonSectionAsync(int sectionId, int lessonId)
        {
            try
            {
                var lessonSection = await unitOfWork.LessonSections.FirstOrDefaultAsync(lessonSection =>
                lessonSection.LessonId == lessonId &&
                lessonSection.SectionId == sectionId);

                if(lessonSection == null)
                {
                    throw new NotFoundException("Раздел не найден");
                }

                await unitOfWork.LessonSections.Entry(lessonSection)
                    .Reference(lessonSection => lessonSection.Section)
                    .LoadAsync();

                await unitOfWork.Sections.Entry(lessonSection.Section)
                    .Reference(section => section.TextContent)
                    .LoadAsync();

                await unitOfWork.Sections.Entry(lessonSection.Section)
                    .Reference(section => section.CodeContent)
                    .LoadAsync();

                await unitOfWork.Sections.Entry(lessonSection.Section)
                    .Reference(section => section.FileContent)
                    .LoadAsync();

                if (lessonSection.Section.FileContent != null)
                {
                    contentInteractor.RemoveLessonFileContent(
                        lessonId,
                        sectionId,
                        lessonSection.Section.FileContent.Id,
                        lessonSection.Section.FileContent.FileName);

                    unitOfWork.FileContents.Remove(lessonSection.Section.FileContent);
                }

                if(lessonSection.Section.TextContent != null)
                {
                    unitOfWork.TextContents.Remove(lessonSection.Section.TextContent);
                }

                if(lessonSection.Section.CodeContent != null)
                {
                    unitOfWork.CodeContents.Remove(lessonSection.Section.CodeContent);
                }

                unitOfWork.LessonSections.Remove(lessonSection);
                unitOfWork.Sections.Remove(lessonSection.Section);
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

        public async Task RemoveSectionsFromLessonAsyncNoResponse(int lessonId)
        {
            var lessonSections = unitOfWork.LessonSections
                .Where(lessonSection => lessonSection.LessonId == lessonId)
                .Include(lessonSection => lessonSection.Section);

            var sections = await lessonSections
                .Select(lessonSection => lessonSection.Section)
                .ToListAsync();

            foreach (var section in sections)
            {
                await unitOfWork.Sections.Entry(section)
                    .Reference(section => section.TextContent)
                    .LoadAsync();

                await unitOfWork.Sections.Entry(section)
                    .Reference(section => section.CodeContent)
                    .LoadAsync();

                await unitOfWork.Sections.Entry(section)
                    .Reference(section => section.FileContent)
                    .LoadAsync();

                if (section.FileContent == null) continue;

                contentInteractor.RemoveLessonFileContent(lessonId, section.Id, section.FileContent.Id, section.FileContent.FileName);
            }

            unitOfWork.LessonSections.RemoveRange(lessonSections);
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

                var next = unitOfWork.LessonSections
                    .Where(lessonSection => lessonSection.LessonId == lessonId)
                    .Include(lessonSection => lessonSection.Section)
                    .Select(lessonSection => lessonSection.Section)
                    .FirstOrDefault(section => section.Order == foundSection.Order + nextOrderModifier);

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
            var sections = await unitOfWork.LessonSections
                .Where(lessonSection => lessonSection.LessonId == lessonId)
                .Include(lessonSection => lessonSection.Section)
                .Select(lessonSection => lessonSection.Section)
                .ToListAsync();

            for (int i = order; i < sections.Count; i++)
            {
                sections[i].Order = i;
                unitOfWork.Sections.Update(sections[i]);
            }
        }
    }
}
