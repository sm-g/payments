using Payments.Model;
using System;
using System.Collections.Generic;

namespace PaymentsWpfApplication.ViewModels
{
    public class PaymentsOfServiceReportViewModel : DataPresenterViewModel
    {
        private DateTime _from;
        private DateTime _to;

        public PaymentsOfServiceReportViewModel(DateTime from, DateTime to)
            : base(Entity.Service,
            null,
            Entity.Payment)
        {
            _from = from;
            _to = to;
        }

        protected override IEnumerable<object> MakeItemsCollection()
        {
            return ec.Get.FilteredPaymentsInService(_from, _to);
        }
    }
}