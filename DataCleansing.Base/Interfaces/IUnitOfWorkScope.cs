using System;

namespace DataCleansing.Base.Interfaces
{
    interface IUnitOfWorkScope : IDisposable
    {
        Guid ScopeId { get; }

        void Commit();

        IUnitOfWork UnitOfWork { get; }
    }
}
