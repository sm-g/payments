using EventAggregator;
using Payments.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PaymentsWpfApplication.ViewModels
{
    public abstract class AbstractReportBuilderViewModel : ViewModelBase
    {
        private ICommand _requset;
        protected EntitiesController ec = EntitiesController.Instance;

        public ObservableCollection<DataPresenterViewModel> Reports
        {
            get;
            private set;
        }

        public ICommand Request
        {
            get
            {
                return _requset ?? (_requset = new RelayCommand(DoRequest,
                () => CanRequest()));
            }
        }

        protected abstract void DoRequest();

        protected abstract bool CanRequest();

        protected abstract void Clear();

        public AbstractReportBuilderViewModel()
        {
            Reports = new ObservableCollection<DataPresenterViewModel>();
            this.Subscribe((int)EventId.UserLoggedOut, (e) =>
            {
                Reports.Clear();
                Clear();
            });
        }
    }
}