using Payments.Model;
using System.Windows;
using System.Windows.Controls;

namespace PaymentsWpfApplication.EntityViews
{
    /// <summary>
    /// Interaction logic for UserEditor.xaml
    /// </summary>
    public partial class UserEditor : UserControl
    {
        public UserEditor()
        {
            InitializeComponent();
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UserController.SetNewPassword(password.SecurePassword, (Payments.Data.User)this.DataContext);
        }
    }
}