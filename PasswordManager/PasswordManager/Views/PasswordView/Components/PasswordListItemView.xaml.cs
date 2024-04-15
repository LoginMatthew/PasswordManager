using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PasswordManager.Views.PasswordView.Components
{
    /// <summary>
    /// Interaction logic for PasswordListItem.xaml
    /// </summary>
    public partial class PasswordListItemView : UserControl
    {
        public PasswordListItemView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DropDownItem.IsOpen = false;
        }

        private void Site_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(Site.Text);
        }

        private void UserName_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(UserName.Text);
        }

        private void Email_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(Email.Text);
        }
    }
}
