using PaymentsWpfApplication.Reports;
using PaymentsWpfApplication.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PaymentsWpfApplication.Screens
{
    /// <summary>
    /// Interaction logic for DataViewerScreen.xaml
    /// </summary>
    public partial class DataViewerScreen : Page
    {
        public DataViewerScreen()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var grid = ChildFinders.FindChild<DataGrid>(this, "grid");
            var vm = this.DataContext as DataPresenterViewModel;

            Printer.PrintDataViewer(vm, grid);
        }
    }
}