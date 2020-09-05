using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PaymentsWpfApplication.ViewModels;
using PaymentsWpfApplication.Reports;

namespace PaymentsWpfApplication.Reports
{
    class Printer
    {
        public static void PrintDataViewer(DataPresenterViewModel vm, DataGrid grid)
        {
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == false)
                return;

            string documentTitle = vm.ListedEntityTitle;
            Size pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);

            CustomDataGridDocumentPaginator paginator = new CustomDataGridDocumentPaginator(grid, documentTitle, pageSize, new Thickness(30, 20, 30, 20));
            printDialog.PrintDocument(paginator, "Grid");
        }
    }
}
