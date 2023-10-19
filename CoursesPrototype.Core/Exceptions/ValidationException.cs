namespace CoursesPrototype.Core.Exceptions
{
    public class ValidationException : CustomException
    {
        public ValidationException(string message) : base(message) { }
    }
}
