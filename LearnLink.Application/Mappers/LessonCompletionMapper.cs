﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnLink.Core.Entities;
using LearnLink.Shared.DataTransferObjects;

namespace LearnLink.Application.Mappers
{
    public static class LessonCompletionMapper
    {
        public static LessonCompletionDto ToDto(this LessonCompletion lessonCompletionEntity)
        {
            return new LessonCompletionDto
            (
                UserId: lessonCompletionEntity.UserId,
                LessonDto: lessonCompletionEntity.Lesson.ToDto(),
                Completed: lessonCompletionEntity.Completed,
                CompletionProgress: lessonCompletionEntity.CompletionProgress
            );
        }
    }
}
