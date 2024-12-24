namespace StarterApi.Application.Common.Models
{
    public class QueryParameters
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;
        
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string SortBy { get; set; }
        public bool IsDescending { get; set; }
        public string SearchTerm { get; set; }
        public List<FilterCriteria> Filters { get; set; } = new();
    }

    public class FilterCriteria
    {
        public string PropertyName { get; set; }
        public string Operation { get; set; }  // eq, neq, gt, lt, contains
        public string Value { get; set; }
    }

    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}
