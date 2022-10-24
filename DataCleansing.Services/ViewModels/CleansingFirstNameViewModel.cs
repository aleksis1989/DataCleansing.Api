namespace DataCleansing.Services.ViewModels
{
    public class CleansingFirstNameViewModel
    {
        public int Id { get; set; }

        public int PersonId { get; set; }

        public string FirstName { get; set; }

        public int? Levenshtein { get; set; }

        public string LevenshteinFirstName { get; set; }

        public int? Jaccard { get; set; }

        public string JaccardFirstName { get; set; }

        public int? JaroWinkler { get; set; }

        public string JaroWinklerFirstName { get; set; }

        public int? LongestCommonSubsequence { get; set; }

        public string LongestCommonSubsequenceFirstName { get; set; }

        public int? SimilarityTypeId { get; set; }

        public string SimilarityTypeName { get; set; }

        public int? SimilarityFirstNameId { get; set; }

        public string SimilarityFirstName { get; set; }

        public string SimilarityFirstNameCleansingResult { get; set; }

        public int? CleansingFirstNameStatusId { get; set; }

        public int? ManualFirstNameId { get; set; }

        public string ManualFirstName { get; set; }
    }
}
