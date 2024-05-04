using LearnLink.Core.Entities;
using LearnLink.Shared.DataTransferObjects;

namespace LearnLink.Application.Mappers
{
    public static class ModuleMapper
    {
        public static Module ToEntity(this ModuleDto moduleDto)
        {
            return new Module()
            {
                Id = moduleDto.Id,
                Title = moduleDto.Title,
                Description = moduleDto.Description,
            };
        }

        public static ModuleDto ToDto(this Module moduleEntity)
        {
            return new ModuleDto
                (
                    Id: moduleEntity.Id,
                    Title: moduleEntity.Title,
                    Description: moduleEntity.Description
                );
        }

        public static ClientModuleDto ToClientModule(this Module moduleEntity, ModuleCompletion? moduleCompletion = null)
        {
            return new ClientModuleDto()
            {
                Id = moduleEntity.Id,
                Title = moduleEntity.Title,
                Description = moduleEntity.Description,
                Completed = moduleCompletion == null ? false : moduleCompletion.Completed,
                CompletionProgress = moduleCompletion?.CompletionProgress
            };
        }

        public static Module Assign(this Module module, ModuleDto moduleDto)
        {
            module.Title = moduleDto.Title;
            module.Description = moduleDto.Description;
            return module;
        }
    }
}
