using DataCleansing.Base.Entity;

namespace DataCleansing.Core.Domain
{
    public class KnowlegeFirstName : BaseEntity<int>
    {
        public virtual string FirstName { get; set; }
    }
}
