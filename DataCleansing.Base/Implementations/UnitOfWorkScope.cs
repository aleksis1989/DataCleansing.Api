using System;
using DataCleansing.Base.Helpers;
using DataCleansing.Base.Interfaces;
using NHibernate;

namespace DataCleansing.Base.Implementations
{
    public class UnitOfWorkScope : IUnitOfWorkScope
    {
        private bool _disposed;
        private readonly ITransaction _scope;
        private readonly bool _isChildScope;
        
        public UnitOfWorkScope(System.Data.IsolationLevel isolation = System.Data.IsolationLevel.ReadCommitted, TimeSpan? timeout = null)
        {
            if (!(CallContext.GetData("call_context") is IUnitOfWork))
            {
                var unitOfWork = new NhUnitOfWork();

                _scope = unitOfWork.Session.BeginTransaction(isolation);

                CallContext.SetData("call_context", unitOfWork);
            }
            else
            {
                _isChildScope = true;
            }
        }

        public static IUnitOfWork CurrentUnitOfWork => CallContext.GetData("call_context") as IUnitOfWork;

        public Guid ScopeId { get; } = Guid.NewGuid();

        public IUnitOfWork UnitOfWork => CurrentUnitOfWork;

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            try
            {
                if (_isChildScope)
                {
                    return;
                }

                _scope?.Dispose();
                var data = (IUnitOfWork)CallContext.GetData("call_context");
                if (data != null)
                {
                    data.Dispose();
                    CallContext.SetData("call_context", null);
                }
            }
            finally
            {
                _disposed = true;
            }
        }

        public void Discard(object entyty)
        {
            ((IUnitOfWork)CallContext.GetData("call_context")).Discard(entyty);
        }

        public void Commit()
        {
            if (_isChildScope)
            {
                return;
            }

            ((IUnitOfWork)CallContext.GetData("call_context")).Flush();
            
            _scope.Commit();
        }
    }
}
