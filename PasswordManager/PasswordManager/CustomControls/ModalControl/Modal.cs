using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PasswordManager.CustomControls.ModalControl
{
    public class Modal : ContentControl
    {
        //Custom Control
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(Modal),
                new PropertyMetadata(false));

        public static readonly DependencyProperty ActualWindowHeightProperty =
            DependencyProperty.Register("ActualWindowHeight", typeof(int), typeof(Modal),
                new PropertyMetadata(0));

        public static readonly DependencyProperty ActualWindowWidthProperty =
            DependencyProperty.Register("ActualWindowWidth", typeof(int), typeof(Modal),
                new PropertyMetadata(0));

        public int ActualWindowHeight
        {
            get { return (int)GetValue(ActualWindowHeightProperty); }
            set { SetValue(ActualWindowHeightProperty, value); }
        }

        public int ActualWindowWidth
        {
            get { return (int)GetValue(ActualWindowWidthProperty); }
            set { SetValue(ActualWindowWidthProperty, value); }
        }

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        static Modal()
        {
            //this is the code for the modal
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Modal), new FrameworkPropertyMetadata(typeof(Modal)));

        }

        private static object CreateDefaultBackground()
        {
            return new SolidColorBrush(Colors.Gray)
            {
                Opacity = 0.3
            };
        }
    }
}