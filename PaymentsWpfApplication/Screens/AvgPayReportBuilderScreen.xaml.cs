using PaymentsWpfApplication.Reports;
using PaymentsWpfApplication.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PaymentsWpfApplication.Screens
{
    /// <summary>
    /// Interaction logic for ReportBuilderScreen.xaml
    /// </summary>
    public partial class AvgPayReportBuilderScreen : Page
    {
        public AvgPayReportBuilderScreen()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var grid = ChildFinders.FindChild<DataGrid>(this, "grid");
            var vm = (this.DataContext as AvgPayReportBuilderViewModel).Reports[0];

            Printer.PrintDataViewer(vm, grid);
        }
    }
}