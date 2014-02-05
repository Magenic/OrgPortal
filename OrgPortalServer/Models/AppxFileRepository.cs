using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgPortalServer.Models
{
    public interface AppxFileRepository
    {
        //IEnumerable<AppxFile> Get();
        //AppxFile Get(string name);
        bool Exists(string name);
        void Save(AppxFile file);
        void Delete(string name);
    }

    public class AppxFileRepositoryFactory
    {
        public static AppxFileRepository Current
        {
            get
            {
                // TODO: Dependency injection?  Some other means of swapping out implementations?
                return new AppxFileRepositoryImpl();
            }
        }
    }
}