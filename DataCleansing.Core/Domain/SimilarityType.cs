using DataCleansing.Base.Entity;

namespace DataCleansing.Core.Domain
{
    public class SimilarityType : BaseEntity<int>
    {
        public virtual string SimilarityTypeName { get; set; }
    }
}
