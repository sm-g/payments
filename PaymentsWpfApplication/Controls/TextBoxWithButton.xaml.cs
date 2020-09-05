using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PaymentsWpfApplication.Controls
{
    /// <summary>
    /// Interaction logic for TextBoxWithButton.xaml
    /// </summary>
    public partial class TextBoxWithButton : UserControl
    {
        public TextBoxWithButton()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextBoxWithButton));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(TextBoxWithButton));

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(TextBoxWithButton));

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Command.Execute(CommandParameter);
        }
    }
}