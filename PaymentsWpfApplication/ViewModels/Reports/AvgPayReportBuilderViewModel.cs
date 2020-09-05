using EventAggregator;
using Payments.Data;
using Payments.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace PaymentsWpfApplication.ViewModels
{
    public class AvgPayReportBuilderViewModel : AbstractReportBuilderViewModel
    {
        private double _avg;
        private DateTime _from;
        private Service _service;
        private DateTime _to;
        private PickUperViewModel pickup;

        public AvgPayReportBuilderViewModel()
        {
            pickup = new PickUperViewModel(OnEntityPicked);

            FromDate = DateTime.Today.AddMonths(-6);
            ToDate = DateTime.Today;
        }

        public double Avg
        {
            get { return _avg; }
            set
            {
                if (_avg != value)
                {
                    _avg = value;
                    OnPropertyChanged(() => Avg);
                }
            }
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

        public Service Service
        {
            get { return _service; }
            set
            {
                if (_service != value)
                {
                    _service = value;
                    OnPropertyChanged(() => Service);
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
            return FromDate <= ToDate && Service != null;
        }

        protected override void DoRequest()
        {
            ec.Current.Set(Service);
            var report = new PaymentsOfServiceReportViewModel(FromDate, ToDate);
            Reports.Clear();
            Reports.Add(report);
            Avg = (double)report.Items.Cast<Payment>().Average(p => p.Amount);
        }
        protected override void Clear()
        {
            Service = null;
        }

        private void OnEntityPicked(EventMessage e)
        {
            var pickedEntity = e.GetValue<object>(Messenger.EntityKey);
            if (EntityMapper.EntityOf(pickedEntity) == Entity.Service)
            {
                Service = (Service)pickedEntity;
            }
        }
    }
}