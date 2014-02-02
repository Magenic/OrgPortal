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
        public byte[] Package { get; private set; }
        public string Name { get; private set; }
        public string Publisher { get; private set; }
        public string Version { get; private set; }
        public string ProcessorArchitecture { get; private set; }
        public string DisplayName { get; private set; }
        public string PublisherDisplayName { get; private set; }
        public byte[] Logo { get; private set; }
        public byte[] SmallLogo { get; private set; }

        private AppxFile() { }

        private AppxFile(Stream data)
        {
            ExtractValuesFromPackage(data);
            data.Seek(0, SeekOrigin.Begin);
            Package = ReadFully(data);
        }

        public void Save()
        {
            // TODO: Persist the raw APPX and the extracted values as an application to the backing data store.
            //       Could be an insert or an update, determine based on some identifier extracted from the APPX.
        }

        public void Delete()
        {
            // TODO: Delete the persisted application and the associated APPX from the backing data store.
            //       Identify it by some identifier extracted from the APPX.
        }

        // TODO: Should these factory methods outwardly discern between fetching a known app vs. building a new one?
        //       Unless we find a compelling reason to, I'd like to try keeping them the same.  If we make them different, update the controller.
        public static AppxFile Get(Stream data)
        {
            // TODO: Parse an identifier from the APPX data and use that to get the persisted APPX and other fields from the backing data store.
            return new AppxFile(data);
        }

        public static AppxFile Get(string id)
        {
            // TODO: Might not be a string, determine the unique identifier from the APPX when we've parsed that.
            // TODO: Get the APPX and related data from the backing data store
            return new AppxFile();
        }

        private void ExtractValuesFromPackage(Stream data)
        {
            using (var zipArchive = ZipFile.Read(data))
            {
                var manifest = GetManifestFromZip(zipArchive);
                ExtractName(manifest);
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
            Name = manifest.Descendants().Single(d => d.Name.LocalName == "Identity")
                           .Attributes().Single(a => a.Name.LocalName == "Name")
                           .Value;
        }

        private void ExtractPublisher(XDocument manifest)
        {
            Publisher = manifest.Descendants().Single(d => d.Name.LocalName == "Identity")
                                .Attributes().Single(a => a.Name.LocalName == "Publisher")
                                .Value;
        }

        private void ExtractVersion(XDocument manifest)
        {
            Version = manifest.Descendants().Single(d => d.Name.LocalName == "Identity")
                              .Attributes().Single(a => a.Name.LocalName == "Version")
                              .Value;
        }

        private void ExtractDisplayName(XDocument manifest)
        {
            DisplayName = manifest.Descendants().Single(d => d.Name.LocalName == "Properties")
                                  .Descendants().Single(d => d.Name.LocalName == "DisplayName")
                                  .Value;
        }

        private void ExtractPublisherDisplayName(XDocument manifest)
        {
            PublisherDisplayName = manifest.Descendants().Single(d => d.Name.LocalName == "Properties")
                                           .Descendants().Single(d => d.Name.LocalName == "PublisherDisplayName")
                                           .Value;
        }

        private void ExtractProcessorArchitecture(XDocument manifest)
        {
            ProcessorArchitecture = manifest.Descendants().Single(d => d.Name.LocalName == "Identity")
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
            var logoFileName = manifest.Descendants().Single(d => d.Name.LocalName == "Applications")
                                       .Descendants().First(d => d.Name.LocalName == "Application") // TODO: What if there is more than one application?
                                       .Descendants().Single(d => d.Name.ToString().EndsWith("VisualElements", StringComparison.InvariantCultureIgnoreCase))
                                       .Attributes().Single(a => a.Name.LocalName == attributeName)
                                       .Value;
            var fileName = Path.GetFileNameWithoutExtension(logoFileName);
            var fileExtension = Path.GetExtension(logoFileName);
            var logoFile = zipArchive.Entries.Single(e => Path.GetFileName(e.FileName).StartsWith(fileName, StringComparison.InvariantCultureIgnoreCase) &&
                                                          Path.GetExtension(e.FileName).Equals(fileExtension, StringComparison.InvariantCultureIgnoreCase));
            var logoData = new MemoryStream();
            logoFile.Extract(logoData);
            return logoData;
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