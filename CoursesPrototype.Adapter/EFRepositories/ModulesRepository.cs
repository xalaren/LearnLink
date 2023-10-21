using CoursesPrototype.Adapter.EFContexts;
using CoursesPrototype.Application.Repository;
using CoursesPrototype.Core.Entities;
using CoursesPrototype.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CoursesPrototype.Adapter.EFRepositories
{
    public class ModulesRepository : IModulesRepository
    {
        private readonly AppDbContext context;

        public ModulesRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(Module entity)
        {
            await context.Modules.AddAsync(entity);
        }

        public async ValueTask DisposeAsync()
        {
            await context.DisposeAsync();
        }

        public async Task<Module[]> GetAllModulesAsync()
        {
            return await context.Modules.ToArrayAsync();
        }

        public async Task<Module?> GetAsync(int entityId)
        {
            return await context.Modules.FindAsync(entityId);
        }

        public async Task<Module[]> GetModulesFromCourseModules(IEnumerable<CourseModule> courseModules)
        {
            List<Module> modules = new List<Module>();
            foreach (CourseModule courseModule in courseModules)
            {
                var module = await context.Modules.FirstOrDefaultAsync(module => module.Id == courseModule.ModuleId);

                if (module != null) modules.Add(module);
            }
            return modules.ToArray();
        }

        public async Task RemoveAsync(int entityId)
        {
            var entity = await GetAsync(entityId);

            if(entity == null)
            {
                throw new NotFoundException("Модуль не найден");
            }

            context.Modules.Remove(entity);
        }

        public void Update(Module entity)
        {
            context.Modules.Update(entity);
        }
    }
}
