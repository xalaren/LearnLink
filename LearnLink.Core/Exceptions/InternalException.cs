namespace LearnLink.Core.Exceptions
{
    public class InternalException : CustomException
    {
        public InternalException(string message) : base(message) { }

        public override int StatusCode => 500;
    }
}
