using OrgPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.Domain.Repositories
{
    public interface ApplicationRepository
    {
        IQueryable<Application> Applications { get; }
        void Add(Application model);
        void Remove(Application model);

        byte[] GetAppx(string packageFamilyName);
        byte[] GetLogo(string packageFamilyName);
        byte[] GetSmallLogo(string packageFamilyName);
    }
}
