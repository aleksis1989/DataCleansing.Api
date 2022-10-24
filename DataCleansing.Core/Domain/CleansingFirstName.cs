using DataCleansing.Base.Entity;

namespace DataCleansing.Core.Domain
{
    public class CleansingFirstName : BaseEntity<int>
    {
        public virtual int PersonId { get; set; }

        public virtual string FirstName { get; set; }

        public virtual decimal? Levenshtein { get; set; }

        public virtual string LevenshteinFirstName { get; set; }

        public virtual decimal? Jaccard { get; set; }

        public virtual string JaccardFirstName { get; set; }

        public virtual decimal? JaroWinkler { get; set; }

        public virtual string JaroWinklerFirstName { get; set; }

        public virtual decimal? LongestCommonSubsequence { get; set; }

        public virtual string LongestCommonSubsequenceFirstName { get; set; }

        public virtual SimilarityType SimilarityType { get; set; }

        public virtual int? SimilarityFirstNameId { get; set; }

        public virtual string SimilarityFirstName { get; set; }

        public virtual int? ManualFirstNameId { get; set; }

        public virtual string ManualFirstName { get; set; }

        public virtual CleansingFirstNameStatus CleansingFirstNameStatus { get; set; }
    }
}
