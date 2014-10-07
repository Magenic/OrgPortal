using OrgPortal.Common;
using OrgPortal.DataModel;
using OrgPortalMonitor.DataModel;
using System;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace OrgPortal.ViewModels
{
    [Export]
    public class AppDetailsPageViewModel : PageViewModelBase
    {
        private readonly IMessageBox _messageBox;
        private readonly IFileSyncManager _fileManager;
        private AppInfo _installedItem;

        [ImportingConstructor]
        public AppDetailsPageViewModel(INavigation navigation, 
            IMessageBox messageBox, 
            INavigationBar navBar, 
            IFileSyncManager fileManager,
            BrandingViewModel branding)
            : base(navigation, navBar, branding)
        {
            this._messageBox = messageBox;
            this._fileManager = fileManager;
        }

        private AppInfo _item;
        public AppInfo Item
        {
            get { return _item; }
            private set
            {
                _item = value;
                NotifyOfPropertyChange(() => Item);
            }
        }

        private string _installInfo;
        public string InstallInfo
        {
            get { return _installInfo; }
            private set
            {
                _installInfo = value;
                NotifyOfPropertyChange(() => InstallInfo);
            }
        }

        private bool _controlEnabled;
        public bool ControlEnabled
        {
            get { return _controlEnabled; }
            private set
            {
                _controlEnabled = value;
                NotifyOfPropertyChange(() => ControlEnabled);
            }
        }

        public bool IsInstalled
        {
            get { return _installedItem != null; }
        }

        private bool _updateAvailable = false;
        public bool UpdateAvailable
        {
            get { return _updateAvailable; }
        }

        protected override void DeserializeParameter(string value)
        {
            Item = Serializer.Deserialize<AppInfo>(value);
        }

        public async Task Install()
        {
            await _fileManager.RequestAppInstall(Item.AppxUrl);
            ////await _messageBox.ShowAsync("Install request sent.", "Install App");
            
            // NATCH 07/10/2014 -> extra installation info
            this.InstallInfo = "Install request has been sent. Please wait...";
            this.ControlEnabled = false;

            await Task.Delay(15000); // 15 sec

            await _messageBox.ShowAsync("The app has been installed successfully!", "Success");
            this.InstallInfo = string.Empty;
            this.ControlEnabled = true;
        }

        //public async Task Update()
        //{

        //}

        //public async Task Uninstall()
        //{

        //}

        private async Task LoadData()
        {
            var apps = await _fileManager.GetInstalledApps();

            _installedItem = apps.FirstOrDefault(a => a.PackageFamilyName == Item.PackageFamilyName);
            NotifyOfPropertyChange(() => IsInstalled);

            CheckUpdate();
        }

        private void CheckUpdate()
        {
            if (IsInstalled)
            {
                Version itemVersion = new Version(Item.Version);
                Version installedVersion = new Version(_installedItem.Version);

                _updateAvailable = itemVersion > installedVersion;
                NotifyOfPropertyChange(() => UpdateAvailable);
            }
        }

    }
}