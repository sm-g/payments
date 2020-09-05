using Payments.Model;
using System;
using System.Collections.Generic;

namespace PaymentsWpfApplication.ViewModels
{
    public class ReadingsInRegistrationReportViewModel : DataPresenterViewModel
    {
        private DateTime _from;
        private DateTime _to;

        public ReadingsInRegistrationReportViewModel(DateTime from, DateTime to)
            : base(Entity.Meter,
            null,
            Entity.Reading)
        {
            _from = from;
            _to = to;
        }

        protected override IEnumerable<object> MakeItemsCollection()
        {
            return ec.Get.FilteredReadingsInRegistration(_from, _to);
        }
    }
}