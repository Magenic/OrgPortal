using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace OrgPortalServer.Models
{
    public class AppxFileRepositoryImpl : AppxFileRepository
    {
        private static string appFolder;

        static AppxFileRepositoryImpl()
        {
            appFolder = ConfigurationManager.AppSettings["AppFolder"];
            if (string.IsNullOrWhiteSpace(appFolder))
                throw new ConfigurationErrorsException("AppFolder configuration value is missing.");
            if (!Directory.Exists(appFolder))
                Directory.CreateDirectory(appFolder);
        }

        public AppxFile Get(string packageFamilyName)
        {
            if (!File.Exists(Path.Combine(appFolder, packageFamilyName + ".appx")))
                throw new ArgumentException("Unable to find an app by that name.");
            using (var file = File.OpenRead(Path.Combine(appFolder, packageFamilyName + ".appx")))
                return AppxFile.Create(file);
        }

        public byte[] GetLogo(string packageFamilyName)
        {
            if (!File.Exists(Path.Combine(appFolder, packageFamilyName + ".png")))
                throw new ArgumentException("Unable to find an app by that name.");
            return File.ReadAllBytes(Path.Combine(appFolder, packageFamilyName + ".png"));
        }

        public byte[] GetSmallLogo(string packageFamilyName)
        {
            if (!File.Exists(Path.Combine(appFolder, packageFamilyName + "-small.png")))
                throw new ArgumentException("Unable to find an app by that name.");
            return File.ReadAllBytes(Path.Combine(appFolder, packageFamilyName + "-small.png"));
        }

        public bool Exists(string packageFamilyName)
        {
            return File.Exists(Path.Combine(appFolder, packageFamilyName + ".appx"));
        }

        public void Save(AppxFile file)
        {
            File.WriteAllBytes(Path.Combine(appFolder, file.Info.PackageFamilyName + ".appx"), file.Package);
            File.WriteAllBytes(Path.Combine(appFolder, file.Info.PackageFamilyName + ".png"), file.Logo);
            File.WriteAllBytes(Path.Combine(appFolder, file.Info.PackageFamilyName + "-small.png"), file.SmallLogo);
        }

        public void Delete(string packageFamilyName)
        {
            if (!File.Exists(Path.Combine(appFolder, packageFamilyName + ".appx")))
                throw new ArgumentException("Unable to find an app by that name.");
            File.Delete(Path.Combine(appFolder, packageFamilyName + ".appx"));
            File.Delete(Path.Combine(appFolder, packageFamilyName + ".png"));
            File.Delete(Path.Combine(appFolder, packageFamilyName + "-small.png"));
        }
    }
}