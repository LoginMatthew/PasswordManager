using System.Windows;
using System.Windows.Controls;

namespace PasswordManager.CustomControls
{
    /// <summary>
    /// Interaction logic for ExitWindowButtonType.xaml
    /// </summary>
    public partial class ExitWindowButtonType : UserControl
    {
        public ExitWindowButtonType()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
