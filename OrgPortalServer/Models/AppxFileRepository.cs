using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgPortalServer.Models
{
    public interface AppxFileRepository
    {
        AppxFile Get(string packageFamilyName);
        byte[] GetLogo(string packageFamilyName);
        byte[] GetSmallLogo(string packageFamilyName);
        bool Exists(string packageFamilyName);
        void Save(AppxFile file);
        void Delete(string packageFamilyName);
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