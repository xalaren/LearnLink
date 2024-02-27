using LearnLink.Core.Entities;
using LearnLink.Shared.DataTransferObjects;

namespace LearnLink.Application.Mappers
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
                CreationDate = courseDto.CreationDate ?? DateTime.Now,
                IsPublic = courseDto.IsPublic,
                IsUnavailable = courseDto.IsUnavailable,
            };
        }

        public static CourseDto ToDto(this Course courseEntity)
        {
            return new CourseDto
                (
                    Id: courseEntity.Id,
                    Title: courseEntity.Title,
                    Description: courseEntity.Description,
                    IsPublic: courseEntity.IsPublic,
                    IsUnavailable: courseEntity.IsUnavailable,
                    SubscribersCount: courseEntity.SubscribersCount,
                    CreationDate: courseEntity.CreationDate
                );
        }

        public static Course Assign(this Course course, CourseDto courseDto)
        {
            course.Title = courseDto.Title;
            course.Description = courseDto.Description;
            course.IsPublic = courseDto.IsPublic;
            course.IsUnavailable = courseDto.IsUnavailable;

            return course;
        }
    }
}
