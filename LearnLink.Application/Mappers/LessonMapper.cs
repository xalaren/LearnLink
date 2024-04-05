using LearnLink.Core.Entities;
using LearnLink.Shared.DataTransferObjects;

namespace LearnLink.Application.Mappers
{
    public static class LessonMapper
    {
        public static LessonDto ToDto(this Lesson lessonEntity)
        {
            return new LessonDto(
                Id: lessonEntity.Id,
                Title: lessonEntity.Title,
                Description: lessonEntity.Description
            );
        }

        public static Lesson ToEntity(this LessonDto lessonDto)
        {
            return new Lesson()
            {
                Id = lessonDto.Id,
                Title = lessonDto.Title,
                Description = lessonDto.Description
            };
        }

        public static Lesson Assign(this Lesson lesson, LessonDto lessonDto)
        {
            lesson.Title = !string.IsNullOrWhiteSpace(lessonDto.Title) ? lessonDto.Title : lesson.Title;
            lesson.Description = lessonDto.Description;
            return lesson;
        }
    }
}
