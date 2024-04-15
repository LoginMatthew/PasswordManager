
using System.Windows;
using System.Windows.Controls;

namespace PasswordManager.CustomControls
{
    /// <summary>
    /// Interaction logic for MinimizeWindowButtonType.xaml
    /// </summary>
    public partial class MinimizeWindowButtonType : UserControl
    {
        public MinimizeWindowButtonType()
        {
            InitializeComponent();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.WindowState = WindowState.Minimized;
        }
    }
}
