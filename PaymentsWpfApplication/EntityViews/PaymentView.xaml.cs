using System.Windows;
using System.Windows.Controls;

namespace PaymentsWpfApplication.EntityViews
{
    /// <summary>
    /// Interaction logic for PaymentView.xaml
    /// </summary>
    public partial class PaymentView : UserControl
    {
        public PaymentView()
        {
            InitializeComponent();
        }
        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = (PaymentsWpfApplication.ViewModels.DataPresenterViewModel)root.DataContext;
            if (vm.IsListedChilds)
            {
                if (vm.Entity == Payments.Model.Entity.Registration)
                {
                    lastNameColumn.Visibility = System.Windows.Visibility.Hidden;
                    firstNameColumn.Visibility = System.Windows.Visibility.Hidden;
                    middleNameColumn.Visibility = System.Windows.Visibility.Hidden;
                    settlementColumn.Visibility = System.Windows.Visibility.Hidden;
                    streetColumn.Visibility = System.Windows.Visibility.Hidden;
                    houseColumn.Visibility = System.Windows.Visibility.Hidden;
                    flatColumn.Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }
    }
}