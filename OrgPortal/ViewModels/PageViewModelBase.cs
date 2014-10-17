using OrgPortal.Common;

namespace OrgPortal.ViewModels
{
    public class PageViewModelBase : ViewModelBase
    {
        protected PageViewModelBase(INavigation navigation, INavigationBar navBar, BrandingViewModel brandingViewModel)
            : base(navigation)
        {
            _navigationBar = navBar;
            _brandingViewModel = brandingViewModel;
        }

        private readonly INavigationBar _navigationBar;
        public INavigationBar NavigationBar
        {
            get { return _navigationBar; }
        }

        private readonly BrandingViewModel _brandingViewModel;
        public BrandingViewModel Branding
        {
            get { return _brandingViewModel; }
        }
        
        protected override async void OnInitialize()
        {
            base.OnInitialize();
            await NavigationBar.LoadCategories();
        }
    }
}
