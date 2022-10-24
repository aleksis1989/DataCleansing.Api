using DataCleansing.Base.Interfaces;
using DataCleansing.Core.Domain;

namespace DataCleansing.Core.Repositories
{
    /// <summary>
    /// Ги содржи сите методи за повлекување/филтрирање на типови на кривични дела.
    /// </summary>
    /// <seealso cref="IRepository{TEntity}.Core.Domain.CrimeType}" />
    public interface ICleansingFirstNameRepository : IRepository<CleansingFirstName>
    {
        T GetCleansingFirstNameReport<T>();
    }
}
