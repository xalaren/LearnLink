namespace LearnLink.Core.Exceptions
{
    public class ValidationException : CustomException
    {
        public ValidationException(string message) : base(message) { }

        public override int StatusCode => 400;
    }
}
