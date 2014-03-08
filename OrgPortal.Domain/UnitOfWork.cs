using OrgPortal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.Domain
{
    public interface UnitOfWork : IDisposable
    {
        CategoryRepository CategoryRepository { get; }
        ApplicationRepository ApplicationRepository { get; }
        void Commit();
    }
}
