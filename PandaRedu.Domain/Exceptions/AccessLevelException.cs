namespace PandaRedu.Domain.Exceptions
{
    /// <summary>
    /// Domain exception type of access level restriction
    /// </summary>
    public class AccessLevelException : DomainException
    {
        public AccessLevelException(string message) : base(message) { }
    }
}
