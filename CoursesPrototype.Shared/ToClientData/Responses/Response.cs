namespace CoursesPrototype.Shared.ToClientData.Responses
{
    public class Response
    {
        public bool Success { get; init; }
        public string? Message { get; init; }
        public string[]? InnerErrorMessages { get; set; }
    }

    public class Response<T> : Response
    {
        public T? Value { get; set; }
    }
}
