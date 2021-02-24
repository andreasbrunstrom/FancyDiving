using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AdminApp.Presenter;

namespace AdminApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var mainwindow = new AdminMainWindow();
            var ad = new Admin();
            var presente = new PresenterAdminMainWindow(mainwindow, ad);
            mainwindow.Show();
        }
    }
}
