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
    }
}
