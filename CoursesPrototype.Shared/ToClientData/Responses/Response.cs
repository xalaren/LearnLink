namespace CoursesPrototype.Shared.ToClientData.Responses
{
    public class Response
    {
        public bool Success { get; init; }
        public string? Message { get; init; }
    }

    public class Response<T> : Response
    {
        public T? Value { get; set; }
    }
}
