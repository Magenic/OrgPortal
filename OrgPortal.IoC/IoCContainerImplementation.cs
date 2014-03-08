using OrgPortal.Domain;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.IoC
{
    public class IoCContainerImplementation : IoCContainer
    {
        private readonly IContainer _container;

        public IoCContainerImplementation(IContainer container)
        {
            _container = container;
        }

        public T GetInstance<T>()
        {
            return _container.GetInstance<T>();
        }
    }
}
