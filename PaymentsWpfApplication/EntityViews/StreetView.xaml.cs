using PaymentsWpfApplication.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PaymentsWpfApplication.EntityViews
{
    /// <summary>
    /// Interaction logic for StreetView.xaml
    /// </summary>
    public partial class StreetView : UserControl
    {
        public StreetView()
        {
            InitializeComponent();
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = (DataPresenterViewModel)root.DataContext;
            if (vm.IsListedChilds)
                settlementColumn.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}