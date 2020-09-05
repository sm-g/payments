using EventAggregator;
using Payments.Data;
using Payments.Model;
using System;
using System.Linq;
using System.Windows.Input;

namespace PaymentsWpfApplication.ViewModels
{
    public class PayNReadReportBuilderViewModel : AbstractReportBuilderViewModel
    {
        private DateTime _from;
        private Registration _registration;
        private double _sum;
        private DateTime _to;
        private PickUperViewModel pickup;

        public PayNReadReportBuilderViewModel()
        {
            pickup = new PickUperViewModel(OnEntityPicked);

            FromDate = DateTime.Today.AddMonths(-6);
            ToDate = DateTime.Today;
        }

        public ICommand BeginPickup
        {
            get
            {
                return pickup.BeginPickup;
            }
        }

        public DateTime FromDate
        {
            get { return _from; }
            set
            {
                if (_from != value)
                {
                    _from = value;
                    OnPropertyChanged(() => FromDate);
                }
            }
        }

        public Registration Registration
        {
            get { return _registration; }
            set
            {
                if (_registration != value)
                {
                    _registration = value;
                    OnPropertyChanged(() => Registration);
                }
            }
        }

        public double Sum
        {
            get { return _sum; }
            set
            {
                if (_sum != value)
                {
                    _sum = value;
                    OnPropertyChanged(() => Sum);
                }
            }
        }

        public DateTime ToDate
        {
            get { return _to; }
            set
            {
                if (_to != value)
                {
                    _to = value;
                    OnPropertyChanged(() => ToDate);
                }
            }
        }

        protected override bool CanRequest()
        {
            return FromDate <= ToDate && Registration != null;
        }

        protected override void DoRequest()
        {
            ec.Current.Set(Registration);

            var paymentsReport = new PaymentsInRegistrationReportViewModel(FromDate, ToDate);
            var readingsReport = new ReadingsInRegistrationReportViewModel(FromDate, ToDate);
            Reports.Clear();
            Reports.Add(paymentsReport);
            Reports.Add(readingsReport);
            Sum = (double)paymentsReport.Items.Cast<Payment>().Sum(p => p.Amount);
        }

        protected override void Clear()
        {
            Registration = null;
        }

        private void OnEntityPicked(EventMessage e)
        {
            var pickedEntity = e.GetValue<object>(Messenger.EntityKey);
            if (EntityMapper.EntityOf(pickedEntity) == Entity.Registration)
            {
                Registration = (Registration)pickedEntity;
            }
        }
    }
}