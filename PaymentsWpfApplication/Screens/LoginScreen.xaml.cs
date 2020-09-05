using PaymentsWpfApplication.ViewModels;
using System.Windows;

namespace PaymentsWpfApplication.Screens
{
    public partial class LoginScreen
    {
        private LoginViewModel vm;

        public LoginScreen()
        {
            this.InitializeComponent();
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            vm.Password = password.SecurePassword;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = vm ?? (vm = new LoginViewModel());
        }
    }
}