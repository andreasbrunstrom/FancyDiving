using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using JudgeApp.Model;
using JudgeApp.Presenter;

namespace JudgeApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var judgeMainWindow = new JudgeMainWindow();
            var judgeModel = new JudgeModel();
            var presenter = new PresenterJudgeMainWindow(judgeMainWindow, judgeModel);
            judgeMainWindow.DataContext = judgeModel;
            judgeMainWindow.Show();
        }
    }
}
