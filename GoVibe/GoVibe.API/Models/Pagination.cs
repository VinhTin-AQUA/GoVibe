namespace GoVibe.API.Models
{
    public class Pagination<T> where T : class
    {
        public List<T> Items { get; set; } = [];

        private int _pageIndex = 0;
        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = Math.Max(0, value);
        }

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = Math.Clamp(value, 5, 50);
        }

        public int TotalCount { get; set; }
    }
}
