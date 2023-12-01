using CoursesPrototype.Application.Mappers;
using CoursesPrototype.Application.Repository;
using CoursesPrototype.Application.Transaction;
using CoursesPrototype.Core.Entities;
using CoursesPrototype.Core.Exceptions;
using CoursesPrototype.Shared.DataTransferObjects;
using CoursesPrototype.Shared.Responses;

namespace CoursesPrototype.Application.Interactors
{
    public class ModuleInteractor
    {
        private readonly IModulesRepository modulesRepository;
        private readonly ICourseRepository coursesRepository;
        private readonly ICourseModuleRepository courseModuleRepository;
        private readonly IUnitOfWork unitOfWork;

        public ModuleInteractor(IModulesRepository modulesRepository, ICourseRepository coursesRepository, ICourseModuleRepository courseModuleRepository, IUnitOfWork unitOfWork)
        {
            this.modulesRepository = modulesRepository;
            this.coursesRepository = coursesRepository;
            this.courseModuleRepository = courseModuleRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Response> CreateModuleAsync(int courseId, ModuleDto moduleDto)
        {
            try
            {
                if (moduleDto == null)
                {
                    throw new ArgumentNullException(nameof(moduleDto), "ModuleDto was null");
                }

                var moduleEntity = moduleDto.ToEntity();

                var course = await coursesRepository.GetAsync(courseId);

                if(course == null)
                {
                    throw new NotFoundException("Курс не найден");
                }

                var courseModule = new CourseModule()
                {
                    Module = moduleEntity,
                    Course = course,
                };

                await modulesRepository.CreateAsync(moduleEntity);
                await courseModuleRepository.CreateAsync(courseModule);

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
                var modules = await modulesRepository.GetAllModulesAsync();

                return new()
                {
                    Success = true,
                    Message = "Модули успешно получены",
                    Value = modules.Select(module => module.ToDto()).ToArray(),
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
                var courseModules = await courseModuleRepository.GetCourseModulesByCourseAsync(courseId);
                var modules = await modulesRepository.GetModulesFromCourseModules(courseModules);

                return new()
                {
                    Success = true,
                    Message = "Модули успешно получены",
                    Value = modules.Select(module => module.ToDto()).ToArray(),
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
                var module = await modulesRepository.GetAsync(moduleId);

                if(module == null)
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

                var module = await modulesRepository.GetAsync(moduleDto.Id);

                if (module == null)
                {
                    throw new NotFoundException("Модуль не найден");
                }

                module.Assign(moduleDto);

                modulesRepository.Update(module);

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
                    InnerErrorMessages = new string[] { exception.Message }
                };
            }
        }

        public async Task<Response> RemoveModuleAsync(int moduleId)
        {
            try
            {
                await modulesRepository.RemoveAsync(moduleId);

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
