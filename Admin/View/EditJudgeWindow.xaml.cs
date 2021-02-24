using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AdminApp.View
{
    /// <summary>
    /// Interaction logic for EditJudgeWindow.xaml
    /// </summary>
    public partial class EditJudgeWindow : Window
    {
        public string name { get; set; }
        public string nationality { get; set; }

        public EditJudgeWindow()
        {
            InitializeComponent();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btn_saveJudge_Click(object sender, RoutedEventArgs e)
        {
            name = tb_name.Text;
            nationality = cmb_country.Text;
            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tb_name.Text = name;
            cmb_country.Text = nationality;
        }
    }
}
