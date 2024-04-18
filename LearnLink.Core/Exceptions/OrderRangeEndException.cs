namespace LearnLink.Core.Exceptions
{
    public class OrderRangeEndException : BadRequestException
    {
        public OrderRangeEndException(string message) : base(message) { }

    }
}
