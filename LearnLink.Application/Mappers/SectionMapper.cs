using LearnLink.Core.Entities;
using LearnLink.Shared.DataTransferObjects;

namespace LearnLink.Application.Mappers
{
    public static class SectionMapper
    {
        public static SectionDto ToDto(this Section sectionEntity)
        {
            return new SectionDto
            (
                LessonId: sectionEntity.LessonId,
                ContentId: sectionEntity.ContentId,
                Content: sectionEntity.Content.ToDto(),
                Title: sectionEntity.Title,
                Order: sectionEntity.Order
            );
        }

        public static Section ToEntity(this SectionDto sectionDto)
        {
            return new Section()
            {
                LessonId = sectionDto.LessonId,
                ContentId = sectionDto.ContentId,
                Content = sectionDto.Content.ToEntity(),
                Order = sectionDto.Order,
                Title = sectionDto.Title
            };
        }

        public static Section Assign(this Section sectionEntity, SectionDto sectionDto)
        {
            sectionEntity.Title = sectionDto.Title ?? sectionEntity.Title;
            sectionEntity.Order = sectionDto.Order;
            sectionEntity.Content = sectionDto.Content?.ToEntity() ?? sectionEntity.Content; 
            sectionEntity.ContentId = sectionDto.ContentId;

            return sectionEntity;
        }
    }
}
