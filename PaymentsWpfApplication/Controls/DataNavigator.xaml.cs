using System.Windows.Controls;
using System.Windows.Input;

namespace PaymentsWpfApplication.Controls
{
    /// <summary>
    /// Interaction logic for DataNavigator.xaml
    /// </summary>
    public partial class DataNavigator : UserControl
    {
        public DataNavigator()
        {
            this.InitializeComponent();
        }

        private void current_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                current.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }
    }
}