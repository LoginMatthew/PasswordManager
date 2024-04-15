using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace PasswordManager.CustomControls.DropdownMenuControl
{
    [TemplatePart(Name = PART_POPUP_NAME, Type = typeof(Popup))]
    [TemplatePart(Name = PART_TOGGLE_NAME, Type = typeof(CheckBox))]
    public class DropdownMenu : ContentControl
    {
        private const string PART_POPUP_NAME = "PART_Popup";
        private const string PART_TOGGLE_NAME = "PART_Toggle";

        private Popup _popup;
        private CheckBox _toggle;

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(DropdownMenu),
                new PropertyMetadata(false));

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        static DropdownMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropdownMenu), new FrameworkPropertyMetadata(typeof(DropdownMenu)));
        }

        public override void OnApplyTemplate()
        {
            _popup = Template.FindName(PART_POPUP_NAME, this) as Popup;
            if (_popup != null)
            {
                _popup.Closed += Popup_Closed;
            }

            _toggle = Template.FindName(PART_TOGGLE_NAME, this) as CheckBox;

            base.OnApplyTemplate();
        }

        private void Popup_Closed(object sender, EventArgs e)
        {
            if (!_toggle.IsMouseOver)
            {
                IsOpen = false;
            }
        }
    }
}