namespace DataCleansing.Services.ViewModels.Search
{
    public class CleansingFirstNameSearchModel : BaseSearchModel
    {
        public string FirstName { get; set; }

        public int SimilarityTypeId { get; set; }

        public int CleansingStreetStatusId { get; set; }

        public int CleansingFirstNameRowFilter { get; set; }
    }
}
