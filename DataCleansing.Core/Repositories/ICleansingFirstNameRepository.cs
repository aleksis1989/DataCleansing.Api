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
        /// <summary>
        /// Враќа податоци за податоците кои се прочистуваат
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetCleansingFirstNameReport<T>();

        /// <summary>
        /// Го прочистува редот со новото име
        /// </summary>
        /// <param name="cleansingFirstNameId"></param>
        /// <param name="knowlegeFirstNameId"></param>
        /// <param name="cleansingFirstNameStatusId"></param>
        void MergeFirstName(int cleansingFirstNameId, int knowlegeFirstNameId, int cleansingFirstNameStatusId);

        /// <summary>
        /// Редот кој е прочистен го враќа во првобитна состојба
        /// </summary>
        /// <param name="cleansingFirstNameId"></param>
        void UndoMergeFirstName(int cleansingFirstNameId);

        /// <summary>
        /// Го добива прочистувањето
        /// </summary>
        /// <param name="cleansingFirstNameId"></param>
        void RejectFirstName(int cleansingFirstNameId);
    }
}
