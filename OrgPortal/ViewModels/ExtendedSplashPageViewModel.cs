using OrgPortal.Common;
using OrgPortal.DataModel;
using System.Composition;
using System.Threading.Tasks;

namespace OrgPortal.ViewModels
{
    [Export]
    public class ExtendedSplashPageViewModel : ViewModelBase
    {
        private readonly IPortalDataSource _dataSource;
        private readonly BrandingViewModel _brandingViewModel;


        [ImportingConstructor]
        public ExtendedSplashPageViewModel(INavigation navigation, IPortalDataSource dataSource, BrandingViewModel brandingViewModel)
            : base(navigation)
        {
            _dataSource = dataSource;
            _brandingViewModel = brandingViewModel;

            LoadBrandingInfo();
        }

        private async Task LoadBrandingInfo()
        {
            var branding = await _dataSource.GetBrandingDataAsync();
            if (branding != null)
            {
                await _brandingViewModel.UpdateAsync(branding);
            }

            Navigation.NavigateToViewModel<MainPageViewModel>();
        }
    }
}
