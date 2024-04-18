namespace LearnLink.Core.Exceptions
{
    public class BadRequestException : CustomException
    {
        public BadRequestException(string message) : base(message) { }

        public override int StatusCode => 400;
    }
}
