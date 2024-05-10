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
                await CreateLocalRoleAsyncNoResponse(localRoleDto);

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
                var localRole = await GetLocalRoleBySignAsyncNoResponse(sign);

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

        public async Task CreateLocalRoleAsyncNoResponse(LocalRoleDto localRoleDto)
        {
            var existingRole = await unitOfWork.LocalRoles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(localRole => localRole.Sign == localRoleDto.Sign);

            if (existingRole != null)
            {
                throw new ValidationException("Данная локальная роль уже добавлена в систему");
            }

            var localRoleEntity = localRoleDto.ToEntity();

            await unitOfWork.LocalRoles.AddAsync(localRoleEntity);
            await unitOfWork.CommitAsync();
        }

        public async Task<LocalRole?> GetLocalRoleBySignAsyncNoResponse(string sign)
        {
            return await unitOfWork.LocalRoles
                    .FirstOrDefaultAsync(localRole => localRole.Sign == sign);
        }
    }
}