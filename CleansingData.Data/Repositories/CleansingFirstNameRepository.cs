using System.Linq;
using DataCleansing.Base.Implementations;
using DataCleansing.Core.Domain;
using DataCleansing.Core.Repositories;
using NHibernate.Transform;

namespace CleansingData.Data.Repositories
{
    public class CleansingFirstNameRepository : NhRepository<CleansingFirstName>, ICleansingFirstNameRepository
    {
        public T GetCleansingFirstNameReport<T>()
        {
            var result = Session
                .GetNamedQuery("GetCleansingFirstNameReport")
                .SetResultTransformer(Transformers.AliasToBean<T>())
                .List<T>()
                .FirstOrDefault();

            return result;
        }

        public void MergeFirstName(int cleansingFirstNameId, int knowlegeFirstNameId, int cleansingFirstNameStatusId)
        {
            Session.GetNamedQuery("MergeFirstName")
                .SetParameter("cleansingFirstNameId", cleansingFirstNameId)
                .SetParameter("knowlegeFirstNameId", knowlegeFirstNameId)
                .SetParameter("cleansingFirstNameStatusId", cleansingFirstNameStatusId)
                .UniqueResult(); 
        }

        public void UndoMergeFirstName(int cleansingFirstNameId)
        {
            Session.GetNamedQuery("UndoMergeFirstName")
                .SetParameter("cleansingFirstNameId", cleansingFirstNameId)
                .UniqueResult();
        }

        public void RejectFirstName(int cleansingFirstNameId)
        {
            Session.GetNamedQuery("RejectFirstName")
                .SetParameter("cleansingFirstNameId", cleansingFirstNameId)
                .UniqueResult();
        }
    }
}
