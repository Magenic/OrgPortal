using OrgPortal.Domain;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.IoC
{
    public class IoCInitializer
    {
        public static void Initialize()
        {
            var assemblies = new List<Assembly>
            {
                typeof(OrgPortal.Domain.IoCContainerFactory).Assembly,
                typeof(OrgPortal.Infrastructure.DAL.UnitOfWorkImplementation).Assembly
            };

            InitializeContainer(assemblies);
        }

        private static void InitializeContainer(List<Assembly> assemblies)
        {
            ObjectFactory.Configure(x =>
            {
                x.Scan(y =>
                {
                    y.TheCallingAssembly();
                    y.WithDefaultConventions();
                });
                assemblies.ForEach(a =>
                    x.Scan(y =>
                    {
                        y.Assembly(a);
                        y.WithDefaultConventions();
                        y.Convention<DomainInterfaceNamingConvention>();
                    }));
            });

            ObjectFactory.Container.AssertConfigurationIsValid();

            IoCContainerFactory.Initialize(new IoCContainerImplementation(ObjectFactory.Container));
        }
    }
}
