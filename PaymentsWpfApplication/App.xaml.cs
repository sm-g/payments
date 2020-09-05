using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Globalization;


namespace PaymentsWpfApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // set wpf culture
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }
    }
}
