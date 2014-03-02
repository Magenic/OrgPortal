using Caliburn.Micro;
using OrgPortal.ViewModels;

namespace OrgPortal.Common
{
    public interface INavigation
    {
        void Initialize(INavigationService navigationService);
        void NavigateToViewModel<TViewModel>();
        void NavigateToViewModel<TViewModel>(object parameter)
            where TViewModel : ViewModelBase;

        void NavigateToViewModelAndRemoveCurrent<TViewModel>();
        void NavigateToViewModelAndRemoveCurrent<TViewModel>(object parameter)
            where TViewModel : ViewModelBase;

        void GoBack();

        bool CanGoBack { get; }
    }
}