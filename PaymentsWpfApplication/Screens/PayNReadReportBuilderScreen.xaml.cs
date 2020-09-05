using PaymentsWpfApplication.Reports;
using PaymentsWpfApplication.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PaymentsWpfApplication.Screens
{
    /// <summary>
    /// Interaction logic for ReportBuilderScreen.xaml
    /// </summary>
    public partial class PayNReadReportBuilderScreen : Page
    {
        public PayNReadReportBuilderScreen()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var grid = ChildFinders.FindChild<DataGrid>(this, "grid");
            var vm = (this.DataContext as PayNReadReportBuilderViewModel).Reports[0]; // first report from two

            Printer.PrintDataViewer(vm, grid);
        }
    }
}