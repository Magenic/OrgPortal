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

        //public IEnumerable<AppxFile> Get()
        //{
        //    var result = new List<AppxFile>();
        //    foreach (var fileName in Directory.GetFiles(appFolder, "*.appx"))
        //        using (var file = File.OpenRead(fileName))
        //            result.Add(AppxFile.Create(file));
        //    return result;
        //}

        //public AppxFile Get(string name)
        //{
        //    if (!File.Exists(Path.Combine(appFolder, name + ".appx")))
        //        throw new ArgumentException("Unable to find an app by that name.");
        //    using (var file = File.OpenRead(Path.Combine(appFolder, name + ".appx")))
        //        return AppxFile.Create(file);
        //}

        public bool Exists(string name)
        {
            return File.Exists(Path.Combine(appFolder, name + ".appx"));
        }

        public void Save(AppxFile file)
        {
            File.WriteAllBytes(Path.Combine(appFolder, file.Info.Name + ".appx"), file.Package);
            File.WriteAllBytes(Path.Combine(appFolder, file.Info.Name + ".png"), file.Logo);
            File.WriteAllBytes(Path.Combine(appFolder, file.Info.Name + "-small.png"), file.SmallLogo);
        }

        public void Delete(string name)
        {
            if (!File.Exists(Path.Combine(appFolder, name + ".appx")))
                throw new ArgumentException("Unable to find an app by that name.");
            File.Delete(Path.Combine(appFolder, name + ".appx"));
            File.Delete(Path.Combine(appFolder, name + ".png"));
            File.Delete(Path.Combine(appFolder, name + "-small.png"));
        }
    }
}