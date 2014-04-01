using Ionic.Zip;
using OrgPortal.Domain.Extensions;
using OrgPortal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrgPortal.Domain.Models
{
    public class Application
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Publisher { get; private set; }
        public string Version { get; private set; }
        public string ProcessorArchitecture { get; private set; }
        public string DisplayName { get; private set; }
        public string PublisherDisplayName { get; private set; }
        public string InstallMode { get; private set; }
        public string PackageFamilyName { get; private set; }
        public DateTime DateAdded { get; private set; }
        public string BackgroundColor { get; private set; }
        public int CategoryID { get; set; }

        // TODO: Make this into a proper navigation property somehow
        public Category Category
        {
            get { return IoCContainerFactory.Current.GetInstance<CategoryRepository>().Categories.Single(c => c.ID == CategoryID); }
        }

        private byte[] _package;
        public byte[] Package
        {
            get
            {
                if (_package == null || _package.Length == 0)
                    _package = IoCContainerFactory.Current.GetInstance<ApplicationRepository>().GetAppx(PackageFamilyName);
                return _package;
            }
            set { _package = value; }
        }

        private byte[] _logo;
        public byte[] Logo
        {
            get
            {
                if (_logo == null || _logo.Length == 0)
                    _logo = IoCContainerFactory.Current.GetInstance<ApplicationRepository>().GetLogo(PackageFamilyName);
                return _logo;
            }
            set { _logo = value; }
        }

        private byte[] _smallLogo;
        public byte[] SmallLogo
        {
            get
            {
                if (_smallLogo == null || _smallLogo.Length == 0)
                    _smallLogo = IoCContainerFactory.Current.GetInstance<ApplicationRepository>().GetSmallLogo(PackageFamilyName);
                return _smallLogo;
            }
            set { _smallLogo = value; }
        }

        private Application() { }

        public Application(Stream data, int categoryID, string installMode)
        {
            CategoryID = categoryID;
            InstallMode = installMode;
            DateAdded = DateTime.UtcNow;
            ExtractValuesFromPackage(data);
            data.Seek(0, SeekOrigin.Begin);
            Package = data.ReadFully();

            // TODO: This is not correct.  Publisher needs to be the Publisher ID, which is a hash of something.
            //       Need to figure out how to calculate/fetch the Publisher ID.
            PackageFamilyName = Name + "_" + Publisher;
        }

        // TODO: Move all of this extraction logic into an infrastructure assembly to get the Zip references out of the domain?
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
                ExtractBackgroundColor(manifest);
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
            Name = ExtractValueFromVisualElementsNode(manifest, "DisplayName");
        }

        private void ExtractBackgroundColor(XDocument manifest)
        {
            BackgroundColor = ExtractValueFromVisualElementsNode(manifest, "BackgroundColor");
        }

        private void ExtractDescription(XDocument manifest)
        {
            Description = ExtractValueFromVisualElementsNode(manifest, "Description");
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
            Logo = new MemoryStream(logoData.ToArray()).ReadFully();
        }

        private void ExtractSmallLogo(ZipFile zipArchive, XDocument manifest)
        {
            var logoData = ExtractAssetImageFromVisualElementsNode(zipArchive, manifest, "Square30x30Logo");
            SmallLogo = new MemoryStream(logoData.ToArray()).ReadFully();
        }

        private static MemoryStream ExtractAssetImageFromVisualElementsNode(ZipFile zipArchive, XDocument manifest, string attributeName)
        {
            var logoFileName = ExtractValueFromVisualElementsNode(manifest, attributeName);
            var fileName = Path.GetFileNameWithoutExtension(logoFileName);
            var fileExtension = Path.GetExtension(logoFileName);
            var logoFile = zipArchive.Entries.SingleOrDefault(e => Path.GetFileNameWithoutExtension(e.FileName).Equals(string.Format("{0}.scale-100", fileName), StringComparison.InvariantCultureIgnoreCase) &&
                                                                   Path.GetExtension(e.FileName).Equals(fileExtension, StringComparison.InvariantCultureIgnoreCase)) ??
                           zipArchive.Entries.SingleOrDefault(e => Path.GetFileNameWithoutExtension(e.FileName).Equals(string.Format("{0}", fileName), StringComparison.InvariantCultureIgnoreCase) &&
                                                                   Path.GetExtension(e.FileName).Equals(fileExtension, StringComparison.InvariantCultureIgnoreCase));
            var logoData = new MemoryStream();
            if (logoFile != null)
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
    }
}
