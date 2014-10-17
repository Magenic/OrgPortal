// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace OrgPortal
{
    using Windows.Storage;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed partial class CustomSettings : SettingsFlyout
    {
        private const string DefaultServiceUri = "http://orgportal.natch.local/api/";

        public CustomSettings()
        {
            this.InitializeComponent();

            if (ApplicationData.Current.LocalSettings.Values["OrgPortalApi"] == null)
            {
                ApplicationData.Current.LocalSettings.Values["OrgPortalApi"] = DefaultServiceUri;
            }

            this.ApiOrgPortal.Text = ApplicationData.Current.LocalSettings.Values["OrgPortalApi"].ToString();
        }

        private void ButtonApiOrgPortal_OnClick(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["OrgPortalApi"] = this.ApiOrgPortal.Text;
        }
    }
}
