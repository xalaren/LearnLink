using LearnLink.Core.Interfaces;

namespace LearnLink.Core.Exceptions
{
    public class CustomException : Exception, IExceptionStatusCode
    {
        public virtual int StatusCode { get; }
        public CustomException(string message) : base(message) { }
    }
}
