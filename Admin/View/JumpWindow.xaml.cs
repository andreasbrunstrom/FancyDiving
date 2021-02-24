using System;
using System.Globalization;
using System.Windows;

namespace AdminApp.View
{
    /// <summary>
    /// Interaction logic for JumpWindow.xaml
    /// </summary>
    public partial class JumpWindow
    {
        public double height { get; set; }
        public string jumpcode { get; set; }

        public JumpWindow()
        {
            InitializeComponent();
            cmb_jumpcode.ItemsSource = Admin.allJumpCodes;
        }

        private void btn_saveJump_OnClick(object sender, RoutedEventArgs e)
        {
            height = Convert.ToDouble(cmb_height.Text);
            jumpcode = cmb_jumpcode.Text;
            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmb_height.Text = height.ToString(CultureInfo.InvariantCulture);
            cmb_jumpcode.Text = jumpcode;
        }
    }
}
