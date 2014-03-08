using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.Domain
{
    public interface IoCContainer
    {
        T GetInstance<T>();
    }

    public static class IoCContainerFactory
    {
        public static IoCContainer Current { get; private set; }

        public static void Initialize(IoCContainer container)
        {
            Current = container;
        }
    }
}
