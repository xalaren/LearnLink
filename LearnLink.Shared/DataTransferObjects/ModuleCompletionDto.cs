using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnLink.Shared.DataTransferObjects
{
    public record ModuleCompletionDto
        (
            int UserId,
            ModuleDto ModuleDto,
            bool Completed,
            int CompletionProgress
        );
}
