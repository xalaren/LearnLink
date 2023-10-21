using CoursesPrototype.Core.Entities;
using CoursesPrototype.Shared.DataTransferObjects;

namespace CoursesPrototype.Application.Mappers
{
    public static class ModuleMapper
    {
        public static Module ToEntity(this ModuleDto moduleDto)
        {
            return new Module()
            {
                Id = moduleDto.Id,
                Title = moduleDto.Title,
                Content = moduleDto.Content,
                Description = moduleDto.Description,
            };
        }

        public static ModuleDto ToDto(this Module moduleEntity)
        {
            return new ModuleDto
                (
                    Id: moduleEntity.Id,
                    Title: moduleEntity.Title,
                    Description: moduleEntity.Description,
                    Content: moduleEntity.Content
                );
        }

        public static Module Assign(this Module module, ModuleDto moduleDto)
        {
            module.Title = moduleDto.Title;
            module.Description = moduleDto.Description;
            module.Content = moduleDto.Content;
            return module;
        }
    }
}
