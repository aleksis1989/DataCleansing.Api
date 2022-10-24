using DataCleansing.Base.Entity;

namespace DataCleansing.Core.Domain
{
    public class CleansingFirstNameStatus : BaseEntity<int>
    {
        public virtual string CleansingFirstNameStatusName { get; set; }
    }
}
