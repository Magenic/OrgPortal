using Caliburn.Micro;
using OrgPortal.ViewModels;
using System.Composition;

namespace OrgPortal.Common
{
    [Export(typeof(INavigation))]
    [Shared]
    public sealed class Navigation : INavigation
    {
        private INavigationService navigationService;

        public Navigation()
        { }

		public Navigation(INavigationService navigationService)
		{
			this.navigationService = navigationService;
		}

        public void Initialize(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

		public void NavigateToViewModel<TViewModel>()
		{
			this.navigationService.NavigateToViewModel<TViewModel>();
		}

		public void NavigateToViewModel<TViewModel>(object parameter)
			where TViewModel : ViewModelBase
		{
			var serializedParameter = Serializer.Serialize(parameter);
			this.navigationService.NavigateToViewModel<TViewModel>(serializedParameter);
		}

		public void NavigateToViewModelAndRemoveCurrent<TViewModel>()
		{
			// Clear this page from the navigation stack.
			this.navigationService.GoBack();

			this.navigationService.NavigateToViewModel<TViewModel>();
		}

		public void NavigateToViewModelAndRemoveCurrent<TViewModel>(object parameter) where TViewModel : ViewModelBase
		{
			var serializedParameter = Serializer.Serialize(parameter);

			// Clear this page from the navigation stack.
			this.navigationService.GoBack();

			this.navigationService.NavigateToViewModel<TViewModel>(serializedParameter);
		}

		public void GoBack()
		{
			this.navigationService.GoBack();
		}

		public bool CanGoBack
		{
			get { return this.navigationService.CanGoBack; }
		}
    }
}