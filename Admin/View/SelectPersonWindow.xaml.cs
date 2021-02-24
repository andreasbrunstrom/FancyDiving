using System.Collections;
using System.Windows;

namespace AdminApp.View
{
    /// <summary>
    /// Interaction logic for SelectPersonWindow.xaml
    /// </summary>
    public partial class SelectPersonWindow
    {
        public SelectPersonWindow(IEnumerable itemSourse)
        {
            InitializeComponent();
            listBox.ItemsSource = itemSourse;
        }

        private void btn_addSelectedJudge_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = listBox.SelectedItem != null;
        }

        private void Btn_cancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
