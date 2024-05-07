using LearnLink.Application.Mappers;
using LearnLink.Application.Transaction;
using LearnLink.Core.Entities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Interactors
{
    public class LocalRoleInteractor
    {
        private readonly IUnitOfWork unitOfWork;

        public LocalRoleInteractor(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Response> CreateLocalRoleAsync(LocalRoleDto localRoleDto)
        {
            try
            {
                var existingRole = await unitOfWork.LocalRoles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(localRole => localRole.Sign == localRoleDto.Sign);

                if (existingRole != null)
                {
                    throw new ValidationException("Данная локальная роль уже добавлена в систему");
                }

                var localRoleEntity = localRoleDto.ToEntity();

                await unitOfWork.Roles.AddAsync(localRoleEntity);
                await unitOfWork.CommitAsync();

                return new Response()
                {
                    Success = true,
                    Message = "Роль успешно добавлена в систему",
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
                    Message = "Не удалось добавить роль. Внутренняя ошибка.",
                    InnerErrorMessages = [exception.Message]
                };
            }
        }

        public async Task<Response<LocalRoleDto[]>> GetAllLocalRolesAsync()
        {
            try
            {
                var localRoles = await unitOfWork.LocalRoles
                    .AsNoTracking()
                    .Select(role => role.ToDto())
                    .ToArrayAsync();

                return new()
                {
                    Success = true,
                    Message = "Локальные успешно получены",
                    Value = localRoles,
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
                    Message = "Не удалось получить локальные роли. Внутренняя ошибка.",
                    InnerErrorMessages = [exception.Message]
                };
            }
        }

        public async Task<Response<LocalRoleDto?>> GetLocalRoleByIdAsync(int id)
        {
            try
            {
                var localRole = await unitOfWork.LocalRoles.AsNoTracking().FirstOrDefaultAsync(role => role.Id == id);

                return new()
                {
                    Success = true,
                    Message = "Локальная роль успешно получена",
                    Value = localRole?.ToDto(),
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
                    Message = "Не удалось получить локальную роль. Внутренняя ошибка.",
                    InnerErrorMessages = [exception.Message]
                };
            }
        }

        public async Task<Response<LocalRoleDto?>> GetLocalRoleByNameAsync(string name)
        {
            try
            {
                var localRole = await unitOfWork.LocalRoles.AsNoTracking().FirstOrDefaultAsync(localRole => localRole.Name == name);

                return new()
                {
                    Success = true,
                    Message = "Локальная роль успешно получена",
                    Value = localRole?.ToDto(),
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
                    Message = "Не удалось получить локальную роль. Внутренняя ошибка.",
                    InnerErrorMessages = [exception.Message]
                };
            }
        }

        public async Task<Response<LocalRoleDto?>> GetLocalRoleBySignAsync(string sign)
        {
            try
            {
                var localRole = await unitOfWork.LocalRoles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(localRole => localRole.Sign == sign);

                return new()
                {
                    Success = true,
                    Message = "Локальная роль успешно получена",
                    Value = localRole?.ToDto(),
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
                    Message = "Не удалось получить локальную роль. Внутренняя ошибка.",
                    InnerErrorMessages = [exception.Message]
                };
            }
        }

        public async Task<Response<LocalRoleDto>> GetUserLocalRoleAtCourse(int courseId, int userId)
        {
            try
            {
                var userCourseLocalRole = await unitOfWork.UserCourseLocalRoles.FirstOrDefaultAsync(u => u.CourseId == courseId && u.UserId == userId);

                if (userCourseLocalRole == null)
                {
                    throw new NotFoundException("Роль пользователя не найдена");
                }

                await unitOfWork.UserCourseLocalRoles.Entry(userCourseLocalRole)
                    .Reference(u => u.LocalRole)
                    .LoadAsync();

                var localRole = userCourseLocalRole.LocalRole;

                return new()
                {
                    Success = true,
                    Message = "Локальная роль успешно получена",
                    Value = localRole?.ToDto(),
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
                    Message = "Не удалось получить локальную роль. Внутренняя ошибка.",
                    InnerErrorMessages = [exception.Message]
                };
            }
        }

        public async Task<Response> RemoveLocalRoleAsync(int id)
        {
            try
            {
                var localRole = await unitOfWork.LocalRoles.FindAsync(id);

                if (localRole == null)
                {
                    throw new NotFoundException("Локальная роль не найдена");
                }

                unitOfWork.Roles.Remove(localRole);
                await unitOfWork.CommitAsync();

                return new()
                {
                    Success = true,
                    Message = "Локальная роль успешно удалена",
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
                    Message = "Не удалось удалить локальную роль. Внутренняя ошибка.",
                    InnerErrorMessages = [exception.Message]
                };
            }
        }

        public async Task<Response> UpdateLocalRoleAsync(LocalRoleDto localRoleDto)
        {
            try
            {
                var localRole = await unitOfWork.LocalRoles.FindAsync(localRoleDto.Id);

                if (localRole == null)
                {
                    throw new NotFoundException("Локальная роль не найдена");
                }

                localRole = localRole.Assign(localRoleDto);

                unitOfWork.Roles.Update(localRole);
                await unitOfWork.CommitAsync();

                return new()
                {
                    Success = true,
                    Message = "Локальная роль успешно изменена",
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
                    Message = "Не удалось изменить локальную роль. Внутренняя ошибка.",
                    InnerErrorMessages = [exception.Message]
                };
            }
        }

        public async Task<Response> ReassignUserRoleAsync(int requesterUserId, int targetUserId, int courseId, int localRoleId)
        {
            try
            {
                var requesterUserLocalRole = await unitOfWork.UserCourseLocalRoles.FirstOrDefaultAsync(
                    u => u.CourseId == courseId &&
                    u.UserId == requesterUserId);

                var targetUserLocalRole = await unitOfWork.UserCourseLocalRoles.FirstOrDefaultAsync(
                    u => u.CourseId == courseId &&
                    u.UserId == targetUserId
                    );

                if(requesterUserLocalRole == null || targetUserLocalRole == null)
                {
                    throw new NotFoundException("Локальная роль пользовтателя не найдена");
                }

                await unitOfWork.UserCourseLocalRoles.Entry(requesterUserLocalRole)
                   .Reference(u => u.LocalRole)
                   .LoadAsync();

                await unitOfWork.UserCourseLocalRoles.Entry(targetUserLocalRole)
                   .Reference(u => u.LocalRole)
                   .LoadAsync();

                await unitOfWork.UserCourseLocalRoles.Entry(targetUserLocalRole)
                  .Reference(u => u.User)
                  .LoadAsync();

                if (requesterUserLocalRole.LocalRole.GetRolePriority() < targetUserLocalRole.LocalRole.GetRolePriority())
                {
                    throw new AccessLevelException("Приоритет вашей роли низкий для изменения роли");
                }
                var localRole = await unitOfWork.LocalRoles.FirstOrDefaultAsync(l => l.Id == localRoleId);

                if (localRole == null)
                {
                    throw new NotFoundException("Локальная роль не найдена");
                }

                if (requesterUserLocalRole.LocalRole.GetRolePriority() < localRole.GetRolePriority())
                {
                    throw new AccessLevelException("Приоритет присваемой роли высокий");
                }

                var course = await unitOfWork.Courses.FindAsync(courseId);
                
                if(course == null)
                {
                    throw new NotFoundException("Курс не найден");
                }

                var userCourseLocalRole = new UserCourseLocalRole()
                {
                    User = targetUserLocalRole.User,
                    Course = course,
                    LocalRole = localRole
                };

                unitOfWork.UserCourseLocalRoles.Remove(targetUserLocalRole);
                await unitOfWork.CommitAsync();

                await unitOfWork.UserCourseLocalRoles.AddAsync(userCourseLocalRole);
                await unitOfWork.CommitAsync();

                return new Response()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Локальная роль пользователя успешно обновлена"
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
                    Message = "Не удалось обновить локальную роль пользователя",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }
    }
}