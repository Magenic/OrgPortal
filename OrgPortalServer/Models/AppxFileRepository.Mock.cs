using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgPortalServer.Models
{
    public class AppxFileRepositoryMock : AppxFileRepository
    {
        private List<AppxFile> files = new List<AppxFile>();

        public AppxFile Get(string name)
        {
            var file = files.SingleOrDefault(f => f.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (file == null)
                throw new ArgumentException("Unable to find an app by that name.");
            return file;
        }

        public void Save(AppxFile file)
        {
            var existingFile = files.SingleOrDefault(f => f.Name.Equals(file.Name, StringComparison.InvariantCultureIgnoreCase));
            if (existingFile != null)
                Delete(file);
            files.Add(file);
        }

        public void Delete(AppxFile file)
        {
            var existingFile = files.SingleOrDefault(f => f.Name.Equals(file.Name, StringComparison.InvariantCultureIgnoreCase));
            if (existingFile == null)
                throw new ArgumentException("Unable to find an app by that name.");
            files.Remove(existingFile);
        }
    }
}