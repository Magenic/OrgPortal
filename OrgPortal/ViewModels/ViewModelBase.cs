using Caliburn.Micro;
using OrgPortal.Common;

namespace OrgPortal.ViewModels
{
    public class ViewModelBase : Screen
    {
        protected ViewModelBase(INavigation navigation)
        {
            this.navigation = navigation;
        }

		public void GoBack()
		{            
			this.Navigation.GoBack();
		}

		protected virtual void DeserializeParameter(string value)
		{
		}

        private readonly INavigation navigation;
        protected INavigation Navigation
		{
			get { return navigation; }
		}

		public bool CanGoBack
		{
			get { return navigation.CanGoBack; }
		}

		private bool isBusy;
		public bool IsBusy
		{
			get { return this.isBusy; }
			set
			{
				this.isBusy = value;
				NotifyOfPropertyChange(() => this.IsBusy);
			}
		}

		private string parameter;
		public string Parameter
		{
			get { return this.parameter; }
			set
			{
				this.parameter = value;
				NotifyOfPropertyChange(() => this.Parameter);

				if (value != null)
				{
					this.DeserializeParameter(value);
				}
			}
		}
    }
}
