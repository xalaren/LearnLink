using LearnLink.Application.Mappers;
using LearnLink.Application.Transaction;
using LearnLink.Core.Entities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Interactors
{
    public class ModuleInteractor(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<Response> CreateModuleAsync(int courseId, ModuleDto moduleDto)
        {
            try
            {
                if (moduleDto == null)
                {
                    throw new ArgumentNullException(nameof(moduleDto), "ModuleDto was null");
                }

                var moduleEntity = moduleDto.ToEntity();

                var course = await unitOfWork.Courses.FindAsync(courseId);

                if (course == null)
                {
                    throw new NotFoundException("Курс не найден");
                }

                var courseModule = new CourseModule()
                {
                    Module = moduleEntity,
                    Course = course,
                };

                await unitOfWork.Modules.AddAsync(moduleEntity);
                await unitOfWork.CourseModules.AddAsync(courseModule);

                var courseCompletions = unitOfWork.CourseCompletions.Where(courseCompletion => courseCompletion.CourseId == courseId).Include(courseCompletion => courseCompletion.User);

                if (courseCompletions.Count() > 0)
                {
                    var moduleCompletions = courseCompletions.Select(sub => new ModuleCompletion()
                    {
                        User = sub.User,
                        Module = moduleEntity,
                        Completed = false,
                        CompletionProgress = 0,
                    });

                    await unitOfWork.ModuleCompletions.AddRangeAsync(moduleCompletions);
                }

                await unitOfWork.CommitAsync();

                return new Response()
                {
                    Success = true,
                    Message = "Модуль успешно создан",
                };
            }
            catch (CustomException exception)
            {
                return new Response()
                {
                    Success = false,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new Response()
                {
                    Success = false,
                    Message = "Не удалось создать модуль",
                    InnerErrorMessages = new string[] { exception.Message }
                };
            }
        }

        public async Task<Response<ModuleDto[]>> GetAllModulesAsync()
        {
            try
            {
                var modules = await unitOfWork.Modules.Select(module => module.ToDto()).ToArrayAsync();

                return new()
                {
                    Success = true,
                    Message = "Модули успешно получены",
                    Value = modules,
                };
            }
            catch (CustomException exception)
            {
                return new()
                {
                    Success = false,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    Message = "Не удалось получить модули",
                    InnerErrorMessages = new string[] { exception.Message }
                };
            }
        }

        public async Task<Response<ModuleDto[]>> GetCourseModulesAsync(int courseId)
        {
            try
            {
                var modules = await unitOfWork.CourseModules
                    .AsNoTracking()
                    .Where(m => m.CourseId == courseId)
                    .Join(
                        unitOfWork.Modules.AsNoTracking(),
                        courseModule => courseModule.ModuleId,
                        module => module.Id,
                        (courseModule, module) => module
                    )
                    .Select(module => module.ToDto())
                    .ToArrayAsync();

                return new()
                {
                    Success = true,
                    Message = "Модули успешно получены",
                    Value = modules,
                };
            }
            catch (CustomException exception)
            {
                return new()
                {
                    Success = false,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    Message = "Не удалось получить модули",
                    InnerErrorMessages = new string[] { exception.Message }
                };
            }
        }

        public async Task<Response<ModuleDto?>> GetModuleAsync(int moduleId)
        {
            try
            {
                var module = await unitOfWork.Modules.AsNoTracking().FirstOrDefaultAsync(module => module.Id == moduleId);

                if (module == null)
                {
                    throw new NotFoundException("Модуль не найден");
                }

                return new()
                {
                    Success = true,
                    Message = "Модуль успешно получен",
                    Value = module.ToDto(),
                };
            }
            catch (CustomException exception)
            {
                return new()
                {
                    Success = false,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    Message = "Не удалось получить модуль",
                    InnerErrorMessages = new string[] { exception.Message }
                };
            }
        }

        public async Task<Response> UpdateModuleAsync(ModuleDto moduleDto)
        {
            try
            {
                if (moduleDto == null)
                {
                    throw new ArgumentNullException(nameof(moduleDto), "ModuleDto was null");
                }

                var module = await unitOfWork.Modules.FindAsync(moduleDto.Id);

                if (module == null)
                {
                    throw new NotFoundException("Модуль не найден");
                }

                module.Assign(moduleDto);
                unitOfWork.Modules.Update(module);

                await unitOfWork.CommitAsync();

                return new()
                {
                    Success = true,
                    Message = "Модуль успешно изменен",
                };
            }
            catch (CustomException exception)
            {
                return new()
                {
                    Success = false,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    Message = "Не удалось изменить модуль",
                    InnerErrorMessages = [exception.Message]
                };
            }
        }

        public async Task<Response> RemoveModuleAsync(int moduleId)
        {
            try
            {
                var existModule = await unitOfWork.Modules.FindAsync(moduleId);


                if (existModule == null)
                {
                    throw new NotFoundException("Модуль не найден");
                }

                unitOfWork.Modules.Remove(existModule);

                await unitOfWork.CommitAsync();

                return new()
                {
                    Success = true,
                    Message = "Модуль успешно удален",
                };
            }
            catch (CustomException exception)
            {
                return new()
                {
                    Success = false,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new()
                {
                    Success = false,
                    Message = "Не удалось удалить модуль",
                    InnerErrorMessages = new string[] { exception.Message }
                };
            }
        }
    }
}
