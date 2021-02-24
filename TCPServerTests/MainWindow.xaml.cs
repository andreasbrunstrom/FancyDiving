using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
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
using SharedClasses;

namespace TCPServerTests
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegate void UpdateStatusCallback(string strMessage);

        private Server mainServer;
        private SendReceive sendReceive;

        public MainWindow()
        {
            InitializeComponent();
        }

        public static void AppendText(RichTextBox box, string text, string color)
        {
            BrushConverter bc = new BrushConverter();
            TextRange tr = new TextRange(box.Document.ContentEnd, box.Document.ContentEnd);
            tr.Text = text;
            try
            {
                tr.ApplyPropertyValue(TextElement.ForegroundProperty,
                    bc.ConvertFromString(color));
            }
            catch (FormatException) { }
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            if (btn_start.Content.ToString() == "Starta server")
            {
                IPAddress ipAddr = IPAddress.Any;
                //IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
                // Create a new instance of the ChatServer object
                mainServer = new Server(ipAddr);
                // Hook the StatusChanged event handler to mainServer_StatusChanged
                Server.StatusChanged += new StatusChangedEventHandler(mainServer_StatusChanged);
                // Start listening for connections
                mainServer.StartListening();
                btn_start.Content = "Stoppa server";
                UpdateStatus("Servern startad.");
            }
            else
            {
                mainServer.RequestStop();
                btn_start.Content = "Starta server";
                UpdateStatus("Servern stoppad.");
            }
           
        }

        public void mainServer_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            // Call the method that updates the form
            var callback = new UpdateStatusCallback(this.UpdateStatus);
            callback(e.EventMessage);

            //this.Invoke(new UpdateStatusCallback(this.UpdateStatus), new object[] { e.EventMessage });
        }

        private void UpdateStatus(string strMessage)
        {
            // Updates the log with the message
            Application.Current.Dispatcher.Invoke(delegate
            {
                tb_log.AppendText(strMessage + "\r\n-----------------------------------\r\n");
                tb_log.ScrollToEnd();
            });
            
        }

        private void btn_sendJump_Click(object sender, RoutedEventArgs e)
        {
            var jump = new Jump(10, 7,false,"hoppkod");
            mainServer.sendData("jump",jump);

            UpdateStatus("hopp skickat.");
        }

        private void btn_sendJudgeList_Click(object sender, RoutedEventArgs e)
        {
            var judgeList = new BindingList<Judge>();
            judgeList.Add(new Judge(1,5,"Anton", "Sverige"));
            judgeList.Add(new Judge(2, 4, "Johan", "Sverige"));
            mainServer.sendData("judgelist", judgeList);

            UpdateStatus("dommarlista skickad");
        }

        private void btn_sendText_Click(object sender, RoutedEventArgs e)
        {
            mainServer.sendData(tb_cmd.Text,tb_data.Text);
        }

        private void btn_sendOk_Click(object sender, RoutedEventArgs e)
        {
            mainServer.sendData("ok", "1");
            UpdateStatus("ok skickat");
        }

        private void btn_sendFail_Click(object sender, RoutedEventArgs e)
        {
            mainServer.sendData("failed", "1");
            UpdateStatus("failed skickat");
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            mainServer.RequestStop();
        }
    }
}
