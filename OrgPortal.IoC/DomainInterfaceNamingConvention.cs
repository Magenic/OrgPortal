using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.IoC
{
    public class DomainInterfaceNamingConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (IsNotRegisterable(type))
                return;

            var interfaceType = type.GetInterface(type.Name.Replace("Implementation", string.Empty));
            registry.AddType(interfaceType, type);
        }

        private bool IsNotRegisterable(Type type)
        {
            return type.IsAbstract || !type.IsClass || !ImplementsACustomInterface(type);
        }

        private bool ImplementsACustomInterface(Type type)
        {
            foreach (var iface in type.GetInterfaces())
                if (iface.Namespace.Contains("OrgPortal"))
                    return true;
            return false;
        }
    }
}
