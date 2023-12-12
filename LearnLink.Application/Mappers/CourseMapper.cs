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
                    IsPublic: courseEntity.IsPublic,
                    CreatorsCount: courseEntity.CreatorsCount,
                    SubscribersCount: courseEntity.SubscribersCount
                );
        }

        public static Course Assign(this Course course, CourseDto courseDto)
        {
            course.Title = courseDto.Title;
            course.Description = courseDto.Description;
            course.IsPublic = courseDto.IsPublic;

            return course;
        }
    }
}
