using System.Windows;
using System.Windows.Controls;

namespace PaymentsWpfApplication.EntityViews
{
    /// <summary>
    /// Interaction logic for RegistrationView.xaml
    /// </summary>
    public partial class RegistrationView : UserControl
    {
        public RegistrationView()
        {
            InitializeComponent();
        }
        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = (PaymentsWpfApplication.ViewModels.DataPresenterViewModel)root.DataContext;
            if (vm.IsListedChilds)
            {
                if (vm.Entity == Payments.Model.Entity.Payer)
                {
                    lastNameColumn.Visibility = System.Windows.Visibility.Hidden;
                    firstNameColumn.Visibility = System.Windows.Visibility.Hidden;
                    middleNameColumn.Visibility = System.Windows.Visibility.Hidden;
                }
                if (vm.Entity == Payments.Model.Entity.Flat)
                {
                    settlementColumn.Visibility = System.Windows.Visibility.Hidden;
                    streetColumn.Visibility = System.Windows.Visibility.Hidden;
                    houseColumn.Visibility = System.Windows.Visibility.Hidden;
                    flatColumn.Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }
    }
}