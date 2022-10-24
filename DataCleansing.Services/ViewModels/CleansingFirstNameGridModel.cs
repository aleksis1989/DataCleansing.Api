namespace DataCleansing.Services.ViewModels
{
    public class CleansingFirstNameGridModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public int? SimilarityTypeId { get; set; }

        public string SimilarityTypeName { get; set; }

        public string SimilarityFirstName { get; set; }

        public string CleansingFirstNameStatusName { get; set; }

        public bool CanProcess { get; set; }

        public bool CanReject { get; set; }
    }
}
