using System.Net;
using System.Xml.Linq;
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
    public class RoleInteractor
    {
        private readonly IUnitOfWork unitOfWork;

        public RoleInteractor(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Response> CreateRoleAsync(RoleDto roleDto)
        {
            try
            {
                var existingRole = await unitOfWork.Roles.AsNoTracking().FirstOrDefaultAsync(role => role.Sign == roleDto.Sign);

                if(existingRole != null)
                {
                    throw new ValidationException("Данная роль уже добавлена в систему");
                }

                var roleEntity = roleDto.ToEntity();

                await unitOfWork.Roles.AddAsync(roleEntity);
                await unitOfWork.CommitAsync();

                return new Response()
                {
                    Success = true,
                    Message = "Роль успешно добавлена в систему",
                };
            }
            catch(CustomException exception)
            {
                return new Response()
                {
                    Success = false,
                    Message = exception.Message,
                };
            }
            catch(Exception exception)
            {
                return new Response()
                {
                    Success = false,
                    Message = "Не удалось добавить роль. Внутренняя ошибка.",
                    InnerErrorMessages = new string[] { exception.Message }
                };
            }
        }


        public async Task<Response> AllocateRoleToUserAsync(int roleId, int userId)
        {
            try
            {
                var role = await unitOfWork.Roles.FindAsync(roleId);

                if (role == null)
                {
                    throw new NotFoundException("Роль не найдена");
                }

                var user = await unitOfWork.Users.FindAsync(userId);

                if(user == null)
                {
                    throw new NotFoundException("Пользователь не найден");
                }

                user.Role = role;

                unitOfWork.Users.Update(user);
                await unitOfWork.CommitAsync();

                return new()
                {
                    Success = true,
                    Message = "Роль присвоена успешно",
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
                    Message = "Не удалось присвоить роль. Внутренняя ошибка.",
                    InnerErrorMessages = new string[] { exception.Message }
                };
            }
        }

        public async Task<Response<RoleDto[]>> GetAllRolesAsync()
        {
            try
            {
                var roles = await unitOfWork.Roles.AsNoTracking().Select(role => role.ToDto()).ToArrayAsync();

                return new()
                {
                    Success = true,
                    Message = "Роли успешно получены",
                    Value = roles,
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
                    Message = "Не удалось получить роли. Внутренняя ошибка.",
                    InnerErrorMessages = new string[] { exception.Message }
                };
            }
        }

        public async Task<Response<RoleDto?>> GetRoleByIdAsync(int roleId)
        {
            try
            {
                var role = await unitOfWork.Roles.AsNoTracking().FirstOrDefaultAsync(role => role.Id == roleId);

                return new()
                {
                    Success = true,
                    Message = "Роль успешно получена",
                    Value = role?.ToDto(),
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
                    Message = "Не удалось получить роль. Внутренняя ошибка.",
                    InnerErrorMessages = new string[] { exception.Message }
                };
            }
        }

        public async Task<Response<RoleDto?>> GetRoleByNameAsync(string name)
        {
            try
            {
                var role = await unitOfWork.Roles.AsNoTracking().FirstOrDefaultAsync(role => role.Name == name);

                return new()
                {
                    Success = true,
                    Message = "Роль успешно получена",
                    Value = role?.ToDto(),
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
                    Message = "Не удалось получить роль. Внутренняя ошибка.",
                    InnerErrorMessages = new string[] { exception.Message }
                };
            }
        }

        public async Task<Response<RoleDto?>> GetRoleBySignAsync(string sign)
        {
            try
            {
                var role = await unitOfWork.Roles.AsNoTracking().FirstOrDefaultAsync(role => role.Sign == sign);

                return new()
                {
                    Success = true,
                    Message = "Роль успешно получена",
                    Value = role?.ToDto(),
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
                    Message = "Не удалось получить роль. Внутренняя ошибка.",
                    InnerErrorMessages = new string[] { exception.Message }
                };
            }
        }

        public async Task<Response> RemoveRoleAsync(int roleId)
        {
            try
            {
                var role = await unitOfWork.Roles.FindAsync(roleId);

                if(role == null)
                {
                    throw new NotFoundException("Роль не найдена");
                }

                unitOfWork.Roles.Remove(role);
                await unitOfWork.CommitAsync();

                return new()
                {
                    Success = true,
                    Message = "Роль успешно удалена",
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
                    Message = "Не удалось удалить роль. Внутренняя ошибка.",
                    InnerErrorMessages = new string[] { exception.Message }
                };
            }
        }

        public async Task<Response> UpdateRoleAsync(RoleDto roleDto)
        {
            try
            {
                var role = await unitOfWork.Roles.FindAsync(roleDto.Id);

                if (role == null)
                {
                    throw new NotFoundException("Роль не найдена");
                }

                role = role.Assign(roleDto);

                unitOfWork.Roles.Update(role);
                await unitOfWork.CommitAsync();

                return new()
                {
                    Success = true,
                    Message = "Роль успешно изменена",
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
                    Message = "Не удалось изменить роль. Внутренняя ошибка.",
                    InnerErrorMessages = new string[] { exception.Message }
                };
            }
        }
    }
}
