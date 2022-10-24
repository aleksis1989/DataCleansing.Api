using System;

namespace DataCleansing.Base.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Flush();

        void Discard(object entyty);
    }
}
