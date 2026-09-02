using System;
using System.Collections.Generic;
using System.Text;

namespace LearnLink.Domain.Entities.Abstractions
{
    public interface IEmptyable<T>
    {
        static abstract bool IsEmpty(T value);
    }
}
