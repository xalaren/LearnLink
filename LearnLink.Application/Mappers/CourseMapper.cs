using LearnLink.Core.Entities;
using LearnLink.Shared.DataTransferObjects;

namespace LearnLink.Application.Mappers
{
    public static class CourseMapper
    {
        public static Course ToEntity(this CourseDto courseDto)
        {
            DateTime dateTime;

            if (string.IsNullOrWhiteSpace(courseDto.CreationDate))
            {
                dateTime = DateTime.Now.ToUniversalTime();
            }
            else
            {
                dateTime = DateTime.Parse(courseDto.CreationDate);
            }

            return new Course()
            {
                Id = courseDto.Id,
                Title = courseDto.Title,
                Description = courseDto.Description,
                CreationDate = dateTime,
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
                    CreationDate: courseEntity.CreationDate.ToShortDateString()
                );
        }

        public static ClientCourseDto ToClientCourseDto(this Course courseEntity, LocalRole? localRole = null, CourseCompletion? courseCompletion = null, Subscription? subscription = null)
        {
            return new ClientCourseDto()
            {
                Id = courseEntity.Id,
                Title = courseEntity.Title,
                Description = courseEntity.Description,
                IsPublic = courseEntity.IsPublic,
                IsUnavailable = courseEntity.IsUnavailable,
                SubscribersCount = courseEntity.SubscribersCount,
                CreationDate = courseEntity.CreationDate.ToShortDateString(),
                SubscribeDate = subscription?.StartDate.ToShortDateString(),
                Completed = courseCompletion?.Completed,
                CompletionProgress = courseCompletion?.CompletionProgress,
                LocalRole = localRole?.ToDto()
            };
        }

        public static Course Assign(this Course course, CourseDto courseDto)
        {
            DateTime dateTime;

            if (string.IsNullOrWhiteSpace(courseDto.CreationDate))
            {
                dateTime = DateTime.Now.ToUniversalTime();
            }
            else
            {
                dateTime = DateTime.Parse(courseDto.CreationDate);
            }

            course.Title = courseDto.Title;
            course.Description = courseDto.Description;
            course.IsPublic = courseDto.IsPublic;
            course.IsUnavailable = courseDto.IsUnavailable;
            course.CreationDate = dateTime;

            return course;
        }
    }
}
