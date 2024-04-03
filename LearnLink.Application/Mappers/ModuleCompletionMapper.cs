using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnLink.Core.Entities;
using LearnLink.Shared.DataTransferObjects;

namespace LearnLink.Application.Mappers
{
    public static class ModuleCompletionMapper
    {
        public static ModuleCompletionDto ToDto(this ModuleCompletion moduleCompletionEntity)
        {
            return new ModuleCompletionDto
            (
                UserId: moduleCompletionEntity.UserId,
                CourseId: moduleCompletionEntity.CourseId,
                ModuleDto: moduleCompletionEntity.Module.ToDto(),
                Completed: moduleCompletionEntity.Completed,
                CompletionProgress: moduleCompletionEntity.CompletionProgress
            );
        }
    }
}
