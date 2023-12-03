using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Controllers
{
    /// <summary>
    /// Контроллер для управления модулями курса
    /// </summary>
    [ApiController]
    [Route("api/Modules")]
    public class ModuleController
    {
        private readonly ModuleInteractor moduleInteractor;

        public ModuleController(ModuleInteractor moduleInteractor)
        {
            this.moduleInteractor = moduleInteractor;
        }

        /// <summary>
        /// Получение модуля по идентификатору
        /// </summary>
        /// <param name="moduleId">Идентификатор модуля</param>
        [HttpGet("get")]
        public async Task<Response<ModuleDto?>> GetModule(int moduleId)
        {
            return await moduleInteractor.GetModuleAsync(moduleId);
        }

        /// <summary>
        /// Получение всех модулей курса
        /// </summary>
        [HttpGet("get-all")]
        public async Task<Response<ModuleDto[]>> GetAllModules()
        {
            return await moduleInteractor.GetAllModulesAsync();
        }

        /// <summary>
        /// Получение модулей, добавленных в курс
        /// </summary>
        /// <param name="courseId">Идентификатор курса</param>
        [Authorize]
        [HttpGet("get-course-modules")]
        public async Task<Response<ModuleDto[]>> GetCourseModulesAsync(int courseId)
        {
            return await moduleInteractor.GetCourseModulesAsync(courseId);
        }

        /// <summary>
        /// Добавление модуля в курс
        /// </summary>
        /// <param name="courseId">Идентификатор курса</param>
        /// <param name="moduleDto">Объект данных модуля</param>
        [Authorize]
        [HttpPost("create")]
        public async Task<Response> CreateModuleAsync(int courseId, ModuleDto moduleDto)
        {
            return await moduleInteractor.CreateModuleAsync(courseId, moduleDto);
        }


        /// <summary>
        /// Изменение модуля
        /// </summary>
        /// <param name="moduleDto">Объект данных измененного модуля</param>
        [Authorize]
        [HttpPost("update")]
        public async Task<Response> UpdateModuleAsync(ModuleDto moduleDto)
        {
            return await moduleInteractor.UpdateModuleAsync(moduleDto);
        }

        /// <summary>
        /// Удаление модуля
        /// </summary>
        /// <param name="moduleId">Идентификатор пользователя</param>
        [HttpDelete("remove")]
        public async Task<Response> RemoveModuleAsync(int moduleId)
        {
            return await moduleInteractor.RemoveModuleAsync(moduleId);
        }

    }
}
