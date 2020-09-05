using PaymentsWpfApplication.ViewModels;
using System;
using System.Windows;

namespace PaymentsWpfApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new MainWindowViewModel(frame.NavigationService);
            frame.NavigationService.Navigate(new Uri("pack://application:,,,/Screens/LoginScreen.xaml"));
        }
    }
}