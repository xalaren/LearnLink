namespace LearnLink.Shared.Responses
{
    public class DataPageHeader
    {
        private int pageSize = 1;
        public int PageNumber { get; init; } = 1;
        public int PageSize
        {
            get => pageSize;
            init => pageSize = value == 0 ? 1 : value;
        }

        public DataPageHeader() { }

        public DataPageHeader(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
    public class DataPage<T> : DataPageHeader
    {
        public int ItemsCount { get; init; }
        public int PageCount => (int)Math.Ceiling((decimal)ItemsCount / PageSize);

        public T? Values { get; set; }
    }
}
