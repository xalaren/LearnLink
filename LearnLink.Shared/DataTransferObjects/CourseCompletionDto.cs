﻿namespace LearnLink.Shared.DataTransferObjects
{
    public record CourseCompletionDto
        (
            int UserId,
            CourseDto Course,
            bool Completed,
            int CompletionProgress
        );
}
