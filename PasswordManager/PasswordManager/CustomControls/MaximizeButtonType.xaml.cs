
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace PasswordManager.CustomControls
{
    /// <summary>
    /// Interaction logic for MaximizeButtonType.xaml
    /// </summary>
    public partial class MaximizeButtonType : UserControl
    {
        public MaximizeButtonType()
        {
            InitializeComponent();
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);

            if (window.WindowState == WindowState.Normal)
                window.WindowState = WindowState.Maximized;
            else window.WindowState = WindowState.Normal;
        }
    }
}
