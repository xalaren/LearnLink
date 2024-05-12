using LearnLink.Application.Mappers;
using LearnLink.Application.Transaction;
using LearnLink.Core.Constants;
using LearnLink.Core.Entities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Interactors
{
    public class CourseLocalRoleInteractor(IUnitOfWork unitOfWork, LocalRoleInteractor localRoleInteractor)
    {
        public async Task<Response<LocalRoleDto[]>> GetLocalRolesAtCourseAsync(int courseId)
        {
            try
            {
                var courseLocalRoles = await unitOfWork.CourseLocalRoles
                    .Where(courseLocalRole => courseLocalRole.CourseId == courseId)
                    .Include(courseLocalRole => courseLocalRole.LocalRole)
                    .ToArrayAsync();
                    
                    var courseLocalRolesDto = courseLocalRoles
                    .OrderByDescending(courseLocalRole => courseLocalRole.LocalRole.GetRolePriority())
                    .Select(courseLocalRole => courseLocalRole.LocalRole.ToDto())
                    .ToArray();

                return new Response<LocalRoleDto[]>()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Локальные роли курса успешно получены",
                    Value = courseLocalRolesDto
                };
            }
            catch (CustomException exception)
            {
                return new Response<LocalRoleDto[]>()
                {
                    Success = false,
                    StatusCode = exception.StatusCode,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new Response<LocalRoleDto[]>()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Не удалось получить локальные роли курса",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }
        
        public async Task<Response<LocalRoleDto>> GetByCourseAndLocalIdAsync(int courseId, int localRoleId)
        {
            try
            {
                var courseLocalRole = await unitOfWork.CourseLocalRoles.FirstOrDefaultAsync(courseLocalRole =>
                    courseLocalRole.CourseId == courseId &&
                    courseLocalRole.LocalRoleId == localRoleId
                );

                if(courseLocalRole == null)
                {
                    throw new NotFoundException("Не удалось найти локальную роль внутри курса");
                }

                await unitOfWork.CourseLocalRoles.Entry(courseLocalRole)
                    .Reference(courseLocalRole => courseLocalRole.LocalRole)
                    .LoadAsync();

                return new Response<LocalRoleDto>()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Локальная роль успешно получена",
                    Value = courseLocalRole.LocalRole.ToDto()
                };
            }
            catch (CustomException exception)
            {
                return new Response<LocalRoleDto>()
                {
                    Success = false,
                    StatusCode = exception.StatusCode,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new Response<LocalRoleDto>()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Не удалось получить локальную роль",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }
        
        public async Task<Response> RequestCreateAsync(int requesterUserId, int courseId, LocalRoleDto localRoleDto)
        {
            try
            {
                var requesterCourseLocalRole = await unitOfWork.UserCourseLocalRoles
                    .Include(userCourseLocalRole => userCourseLocalRole.LocalRole)
                    .FirstOrDefaultAsync(userCourseLocalRole =>
                        userCourseLocalRole.UserId == requesterUserId &&
                        userCourseLocalRole.CourseId == courseId);

                if (requesterCourseLocalRole == null)
                {
                    throw new NotFoundException("Ваша локальная роль не найдена");
                }

                if (!requesterCourseLocalRole.LocalRole.EditRolesAccess)
                {
                    throw new AccessLevelException("Приоритет вашей роли низкий");
                }
                

                var course = await unitOfWork.Courses.FindAsync(courseId);

                if (course == null)
                {
                    throw new NotFoundException("Курс не найден");
                }

                try
                {
                    await localRoleInteractor.CreateLocalRoleAsyncNoResponse(localRoleDto);
                }
                catch (ValidationException)
                {

                }

                var localRole = await localRoleInteractor.GetLocalRoleBySignAsyncNoResponse(localRoleDto.Sign);

                if (localRole == null)
                {
                    throw new NotFoundException("Не удалось получить созданную роль");
                }

                var courseLocalRole = new CourseLocalRole()
                {
                    Course = course,
                    LocalRole = localRole
                };

                await unitOfWork.CourseLocalRoles.AddAsync(courseLocalRole);
                await unitOfWork.CommitAsync();

                return new Response()
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Локальная роль успешно создана внутри курса"
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
                    Message = "Не удалось создать локальную роль в курсе",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }
        
        public async Task<Response> RequestUpdateLocalRoleAsync(int requesterUserId, int courseId, LocalRoleDto localRoleDto)
        {
            try
            {
                var requesterCourseLocalRole = await unitOfWork.UserCourseLocalRoles
                    .Include(userCourseLocalRole => userCourseLocalRole.LocalRole)
                    .FirstOrDefaultAsync(userCourseLocalRole =>
                        userCourseLocalRole.UserId == requesterUserId &&
                        userCourseLocalRole.CourseId == courseId);

                if(requesterCourseLocalRole == null)
                {
                    throw new NotFoundException("Ваша локальная роль не найдена");
                }

                if(!requesterCourseLocalRole.LocalRole.EditRolesAccess)
                {
                    throw new AccessLevelException("Приоритет вашей роли низкий");
                }

                return await localRoleInteractor.UpdateLocalRoleAsync(localRoleDto);
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
                    Message = "Не удалось изменить локальную роль",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task<Response> RequestRemoveAsync(int requesterUserId, int courseId, int localRoleId)
        {
            try
            {
                var requesterUserLocalRole = await unitOfWork.UserCourseLocalRoles
                    .Include(userCourseLocalRole => userCourseLocalRole.LocalRole)
                    .FirstOrDefaultAsync(userCourseLocalRole =>
                        userCourseLocalRole.UserId == requesterUserId &&
                        userCourseLocalRole.CourseId == courseId);

                if(requesterUserLocalRole == null)
                {
                    throw new NotFoundException("Ваша локальная роль не найдена");
                }

                if(!requesterUserLocalRole.LocalRole.IsAdmin)
                {
                    throw new AccessLevelException("Приоритет вашей роли низкий");
                }

                var localRole = await unitOfWork.LocalRoles.FindAsync(localRoleId);

                if(localRole == null)
                {
                    throw new NotFoundException("Локальная роль не найдена");
                }

                if(string.Equals(localRole.Sign, RoleSignConstants.MEMBER) || string.Equals(localRole.Sign, RoleSignConstants.MODERATOR))
                {
                    throw new AccessLevelException("Невозможно удалить стандартные роли");
                }

                var courseLocalRole = await unitOfWork.CourseLocalRoles.FirstOrDefaultAsync(courseLocalRole =>
                    courseLocalRole.CourseId == courseId &&
                    courseLocalRole.LocalRoleId == localRoleId);

                if(courseLocalRole == null)
                {
                    throw new NotFoundException("Локальная роль не найдена");
                }

                unitOfWork.CourseLocalRoles.Remove(courseLocalRole);
                unitOfWork.LocalRoles.Remove(localRole);

                await unitOfWork.CommitAsync();

                return new Response()
                {
                    Success = true,
                    StatusCode = 200,
                    Message="Локальная роль успешно удалена",
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
                    Message = "Не удалось удалить локальную роль внутри курса",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }

        public async Task CreateDefaultLocalRoles(int courseId, params string[] localRoleSigns)
        {
            Course? course = await unitOfWork.Courses.FindAsync(courseId);

            if (course == null)
            {
                throw new NotFoundException("Курс не найден");
            }

            List<LocalRole> foundLocalRoles = new List<LocalRole>();
            foreach(var localRoleSign in localRoleSigns)
            {
                var localRole = await unitOfWork.LocalRoles.FirstOrDefaultAsync(localRole => localRole.Sign == localRoleSign);

                if(localRole == null)
                {
                    throw new NotFoundException($"Локальная роль с сигнатурой '{localRoleSign}' не найдена");
                }

                foundLocalRoles.Add(localRole);
            }

            var defaultCourseLocalRoles = foundLocalRoles.Select(localRole =>
                new CourseLocalRole()
                {
                    Course = course,
                    LocalRole = localRole
                }
            );

            unitOfWork.CourseLocalRoles.AddRange(defaultCourseLocalRoles);
            await unitOfWork.CommitAsync();
        }
    }
}
