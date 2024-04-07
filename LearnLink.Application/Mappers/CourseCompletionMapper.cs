using LearnLink.Core.Entities;
using LearnLink.Shared.DataTransferObjects;

namespace LearnLink.Application.Mappers
{
    public static class CourseCompletionMapper
    {
        public static CourseCompletionDto ToDto(this CourseCompletion courseCompletionEntity)
        {
            return new CourseCompletionDto
            (
                UserId: courseCompletionEntity.UserId,
                Course: courseCompletionEntity.Course.ToDto(),
                Completed: courseCompletionEntity.Completed,
                CompletionProgress: courseCompletionEntity.CompletionProgress
            );
        }
    }
}
