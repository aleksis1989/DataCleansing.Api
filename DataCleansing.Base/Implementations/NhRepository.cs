using System;
using System.Collections.Generic;
using System.Linq;
using DataCleansing.Base.Interfaces;
using NHibernate;

namespace DataCleansing.Base.Implementations
{
    public class NhRepository<TEntity> : IRepository<TEntity>
    {
        protected ISession Session
        {
            get
            {
                if (UnitOfWorkScope.CurrentUnitOfWork is NhUnitOfWork currentUnitOfWork)
                {
                    return currentUnitOfWork.Session;
                }

                throw new Exception("not valid unit of work");
            }
        }

        public TEntity Get(object id)
        {
            return Session.Get<TEntity>(id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Session.Query<TEntity>().ToList();
        }

        public IQueryable<TEntity> Query()
        {
            return Session.Query<TEntity>();
        }

        public virtual void Save(TEntity entity)
        {
            Session.Save(entity);
            Session.Flush();
        }

        public virtual void Update(TEntity entity)
        {
            Session.Update(entity);
            Session.Flush();
        }

        public virtual void SaveOrUpdate(TEntity entity)
        {
            Session.SaveOrUpdate(entity);
            Session.Flush();
        }

        public void Delete(TEntity entity)
        {
            Session.Delete(entity);
            Session.Flush();
        }

        public void DeleteById(object id)
        {
            Session
                .CreateQuery($"delete {typeof(TEntity)} where id = :id")
                .SetParameter(nameof(id), id).ExecuteUpdate();
            Session.Flush();
        }
    }
}
