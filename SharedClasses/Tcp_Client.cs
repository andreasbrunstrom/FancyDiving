using System;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static System.Console;

namespace SharedClasses
{
    [Serializable]
    public class Tcp_Client : SendReceive
    {
        public int port { get; set; }
        public string serverAddress { get; set; }
        public static TcpClient tcpClient { get; set; }
        public StreamReader clientStreamReader { get; set; }
        public StreamWriter clientStreamWriter { get; set; }
        public NetworkStream clientStream { get; set; }
        public BindingList<Judge> localTCPJudgeList { get; set; } = new BindingList<Judge>();
        public Jump localJump { get; set; }
        private bool connected;

        private delegate void UpdateCallback(string str);

        public bool judgeAccepted;
        public bool incommingJump;
        public bool terminateJudge;

        public bool connect(string ipAdress)
        {
            try
            {
                tcpClient = new TcpClient(ipAdress, 9058);
                clientStream = tcpClient.GetStream();
                clientStreamReader = new StreamReader(clientStream);
                clientStreamWriter = new StreamWriter(clientStream);
                connected = true;
                var recieveThread = new Thread(receiveData);
                recieveThread.Start();
                return true;
            }
            catch (SocketException se)
            {
                WriteLine(se.Message);
                return false;
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

        public void sendData<T>(string command, T data)
        {
            try
            {
                byte[] msg = Encoding.UTF8.GetBytes(command + "@" +serializer(data));
                clientStream.Write(msg, 0, msg.Length);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public void receiveFromAdmin(string str)
        {
            var splitmessage = str.Split('@');

            var command = splitmessage[0];
            var msgRecieved = splitmessage[1];

            try
            {
                if (command == "judgelist")
                {
                    localTCPJudgeList = deserializer<BindingList<Judge>>(msgRecieved);
                }

                if (command == "ok")
                {
                    judgeAccepted = true;
                }

                if(command == "jump")
                {
                    localJump = deserializer<Jump>(msgRecieved);
                    incommingJump = true;
                    //gå in i nytt tillstånd för att skicka jump tillbaka till admin
                    //när vi har skickat det ska vi börja lyssna igen.
                }

                if(command == "failed")
                {
                    //Om failed returneras kan man ej vara den judge man valt. Välj ny judge och testa igen.
                }

                if(command == "exit")
                {
                    terminateJudge = true;
                    //Här ska programmet avslutas, typ.
                }
            }
            catch (Exception)
            {
                throw;
            }           
        }

        public void Close()
        {
            try
            {
                tcpClient.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region gammal kod
        //private static void Main(string[] args)
        //{
        //    Score s = new Score(1, 2, 7.5);

        //    //Connection start
        //    TcpClient tcpClient = null;
        //    try
        //    {
        //        const int port = 23;
        //        tcpClient = new TcpClient("130.243.104.98", port);
        //        //tcpClient = new System.Net.Sockets.TcpClient("127.0.0.1", port);

        //        var clientStreamReader = new StreamReader(tcpClient.GetStream());
        //        var clientStreamWriter = new StreamWriter(tcpClient.GetStream());
        //        NetworkStream clientStream = tcpClient.GetStream();

        //        var str = "";
        //        while (str != null && !str.StartsWith("quit"))
        //        {
        //            str = clientStreamReader.ReadLine();
        //            WriteLine(str);

        //            if (str.StartsWith("go to next"))
        //            {
        //                Console.WriteLine("Go to next jump");
        //                clientStreamWriter.WriteLine("Next jump back to server");
        //                // Go to next here
        //            }

        //            if (str.StartsWith("go to end"))
        //            {
        //                Console.WriteLine("Go to end");
        //                clientStreamWriter.WriteLine("Go to end back to server");
        //                //Go to end here
        //            }

        //            if (str.StartsWith("force quit"))
        //            {
        //                Console.WriteLine("Force quit");
        //                clientStreamWriter.WriteLine("Force quit back to server");
        //                //Force quit here

        //                return;
        //            }

        //            if (str.StartsWith("Request score"))
        //            {
        //                Console.WriteLine("Press enter to send");


        //                IFormatter formatter = new BinaryFormatter();
        //                formatter.Serialize(clientStream, s);
        //            }
        //            var newMessage = ReadLine();
        //            clientStreamWriter.WriteLine(newMessage);
        //            clientStreamWriter.Flush();
        //        }
        //    }
        //    catch (IOException ioe)
        //    {
        //        tcpClient?.Close();
        //    }
        //    finally
        //    {
        //        tcpClient?.Close();
        //    }
        #endregion

    }
}

