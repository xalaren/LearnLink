using CoursesPrototype.Core.Entities;
using CoursesPrototype.Shared.DataTransferObjects;

namespace CoursesPrototype.Application.Mappers
{
    public static class CourseMapper
    {
        public static Course ToEntity(this CourseDto courseDto)
        {
            return new Course()
            {
                Id = courseDto.Id,
                Title = courseDto.Title,
                Description = courseDto.Description,
                IsPublic = courseDto.IsPublic,
            };
        }

        public static CourseDto ToDto(this Course courseEntity)
        {
            return new CourseDto
                (
                    Id: courseEntity.Id,
                    Title: courseEntity.Title,
                    Description: courseEntity.Description,
                    IsPublic: courseEntity.IsPublic
                );
        }
    }
}
