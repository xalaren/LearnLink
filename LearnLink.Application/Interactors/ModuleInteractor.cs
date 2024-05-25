﻿using LearnLink.Application.Mappers;
using LearnLink.Application.Transaction;
using LearnLink.Core.Entities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Interactors
{
    public class ModuleInteractor(IUnitOfWork unitOfWork, LessonInteractor lessonInteractor)
    {
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
                        Course = course,
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
                await RemoveModuleAsyncNoResponse(moduleId, true);
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

        public async Task<Response<ClientModuleDto[]>> RequestGetCourseModulesAsync(int courseId, int userId = 0)
        {
            try
            {
                if (userId == 0)
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
                        .ToArrayAsync();

                    return new()
                    {
                        Success = true,
                        Message = "Модули успешно получены",
                        Value = modules.Select(module => module.ToClientModule()).ToArray()
                    };
                }

                var user = await unitOfWork.Users.FirstOrDefaultAsync(user => user.Id == userId);

                if (user == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                var moduleCompletions = unitOfWork.ModuleCompletions
                    .Where(mc => mc.UserId == userId && mc.CourseId == courseId)
                    .Include(mc => mc.Module);

                var moduleCompletionsArray = await moduleCompletions.ToArrayAsync();

                var clientModules = moduleCompletionsArray.Select(mc => mc.Module.ToClientModule(mc)).ToArray();

                return new()
                {
                    Success = true,
                    Message = "Модули успешно получены",
                    Value = clientModules,
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

        public async Task<Response<ClientModuleDto?>> RequestGetModuleAsync(int userId, int courseId, int moduleId)
        {
            try
            {
                var userCourseLocalRole = await unitOfWork.UserCourseLocalRoles.FirstOrDefaultAsync(role =>
                    role.CourseId == courseId &&
                    role.UserId == userId);

                if (userCourseLocalRole == null)
                {
                    throw new NotFoundException("Локальная роль пользователя не найдена");
                }

                await unitOfWork.UserCourseLocalRoles.Entry(userCourseLocalRole)
                    .Reference(role => role.LocalRole)
                    .LoadAsync();

                await unitOfWork.UserCourseLocalRoles.Entry(userCourseLocalRole)
                    .Reference(role => role.Course)
                    .LoadAsync();

                if (userCourseLocalRole.Course.IsUnavailable)
                {
                    throw new AccessLevelException("Курс является недоступным");
                }

                if (!userCourseLocalRole.LocalRole.ViewAccess)
                {
                    throw new AccessLevelException("Вы не можете просматривать модуль");
                }

                var module = await unitOfWork.Modules.AsNoTracking().FirstOrDefaultAsync(module => module.Id == moduleId);

                if (module == null)
                {
                    throw new NotFoundException("Модуль не найден");
                }

                var moduleCompletion = await unitOfWork.ModuleCompletions.FirstOrDefaultAsync(moduleCompletion =>
                    moduleCompletion.UserId == userId &&
                    moduleCompletion.CourseId == courseId &&
                    moduleCompletion.ModuleId == moduleId);

                return new()
                {
                    Success = true,
                    Message = "Модуль успешно получен",
                    Value = module.ToClientModule(moduleCompletion),
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

        public async Task<Response> RequestCreateModuleAsync(int userId, int courseId, ModuleDto moduleDto)
        {
            try
            {
                var userCourseLocalRole = await unitOfWork.UserCourseLocalRoles.FirstOrDefaultAsync(role =>
                    role.UserId == userId &&
                    role.CourseId == courseId);

                if (userCourseLocalRole == null)
                {
                    throw new NotFoundException("Локальная роль пользователя не найдена");
                }

                await unitOfWork.UserCourseLocalRoles.Entry(userCourseLocalRole)
                    .Reference(role => role.LocalRole)
                    .LoadAsync();

                if (!userCourseLocalRole.LocalRole.ManageInternalAccess)
                {
                    throw new AccessLevelException("Вы не можете изменять материалы курса");
                }

                return await CreateModuleAsync(courseId, moduleDto);
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

        public async Task<Response> RequestUpdateModuleAsync(int userId, int courseId, ModuleDto moduleDto)
        {
            try
            {
                var userCourseLocalRole = await unitOfWork.UserCourseLocalRoles.FirstOrDefaultAsync(role =>
                    role.UserId == userId &&
                    role.CourseId == courseId);

                if (userCourseLocalRole == null)
                {
                    throw new NotFoundException("Локальная роль пользователя не найдена");
                }

                await unitOfWork.UserCourseLocalRoles.Entry(userCourseLocalRole)
                    .Reference(role => role.LocalRole)
                    .LoadAsync();

                if (!userCourseLocalRole.LocalRole.ManageInternalAccess)
                {
                    throw new AccessLevelException("Вы не можете изменять материалы курса");
                }

                return await UpdateModuleAsync(moduleDto);
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

        public async Task<Response> RequestRemoveModuleAsync(int userId, int courseId, int moduleId)
        {
            try
            {
                var userCourseLocalRole = await unitOfWork.UserCourseLocalRoles.FirstOrDefaultAsync(role =>
                    role.UserId == userId &&
                    role.CourseId == courseId);

                if (userCourseLocalRole == null)
                {
                    throw new NotFoundException("Локальная роль пользователя не найдена");
                }

                await unitOfWork.UserCourseLocalRoles.Entry(userCourseLocalRole)
                    .Reference(role => role.LocalRole)
                    .LoadAsync();

                if (!userCourseLocalRole.LocalRole.ManageInternalAccess)
                {
                    throw new AccessLevelException("Вы не можете изменять материалы курса");
                }

                return await RemoveModuleAsync(moduleId);
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

        public async Task RemoveModuleAsyncNoResponse(int moduleId, bool strictRemove)
        {
            var module = await unitOfWork.Modules.FindAsync(moduleId);

            if (module == null && strictRemove) throw new NotFoundException("Модуль не найден");

            if (module == null) return;

            var moduleLessons = await unitOfWork.ModuleLessons
                .Where(moduleLesson => moduleLesson.ModuleId == moduleId)
                .ToListAsync();

            foreach (var moduleLesson in moduleLessons)
            {
                await lessonInteractor.RemoveLessonAsyncNoResponse(moduleLesson.LessonId, false);
            }

            var completions = unitOfWork.ModuleCompletions.Where(moduleCompletion => moduleCompletion.ModuleId == moduleId);

            unitOfWork.Modules.Remove(module);
            unitOfWork.ModuleCompletions.RemoveRange(completions);
        }
    }
}
