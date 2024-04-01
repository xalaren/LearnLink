﻿namespace LearnLink.Shared.DataTransferObjects
{
    public record CourseDto
        (
          int Id,
          string Title,
          string? Description = null,
          DateTime? CreationDate = null,
          int SubscribersCount = 0,
          bool IsPublic = false,
          bool IsUnavailable = false,
          int? CompletionProgress = null,
          bool? Competed = null
        );
}
