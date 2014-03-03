using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace OrgPortalServer.Models
{
    public class AppxFile
    {
        public AppInfo Info { get; private set; }
        public byte[] Package { get; private set; }
        public byte[] Logo { get; private set; }
        public byte[] SmallLogo { get; private set; }

        private AppxFile() { }

        private AppxFile(Stream data)
        {
            Info = new AppInfo();
            ExtractValuesFromPackage(data);
            data.Seek(0, SeekOrigin.Begin);
            Package = ReadFully(data);
        }

        public void Save()
        {
            AppxFileRepositoryFactory.Current.Save(this);
            AppInfoRepositoryFactory.Current.Save(Info);
        }

        public bool Exists()
        {
            return AppxFileRepositoryFactory.Current.Exists(Info.PackageFamilyName);
        }

        public static AppxFile Create(Stream data)
        {
            return new AppxFile(data);
        }

        public static AppxFile Get(string packageFamilyName)
        {
            return AppxFileRepositoryFactory.Current.Get(packageFamilyName);
        }

        public static byte[] GetLogo(string packageFamilyName)
        {
            return AppxFileRepositoryFactory.Current.GetLogo(packageFamilyName);
        }

        public static byte[] GetSmallLogo(string packageFamilyName)
        {
            return AppxFileRepositoryFactory.Current.GetSmallLogo(packageFamilyName);
        }

        private void ExtractValuesFromPackage(Stream data)
        {
            using (var zipArchive = ZipFile.Read(data))
            {
                var manifest = GetManifestFromZip(zipArchive);
                ExtractName(manifest);
                ExtractDescription(manifest);
                ExtractPublisher(manifest);
                ExtractVersion(manifest);
                ExtractProcessorArchitecture(manifest);
                ExtractDisplayName(manifest);
                ExtractPublisherDisplayName(manifest);
                ExtractLogo(zipArchive, manifest);
                ExtractSmallLogo(zipArchive, manifest);
            }
        }

        private static XDocument GetManifestFromZip(ZipFile zipArchive)
        {
            // Juggling streams because they were closed when I accessed them before
            // See here: http://stackoverflow.com/q/8100590/328193
            // This can probably be made more efficient
            var manifestFile = zipArchive.Entries.Single(e => e.FileName == "AppxManifest.xml");
            var manifestData = new MemoryStream();
            manifestFile.Extract(manifestData);
            var manifest = XDocument.Load(new MemoryStream(manifestData.ToArray()));
            return manifest;
        }

        private void ExtractName(XDocument manifest)
        {
            Info.Name = manifest.Descendants().Single(d => d.Name.LocalName == "Identity")
                                .Attributes().Single(a => a.Name.LocalName == "Name")
                                .Value;
        }

        private void ExtractDescription(XDocument manifest)
        {
            Info.Description = ExtractValueFromVisualElementsNode(manifest, "Description");
        }

        private void ExtractPublisher(XDocument manifest)
        {
            Info.Publisher = manifest.Descendants().Single(d => d.Name.LocalName == "Identity")
                                     .Attributes().Single(a => a.Name.LocalName == "Publisher")
                                     .Value;
        }

        private void ExtractVersion(XDocument manifest)
        {
            Info.Version = manifest.Descendants().Single(d => d.Name.LocalName == "Identity")
                                   .Attributes().Single(a => a.Name.LocalName == "Version")
                                   .Value;
        }

        private void ExtractDisplayName(XDocument manifest)
        {
            Info.DisplayName = manifest.Descendants().Single(d => d.Name.LocalName == "Properties")
                                       .Descendants().Single(d => d.Name.LocalName == "DisplayName")
                                       .Value;
        }

        private void ExtractPublisherDisplayName(XDocument manifest)
        {
            Info.PublisherDisplayName = manifest.Descendants().Single(d => d.Name.LocalName == "Properties")
                                                .Descendants().Single(d => d.Name.LocalName == "PublisherDisplayName")
                                                .Value;
        }

        private void ExtractProcessorArchitecture(XDocument manifest)
        {
            Info.ProcessorArchitecture = manifest.Descendants().Single(d => d.Name.LocalName == "Identity")
                                                 .Attributes().Single(a => a.Name.LocalName == "ProcessorArchitecture")
                                                 .Value;
        }

        private void ExtractLogo(ZipFile zipArchive, XDocument manifest)
        {
            var logoData = ExtractAssetImageFromVisualElementsNode(zipArchive, manifest, "Square150x150Logo");
            Logo = ReadFully(new MemoryStream(logoData.ToArray()));
        }

        private void ExtractSmallLogo(ZipFile zipArchive, XDocument manifest)
        {
            var logoData = ExtractAssetImageFromVisualElementsNode(zipArchive, manifest, "Square30x30Logo");
            SmallLogo = ReadFully(new MemoryStream(logoData.ToArray()));
        }

        private static MemoryStream ExtractAssetImageFromVisualElementsNode(ZipFile zipArchive, XDocument manifest, string attributeName)
        {
            var logoFileName = ExtractValueFromVisualElementsNode(manifest, attributeName);
            var fileName = Path.GetFileNameWithoutExtension(logoFileName);
            var fileExtension = Path.GetExtension(logoFileName);
            var logoFile = zipArchive.Entries.Last(e => Path.GetFileName(e.FileName).StartsWith(fileName, StringComparison.InvariantCultureIgnoreCase) &&
                                                        Path.GetExtension(e.FileName).Equals(fileExtension, StringComparison.InvariantCultureIgnoreCase));
            var logoData = new MemoryStream();
            logoFile.Extract(logoData);
            return logoData;
        }

        private static string ExtractValueFromVisualElementsNode(XDocument manifest, string attributeName)
        {
            var logoFileName = manifest.Descendants().Single(d => d.Name.LocalName == "Applications")
                                       .Descendants().First(d => d.Name.LocalName == "Application") // TODO: What if there is more than one application?
                                       .Descendants().Single(d => d.Name.ToString().EndsWith("VisualElements", StringComparison.InvariantCultureIgnoreCase))
                                       .Attributes().Single(a => a.Name.LocalName == attributeName)
                                       .Value;
            return logoFileName;
        }

        private static byte[] ReadFully(Stream input)
        {
            // Provided by Jon Skeet: http://stackoverflow.com/a/221941/328193
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);
                return ms.ToArray();
            }
        }
    }
}