using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
using System.Xml.Serialization;
using SharedClasses;

namespace JudgeTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int port { get; set; }
        public string serverAddress { get; set; }
        public TcpClient tcpClient { get; set; }
        public StreamReader clientStreamReader { get; set; }
        public StreamWriter clientStreamWriter { get; set; }
        public NetworkStream clientStream { get; set; }
        private bool connected = false;
        private Thread recieveThread;
        public BindingList<Judge> judgeList = new BindingList<Judge>();

        private delegate void UpdateCallback(string str);

        public void connect(string ipAdress)
        {
            if (connected) return;
            try
            {
                tcpClient = new TcpClient(ipAdress, 9058);
                clientStream = tcpClient.GetStream();
                clientStreamReader = new StreamReader(clientStream); //flytta?
                clientStreamWriter = new StreamWriter(clientStream);
                connected = true;
                recieveThread = new Thread(new ThreadStart(receiveData));
                recieveThread.Start();
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
            }
        }

        private void receiveData()
        {
            var sr = tcpClient.GetStream();
            while (connected)
            {
                try
                {
                    var text = receive(sr);
                    var mainThreadCallback = new UpdateCallback(receiveFromAdmin);
                    mainThreadCallback(text);

                }
                catch (Exception)
                {

                }
            }
        }

        public void receiveFromAdmin(string str)
        {
            var splitmessage = str.Split('@');

            var command = splitmessage[0];
            var data = splitmessage[1];

            switch (command)
            {
                case "judgelist":
                    var judgeList = deserializer<BindingList<Judge>>(data);
                    //comboBox.ItemsSource = judgeList;
                    foreach (var j in judgeList)
                    {
                       // MessageBox.Show(j.name);
                        comboBox.Items.Add(j.name);
                    }
                    break;
                default:
                    break;
            }
        }

        protected string receive(NetworkStream clientStream)
        {
            if (clientStream.CanRead)
            {
                var myReadBuffer = new byte[2048];
                var myCompleteMessage = new StringBuilder();

                do
                {
                var numberOfBytesRead = clientStream.Read(myReadBuffer, 0, myReadBuffer.Length);
                myCompleteMessage.AppendFormat("{0}", Encoding.UTF8.GetString(myReadBuffer, 0, numberOfBytesRead));
                } while (clientStream.DataAvailable);


                return myCompleteMessage.ToString();
            }
            return null;
        }

        protected static string serializer<T>(T dataToSerialize)
        {
            try
            {
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(dataToSerialize.GetType());
                serializer.Serialize(stringwriter, dataToSerialize);
                return stringwriter.ToString();
            }
            catch
            {
                throw;
            }
        }

        protected static T deserializer<T>(string xmlText)
        {
            try
            {
                var stringReader = new StringReader(xmlText);
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
            catch
            {
                throw;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            connect("127.0.0.1");
        }
    }
}
