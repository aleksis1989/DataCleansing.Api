using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using DataCleansing.Base.Interfaces;
using NHibernate;
using Configuration = NHibernate.Cfg.Configuration;

namespace DataCleansing.Base.Implementations
{
    public class NhUnitOfWork : IUnitOfWork
    {
        private static readonly ISessionFactory _sessionFactory;

        static NhUnitOfWork()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var hibernateConfigPath = GetHibernateConfigPath(environment);

            var configuration = new Configuration();
            configuration.Configure(hibernateConfigPath);
            configuration.BuildMappings();
            
            var section = (NameValueCollection)ConfigurationManager.GetSection("nhibernate.interceptors");
            if (section != null)
            {
                foreach (string typeName in section)
                {
                    if (typeName != null)
                    {
                        var type = Type.GetType(typeName);
                        if (type != null)
                        {
                            configuration.SetInterceptor((IInterceptor)Activator.CreateInstance(type));
                        }
                    }
                }
            }

            if (_sessionFactory != null)
            {
                return;
            }

            _sessionFactory = configuration.BuildSessionFactory();
        }

        public NhUnitOfWork()
        {
            Session = _sessionFactory.OpenSession();
        }

        public ISession Session { get; }

        public void Flush()
        {
            Session.Flush();
        }

        public void Discard(object entyty)
        {
            Session.Evict(entyty);
        }

        public void Dispose()
        {
            if (Session.IsOpen)
            {
                Session.Close();
            }

            Session.Dispose();
        }

        private static string GetHibernateConfigPath(string environment)
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;

            // Note RelativeSearchPath can be null even if the doc say something else; don't remove the check
            var searchPath = AppDomain.CurrentDomain.RelativeSearchPath ?? string.Empty;

            var relativeSearchPath = searchPath.Split(';').First();
            var binPath = Path.Combine(baseDir ?? string.Empty, relativeSearchPath);
            var fileName = $"hibernate.cfg.xml";
           
            return Path.Combine(binPath, fileName);
        }
    }
}
