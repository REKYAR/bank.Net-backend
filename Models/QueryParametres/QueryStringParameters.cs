using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;

namespace Bank.NET___backend.Models.QueryParametres
{
    public abstract class QueryStringParameters
    {
        const int maxPageSize = 100;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 25;
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value > maxPageSize ? maxPageSize : value;
            }
        }

        public string GetPagingMetadata(int TotalCount)
        {
            var metadata = new
            {
                PageNumber,
                PageSize,
                TotalCount
            };

            return JsonConvert.SerializeObject(metadata);
        }
    }
}
