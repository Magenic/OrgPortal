using OrgPortal.Common;

namespace OrgPortal.ViewModels
{
    public class PageViewModelBase : ViewModelBase
    {
        protected PageViewModelBase(INavigation navigation, INavigationBar navBar)
            : base(navigation)
        {
            _navigationBar = navBar;
        }

        private readonly INavigationBar _navigationBar;
        public INavigationBar NavigationBar
        {
            get { return _navigationBar; }
        }
    }
}
