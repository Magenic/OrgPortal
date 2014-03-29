using Caliburn.Micro;
using OrgPortal.Common;
using OrgPortal.DataModel;
using System;
using System.Composition;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace OrgPortal.ViewModels
{
    [Export]
    [Shared]
    public class BrandingViewModel : PropertyChangedBase
    {
        private const string storageFolderName = "Assets";
        private const string brandingContainerKey = "BrandingSettings";
        private const string appNameKey = "AppName";
        private const string primaryColorKey = "PrimaryColor";
        private const string secondaryColorKey = "SecondaryColor";
        private const string headerImageName = "HeaderImage.png";
        private const string logoImageName = "LogoImage.png";

        private ApplicationDataContainer _settings;


        public BrandingViewModel()
        {
            _settings = ApplicationData.Current.LocalSettings.CreateContainer(brandingContainerKey, ApplicationDataCreateDisposition.Always);
        }


        public string Name
        {
            get { return GetSetting(appNameKey); }
            set
            {
                _settings.Values[appNameKey] = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        private BitmapImage _headerImage;
        public BitmapImage HeaderImage
        {
            get { return _headerImage; }
            private set
            {
                _headerImage = value;
                NotifyOfPropertyChange(() => HeaderImage);
            }
        }

        private BitmapImage _logoImage;
        public BitmapImage LogoImage
        {
            get { return _logoImage; }
            private set
            {
                _logoImage = value;
                NotifyOfPropertyChange(() => LogoImage);
            }
        }


        private string GetSetting(string settingName)
        {
            if (_settings.Values.ContainsKey(settingName))
                return _settings.Values[settingName].ToString();

            return null;
        }

        private async Task<BitmapImage> GetImageAsync(string imageName)
        {
            if (string.IsNullOrWhiteSpace(imageName))
                return null;

            var assetFolder = await GetAssetFolderAsync();
            var imageFile = await assetFolder.GetFileAsync(imageName);

            return GetImage(imageFile);            
        }

        private BitmapImage GetImage(IStorageFile file)
        {
            if (file == null)
                return null;

            return new BitmapImage(new Uri(file.Path));
        }

        public async Task UpdateAsync(BrandingInfo branding)
        {
            if (branding == null)
                return;

            Name = branding.Name;
            
            if (!string.IsNullOrWhiteSpace(branding.PrimaryColor))
            {
                ThemeManager.CurrentTheme.PrimaryColor = Theme.ColorFromHex(branding.PrimaryColor);
                _settings.Values[primaryColorKey] = branding.PrimaryColor;
            }

            if (!string.IsNullOrWhiteSpace(branding.SecondaryColor))
            {
                ThemeManager.CurrentTheme.AccentColor = Theme.ColorFromHex(branding.SecondaryColor);
                _settings.Values[secondaryColorKey] = branding.SecondaryColor;
            }

            if (branding.HeaderImage != null)
            {
                var image = await SaveFileAsync(headerImageName, branding.HeaderImage);
                HeaderImage = GetImage(image);
            }
            else
            {
                await DeleteFileAsync(headerImageName);
            }

            if (branding.LogoImage != null)
            {
                var image = await SaveFileAsync(logoImageName, branding.LogoImage);
                LogoImage = GetImage(image);
            }
            else
            {
                await DeleteFileAsync(logoImageName);
            }
        }

        private async Task<IStorageFolder> GetAssetFolderAsync()
        {
            var assetFolder = await ApplicationData.Current.LocalFolder.TryGetItemAsync(storageFolderName) as IStorageFolder;
            if (assetFolder == null)
            {
                assetFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(storageFolderName);
            }

            return assetFolder;
        }

        private async Task<IStorageFile> SaveFileAsync(string fileName, byte[] data)
        {
            var assetFolder = await GetAssetFolderAsync();

            var file = await assetFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteBytesAsync(file, data);

            return file;
        }

        private async Task DeleteFileAsync(string fileName)
        {
            var assetFolder = await GetAssetFolderAsync();

            var file = await assetFolder.GetFileAsync(fileName);
            if (file != null)
            {
                await file.DeleteAsync();
            }
        }

    }
}
