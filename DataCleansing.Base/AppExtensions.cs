using System.Linq;
using System.Reflection;
using DataCleansing.Base.Implementations;
using DataCleansing.Base.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DataCleansing.Base
{
    public static class AppExtensions
    {
        public static void AddAppDependencies(this IServiceCollection services)
        {
            services.AddNHibernate();
            services.AddAssemblyTypes("CleansingData.Data", "Repository");
            services.AddAssemblyTypes("DataCleansing.Services", "Service");
        }

        private static void AddNHibernate(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(NhRepository<>));
        }

        private static void AddAssemblyTypes(this IServiceCollection services, string assemblyName, string suffix)
        {
            var assembly = Assembly.Load(assemblyName);
            var types = assembly.GetTypes()
                .Where(t => t.Name.EndsWith(suffix))
                .ToList();

            foreach (var type in types)
            {
                var interfaceTypes = type.GetInterfaces()
                    .Where(t => t.Name.EndsWith(suffix))
                    .Where(interfaceType => interfaceType.IsAssignableFrom(type));


                foreach (var interfaceType in interfaceTypes)
                {
                    services.AddTransient(interfaceType, type);
                    break;
                }
            }
        }
    }
}
