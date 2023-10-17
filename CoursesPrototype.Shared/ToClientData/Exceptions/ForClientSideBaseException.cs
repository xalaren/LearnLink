namespace CoursesPrototype.Shared.Exceptions
{
    /// <summary>
    /// Exception that could be sent to the client
    /// </summary>
    public class ForClientSideBaseException : Exception
    {
        public ForClientSideBaseException(string message) : base(message) { }
    }
}
