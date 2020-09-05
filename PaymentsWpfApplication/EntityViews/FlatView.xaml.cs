using System.Windows;
using System.Windows.Controls;

namespace PaymentsWpfApplication.EntityViews
{
    /// <summary>
    /// Interaction logic for FlatView.xaml
    /// </summary>
    public partial class FlatView : UserControl
    {
        public FlatView()
        {
            InitializeComponent();
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = (PaymentsWpfApplication.ViewModels.DataPresenterViewModel)root.DataContext;
            if (vm.IsListedChilds)
            {
                settlementColumn.Visibility = System.Windows.Visibility.Hidden;
                streetColumn.Visibility = System.Windows.Visibility.Hidden;
                houseColumn.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}