using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using JudgeApp.Model;
using JudgeApp.View;
using SharedClasses;
using log4net;

namespace JudgeApp
{
    /// <summary>
    /// Interaction logic for JudgeMainWindow.xaml
    /// </summary>
    public partial class JudgeMainWindow : IJudgeMainWindow
    {
        /// <summary>
        /// This is the GUI for the JudgeApp
        /// </summary>
        #region IJudgeMainWindow members
        
        public event DelegateSendScore eventSendScore;
        public event DelegateJudgeExit eventJudgeExit;
        public event DelegateSelectJudge eventSelectJudge;
        public event DelegateConnectToAdmin eventConnectToAdmin;
        public event DelegateJudgeSelectionChanged eventJudgeSelectionChanged;

        #endregion

        public JudgeMainWindow()
        {
            JudgeModel.judgeLoggingInit();
            InitializeComponent();
            tb_adminIP.Text = Properties.Settings.Default.ip;

            foreach (var tab in tabControl.Items)
            {
                (tab as TabItem).Visibility = Visibility.Collapsed;
            }
        }
        
        #region Components
        void IJudgeMainWindow.setJudgeList(BindingList<Judge> judgeList)
        {
            cmb_selectJudge.ItemsSource = judgeList;
        }

        #region goto tab
        public void goToInit()
        {
            tabControl.SelectedIndex = 0;
        }

        public void goToSelect()
        {
            tabControl.SelectedIndex = 1;
        }

        public void goToScore()
        {
            tabControl.SelectedIndex = 2;
        }

        public void goToWait()
        {
            tabControl.SelectedIndex = 3;
        }

        public void goToExit()
        {
            tabControl.SelectedIndex = 4;
        }
        #endregion

        #region buttons
        private void btn_scoring_ok_Click(object sender, RoutedEventArgs e)
        {
            eventSendScore?.Invoke(tb_score.Text);
        }

        private void btn_connectToAdmin_Click(object sender, RoutedEventArgs e)
        {
            eventConnectToAdmin?.Invoke(tb_adminIP.Text);
        }

        private void btn_selectJudge_Click(object sender, RoutedEventArgs e)
        {
            eventSelectJudge?.Invoke();
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region Points buttons
        private void btn_wholeUp_Click(object sender, RoutedEventArgs e)
        {
            var d = double.Parse(tb_score.Text);

            if (d <= 9.0)
            {
                d += 1.0;
                tb_score.Text = d.ToString("F1");
            }
            if (d == 9.5)
            {
                d = 10.0;
                tb_score.Text = d.ToString("F1");
            }
        }

        private void btn_halfUp_Click(object sender, RoutedEventArgs e)
        {
            var d = double.Parse(tb_score.Text);

            if (d <= 9.5)
            {
                d += 0.5;
                tb_score.Text = d.ToString("F1");
            }
        }

        private void btn_wholeDown_Click(object sender, RoutedEventArgs e)
        {
            var d = double.Parse(tb_score.Text);

            if (d >= 1.0)
            {
                d -= 1.0;
                tb_score.Text = d.ToString("F1");
            }
            if (d == 0.5)
            {
                d = 0.0;
                tb_score.Text = d.ToString("F1");
            }
        }

        private void btn_halfDown_Click(object sender, RoutedEventArgs e)
        {
            var d = double.Parse(tb_score.Text);

            if (d >= 0.5)
            {
                d -= 0.5;
                tb_score.Text = d.ToString("F1");
            }
        }
        #endregion

        private void cmb_selectJudge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            eventJudgeSelectionChanged?.Invoke(cmb_selectJudge.SelectedIndex);
        }
        #endregion

        public void showMessageBox(string message)
        {
            MessageBox.Show(message);
        }
        
        private void tb_adminIP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            eventConnectToAdmin?.Invoke(tb_adminIP.Text);
        }

        private void tb_adminIP_TextChanged(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.ip = tb_adminIP.Text;
            Properties.Settings.Default.Save();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            eventJudgeExit?.Invoke();
        }
    }
}
