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
    /// Interaction logic for EditContestantWindow.xaml
    /// </summary>
    public partial class EditContestantWindow : Window
    {
        public string name { get; set; }
        public string club { get; set; }
        public string nationality { get; set; }
        public bool gender { get; set; } = true;
        public DateTime birthdate { get; set; } = DateTime.Today;

        public EditContestantWindow()
        {
            InitializeComponent();
        }

        private void btn_saveContestant_Click(object sender, RoutedEventArgs e)
        {
            name = tb_name.Text;
            club = tb_club.Text;
            nationality = cmb_country.Text;
            gender = (rb_male.IsChecked == true);
            if (dp_birthdate.SelectedDate != null) birthdate = dp_birthdate.SelectedDate.Value;
            DialogResult = true;
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tb_name.Text = name;
            tb_club.Text = club;
            cmb_country.Text = nationality;
            rb_male.IsChecked = gender;
            rb_female.IsChecked = !gender;
            dp_birthdate.SelectedDate = birthdate;
        }
    }
}
