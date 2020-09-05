using System.Windows;
using System.Windows.Controls;

namespace PaymentsWpfApplication.EntityViews
{
    /// <summary>
    /// Interaction logic for ReadingView.xaml
    /// </summary>
    public partial class ReadingView : UserControl
    {
        public ReadingView()
        {
            InitializeComponent();
        }
        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = (PaymentsWpfApplication.ViewModels.DataPresenterViewModel)root.DataContext;
            if (vm.IsListedChilds)
            {
                regColumn.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}