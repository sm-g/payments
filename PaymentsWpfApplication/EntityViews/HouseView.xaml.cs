using System.Windows;
using System.Windows.Controls;

namespace PaymentsWpfApplication.EntityViews
{
    /// <summary>
    /// Interaction logic for HouseView.xaml
    /// </summary>
    public partial class HouseView : UserControl
    {
        public HouseView()
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
            }
        }
    }
}