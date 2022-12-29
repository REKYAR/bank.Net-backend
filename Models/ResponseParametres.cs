namespace Bank.NET___backend.Models
{
    public class ResponseParametres
    {
        const int maxPageSize = 100;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 25;
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize: value;
            }
        }
    }
}
