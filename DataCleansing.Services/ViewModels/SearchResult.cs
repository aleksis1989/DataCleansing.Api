using System.Collections.Generic;
using DataCleansing.Services.ViewModels.Search;

namespace DataCleansing.Services.ViewModels
{
    public class SearchResult<T> where T : class
    {
        public IList<T> Data;
        public BaseSearchModel Page;
    }
}
