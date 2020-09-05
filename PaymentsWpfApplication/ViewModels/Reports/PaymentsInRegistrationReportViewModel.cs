using Payments.Model;
using System;
using System.Collections.Generic;

namespace PaymentsWpfApplication.ViewModels
{
    public class PaymentsInRegistrationReportViewModel : DataPresenterViewModel
    {
        private DateTime _from;
        private DateTime _to;

        public PaymentsInRegistrationReportViewModel(DateTime from, DateTime to)
            : base(Entity.Registration,
            null,
            Entity.Payment)
        {
            _from = from;
            _to = to;
        }

        protected override IEnumerable<object> MakeItemsCollection()
        {
            return ec.Get.FilteredPaymentsInRegistration(_from, _to);
        }
    }
}