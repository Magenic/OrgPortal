using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgPortalServer.Models
{
    public interface AppxFileRepository
    {
        IEnumerable<AppxFile> Get();
        AppxFile Get(string name);
        void Save(AppxFile file);
        void Delete(AppxFile file);
    }

    public class AppxFileRepositoryFactory
    {
        public static AppxFileRepository Current
        {
            get
            {
                // TODO: Dependency injection?  Some other means of swapping out implementations?
                //return new AppxFileRepositoryMock();
                return new AppxFileRepositoryFileSystem();
            }
        }
    }
}