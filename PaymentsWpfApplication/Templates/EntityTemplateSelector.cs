using Payments.Data;
using System.Windows;
using System.Windows.Controls;
using Payments.Model;
using System;
using System.Collections.Generic;

namespace PaymentsWpfApplication.Templates
{
    public class EntityTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SettlementTemplate { get; set; }
        public DataTemplate StreetTemplate { get; set; }
        public DataTemplate HouseTemplate { get; set; }
        public DataTemplate FlatTemplate { get; set; }
        public DataTemplate StreetTypeTemplate { get; set; }
        public DataTemplate FlatTypeTemplate { get; set; }
        public DataTemplate MeterTemplate { get; set; }
        public DataTemplate MeterTypeTemplate { get; set; }
        public DataTemplate ReadingTemplate { get; set; }
        public DataTemplate PaymentTemplate { get; set; }
        public DataTemplate ProviderTemplate { get; set; }
        public DataTemplate RateTemplate { get; set; }
        public DataTemplate BenefitTemplate { get; set; }
        public DataTemplate ServiceTemplate { get; set; }
        public DataTemplate ServiceTypeTemplate { get; set; }
        public DataTemplate UnitTemplate { get; set; }
        public DataTemplate RegistrationTemplate { get; set; }
        public DataTemplate PayerTemplate { get; set; }
        public DataTemplate UserTemplate { get; set; }
        public DataTemplate UserGroupTemplate { get; set; }

        public DataTemplate NullEntityTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
            {
                return NullEntityTemplate;
            }

            switch (EntityMapper.EntityOf(item))
            {
                case Entity.Settlement: return SettlementTemplate;
                case Entity.Street: return StreetTemplate;
                case Entity.House: return HouseTemplate;
                case Entity.Flat: return FlatTemplate;
                case Entity.StreetType: return StreetTypeTemplate;
                case Entity.FlatType: return FlatTypeTemplate;
                case Entity.Meter: return MeterTemplate;
                case Entity.MeterType: return MeterTypeTemplate;
                case Entity.Reading: return ReadingTemplate;
                case Entity.Payment: return PaymentTemplate;
                case Entity.Rate: return RateTemplate;
                case Entity.Benefit: return BenefitTemplate;
                case Entity.Service: return ServiceTemplate;
                case Entity.ServiceType: return ServiceTypeTemplate;
                case Entity.Unit: return UnitTemplate;
                case Entity.Registration: return RegistrationTemplate;
                case Entity.Payer: return PayerTemplate;
                case Entity.User: return UserTemplate;
                case Entity.UserGroup: return UserGroupTemplate;
            }

            throw new ArgumentOutOfRangeException("item");

        }


    }
}