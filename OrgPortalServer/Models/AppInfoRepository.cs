using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortalServer.Models
{
    public interface AppInfoRepository
    {
        IEnumerable<AppInfo> Get();
        AppInfo Get(string packageFamilyName);
        void Save(AppInfo app);
        void Delete(string packageFamilyName);
    }

    public class AppInfoRepositoryFactory
    {
        public static AppInfoRepository Current
        {
            get
            {
                // TODO: Dependency injection?  Some other means of swapping out implementations?
                return new AppInfoRepositoryImpl();
            }
        }
    }
}
