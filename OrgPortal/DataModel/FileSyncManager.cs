using OrgPortalMonitor.DataModel;
using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;

namespace OrgPortal.DataModel
{
    [Export(typeof(IFileSyncManager))]
    [Shared]
    public class FileSyncManager : IFileSyncManager
    {
        private const string WRITE_FILE_EXTENSION = ".rt2win";
        private const string READ_FILE_EXTENSION = ".win2rt";

        public async Task RequestAppInstall(string appxUrl)
        {
            var content = new string[] { "install", appxUrl };
            await WriteTempFile(content);
        }

        public async Task<List<AppInfo>> GetInstalledApps()
        {
            var results = new List<AppInfo>();
            var folder = GetSyncFolder();
            var file = await folder.GetFileAsync("InstalledPackages.txt");
            if (file != null)
            {
                var installedPackages = await FileIO.ReadLinesAsync(file);
                AppInfo info = new AppInfo();
                foreach (var line in installedPackages)
                {
                    if (line.StartsWith("Name"))
                        info.Name = line.Substring(line.IndexOf(":") + 2);
                    else if (line.StartsWith("PackageFamilyName"))
                        info.PackageFamilyName = line.Substring(line.IndexOf(":") + 2);
                    else if (line.StartsWith("Version"))
                        info.Version = line.Substring(line.IndexOf(":") + 2);
                    else if (line.StartsWith("IsDevelopmentMode"))
                    {
                        results.Add(info);
                        info = new AppInfo();
                    }
                }
            }

            return results;
        }

        public async Task UpdateDevLicense()
        {
            var content = new string[] { "getDevLicense" };
            await WriteTempFile(content);
        }

        public async Task<IReadOnlyList<StorageFile>> ReadFiles()
        {
            var folder = GetSyncFolder();
            var criteria = new string[] { READ_FILE_EXTENSION };
            var query = folder.CreateFileQueryWithOptions(new QueryOptions(CommonFileQuery.DefaultQuery, criteria));
            //query.ContentsChanged += async (o, a) =>
            //{
            //    var files = await query.GetFilesAsync();
            //    var count = files.Count();
            //};
            return await query.GetFilesAsync();
        }

        private StorageFolder GetSyncFolder()
        {
            return ApplicationData.Current.TemporaryFolder;
        }

        private string GenerateFileName()
        {
            return string.Format("{0}{1}", Path.GetRandomFileName(), WRITE_FILE_EXTENSION);
        }

        private async Task WriteTempFile(string[] content)
        {
            var folder = GetSyncFolder();
            var file = await folder.CreateFileAsync(GenerateFileName(), CreationCollisionOption.OpenIfExists);
            
            await FileIO.WriteLinesAsync(file, content);
        }
    }
}
