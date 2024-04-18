namespace LearnLink.Core.Exceptions
{
    public class NotFoundException : CustomException
    {
        public NotFoundException(string message) : base(message) { }

        public override int StatusCode => 404;
    }
}
