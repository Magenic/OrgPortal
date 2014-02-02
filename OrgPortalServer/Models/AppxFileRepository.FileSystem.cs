using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace OrgPortalServer.Models
{
    public class AppxFileRepositoryFileSystem : AppxFileRepository
    {
        private static string appFolder;

        static AppxFileRepositoryFileSystem()
        {
            appFolder = ConfigurationManager.AppSettings["AppFolder"];
            if (string.IsNullOrWhiteSpace(appFolder))
                throw new ConfigurationErrorsException("AppFolder configuration value is missing.");
            if (!Directory.Exists(appFolder))
                Directory.CreateDirectory(appFolder);
        }

        public AppxFile Get(string name)
        {
            if (!File.Exists(Path.Combine(appFolder, name + ".appx")))
                throw new ArgumentException("Unable to find an app by that name.");
            using (var file = File.OpenRead(Path.Combine(appFolder, name + ".appx")))
                return AppxFile.Create(file);
        }

        public void Save(AppxFile file)
        {
            File.WriteAllBytes(Path.Combine(appFolder, file.Name + ".appx"), file.Package);
        }

        public void Delete(AppxFile file)
        {
            if (!File.Exists(Path.Combine(appFolder, file.Name + ".appx")))
                throw new ArgumentException("Unable to find an app by that name.");
            File.Delete(Path.Combine(appFolder, file.Name + ".appx"));
        }
    }
}