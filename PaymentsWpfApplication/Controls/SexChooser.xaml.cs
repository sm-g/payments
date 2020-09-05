using System.Windows;
using System.Windows.Controls;

namespace PaymentsWpfApplication.Controls
{
    /// <summary>
    /// Interaction logic for SexChooser.xaml
    /// </summary>
    public partial class SexChooser : UserControl
    {
        public SexChooser()
        {
            InitializeComponent();
        }

        public int Sex
        {
            get { return (int)GetValue(SexProperty); }
            set
            {
                SetValue(SexProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Sex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SexProperty =
            DependencyProperty.Register("Sex", typeof(int), typeof(SexChooser), new PropertyMetadata(1));
    }
}