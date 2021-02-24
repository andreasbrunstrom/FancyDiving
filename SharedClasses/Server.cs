using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SharedClasses
{
    public delegate void DelegateIncommingDataCallback(TcpClient tcpClient, string str);
    public delegate void DelegateNewClientCallback(TcpClient tcpClient);
    public delegate void DelegatDisconnectCallback(TcpClient tcpClient);

    public class Server
    {
        private TcpClient tcpClient;
        private Thread thrListener;
        private TcpListener tlsClient;
        private bool serverRunning;

        public static List<TcpClient> clientsList = new List<TcpClient>();
        public static event DelegateIncommingDataCallback eventIncommingDataCallback;
        public static event DelegateNewClientCallback eventNewClientCallback;
        public static event DelegatDisconnectCallback eventDisconnectCallback;

        public void startListening()
        {
            tlsClient = new TcpListener(IPAddress.Any,9058);
            tlsClient.Start();

            serverRunning = true;

            thrListener = new Thread(keepListening);
            thrListener.Start();
        }

        private void keepListening()
        {
            while (serverRunning)
            {
                try
                {
                    tcpClient = tlsClient.AcceptTcpClient();
                    var judgeClient = new JudgeClient(tcpClient);
                }
                catch (SocketException)
                {
                    ; // Do nothing
                }
            }
        }

        public void stopListening()
        {
            serverRunning = false;
            if (thrListener != null && thrListener.IsAlive)
            {
                tlsClient.Stop();

                while (thrListener.IsAlive)
                {
                    Application.DoEvents();
                }
            }

            for (var i = clientsList.Count - 1; i > 0; i--)
            {
                removeClient(clientsList[i]);
            }

            thrListener?.Abort();
            clientsList.Clear();
            tcpClient?.Close();
        }

        public static void addClient(TcpClient tcpUser)
        {
            clientsList.Add(tcpUser);
            eventNewClientCallback?.Invoke(tcpUser);
        }

        public static void removeClient(TcpClient tcpUser)
        {
            eventDisconnectCallback?.Invoke(tcpUser);
            clientsList.Remove(tcpUser);
        }

        public static void sendData<T>(string cmd, T data, TcpClient tcpClient)
        {
            var networkStream = tcpClient.GetStream();
            
            try
            {
                var _data = "";
                if (data is string)
                {
                    _data = data.ToString();
                }
                else
                {
                    _data = SendReceive.serializer(data);
                }
                var msg = Encoding.UTF8.GetBytes(cmd + "@" + _data);
                networkStream.Write(msg, 0, msg.Length);
                networkStream.Flush();
            }
            catch
            {
                removeClient(tcpClient);
            }
        }

        public static void sendDataToAll<T>(string cmd, T data)
        {
            TcpClient[] tcpClients = new TcpClient[clientsList.Count];
            
            clientsList.CopyTo(tcpClients, 0);

            foreach (var client in tcpClients)
            {
                sendData(cmd, data, client);
            }
        }

        public static void receiveFromClient(TcpClient tcpClient ,string str)
        {
            eventIncommingDataCallback?.Invoke(tcpClient, str);
        }
    }

    class JudgeClient
    {
        private TcpClient tcpClient;
        private string strResponse;

        public JudgeClient(TcpClient tcpCon)
        {
            tcpClient = tcpCon;
            var thrSender = new Thread(addAndListen);
            thrSender.Start();
        }

        public void closeConnection()
        {
            tcpClient.Close();
        }

        private void addAndListen()
        {
            //var ip = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();

            Server.addClient(tcpClient);

            try
            {
                while ((strResponse = receiveData()) != "")
                {
                    if (strResponse == null)
                    {
                        Server.removeClient(tcpClient);
                        break;
                    }
                    else
                    {
                        Server.receiveFromClient(tcpClient, strResponse);
                    }
                }
            }
            catch
            {
                // If anything went wrong with this user, disconnect him
                Server.removeClient(tcpClient);
            }
        }

        private string receiveData()
        {
            var clientStream = tcpClient.GetStream();
            if (clientStream.CanRead)
            {
                var myReadBuffer = new byte[2048];
                var myCompleteMessage = new StringBuilder();

                do
                {
                    var numberOfBytesRead = clientStream.Read(myReadBuffer, 0, myReadBuffer.Length);
                    myCompleteMessage.AppendFormat("{0}", Encoding.UTF8.GetString(myReadBuffer, 0, numberOfBytesRead));
                } while (clientStream.DataAvailable);


                if (myCompleteMessage.Length < 1)
                {
                    return null;
                }
                return myCompleteMessage.ToString();
            }
            return null;
        }
    }
}
