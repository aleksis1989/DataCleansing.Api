namespace DataCleansing.Services.ViewModels.Search
{
    public class BaseSearchModel
    {
        public int PageNumber { get; set; }

        public int Size { get; set; }

        public int TotalElements { get; set; }

        public int TotalPages { get; set; }

        public string SortColumn { get; set; }

        public string SortOrder { get; set; }

        public string SearchText { get; set; }
    }
}
