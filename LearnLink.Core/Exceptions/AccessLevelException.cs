namespace LearnLink.Core.Exceptions
{
    public class AccessLevelException : CustomException
    {
        public AccessLevelException(string message) : base(message) { }

        public override int StatusCode => 403;
    }
}
