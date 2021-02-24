using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static System.Console;

namespace SharedClasses
{
    public class HandleTcpClient: SendReceive, INotifyPropertyChanged
    {
        public HandleTcpClient(Tcp_Server _tcpServer, TcpClient _tcpClient, ConcurrentQueue<Score> _scoreQueue, ConcurrentQueue<Judge> _judgeQueue)
        {
            scoreQueue = _scoreQueue;
            judgeQueue = _judgeQueue;
            tcpServer = _tcpServer;
            tcpClient = _tcpClient;
            endPoint = tcpClient.Client.RemoteEndPoint as IPEndPoint;

            threadClient = new Thread(client);
            threadClient.Start();
            
        }

        public IPEndPoint endPoint { get; set; }
        public NetworkStream networkStream { get; set; }
        public StreamReader streamReader { get; set; }
        public StreamWriter streamWriter { get; set; }
        public TcpClient tcpClient { get; set; }
        public Thread threadClient { get; set; }
        public Tcp_Server tcpServer { get; set; }
        public ConcurrentQueue<Score> scoreQueue { get; set; }
        public ConcurrentQueue<Judge> judgeQueue { get; set; }
        public Score receivedScore { get; set; }
        private Judge _localJudge;
        public bool validatingJudge { get; set; }

        public Judge localJudge
        {
            get { return _localJudge; }
            set
            {
                _localJudge = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(localJudge.id.ToString()));
            }
        }

        public void client()
        {
            try
            {
                networkStream = tcpClient.GetStream();
                streamReader = new StreamReader(networkStream);
                streamWriter = new StreamWriter(networkStream);

                var quit = false;
                
                while (!quit)
                {
                    try
                    {
                        receive(networkStream);
                        switch (command)
                        {
                            case "judge":
                                lock (judgeQueue)
                                {
                                    localJudge = deserializer<Judge>(msgRecieved);
                                    judgeQueue.Enqueue(localJudge);
                                    validatingJudge = true;
                                }
                                break;

                            case "score":
                                receivedScore = deserializer<Score>(msgRecieved);
                                scoreQueue.Enqueue(receivedScore);
                                break;

                            default:
                                break;
                        }                     
                    }
                    catch(IOException e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
            catch (IOException ioe)
            {
                MessageBox.Show(ioe.Message);
            }
            finally
            {
                tcpClient.Close(); // Shutdown and end connection
                threadClient.IsBackground = true;
                removeFromListHandleTcpClients();
                threadClient.Abort();
            }
        }

        protected void removeFromListHandleTcpClients()
        {
            for (var i = 0; i < tcpServer.listHandleTcpClients.Count; i++)
            {
                if (this == tcpServer.listHandleTcpClients[i])
                {
                    tcpServer.listHandleTcpClients.Remove(this);
                    break;
                }
            }
        }

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Event declaration.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }

    public class Tcp_Server: SendReceive
    {
        protected static Tcp_Server tcpServer;

        private const int port = 9058;
        private IPAddress endPointAddressIP = IPAddress.Any;
        protected TcpListener server;
        protected Thread threadServer;
        public ConcurrentQueue<Judge> judgeQueue { get; set; } = new ConcurrentQueue<Judge>();
        public ConcurrentQueue<Score> scoreQueue { get; set; } = new ConcurrentQueue<Score>();
        public List<HandleTcpClient> listHandleTcpClients { get; set; } = new List<HandleTcpClient>();
        public static Tcp_Server instance()
        {
            return tcpServer ?? (tcpServer = new Tcp_Server());              //?? checks for null. if tcpServer == null, right hand operator is returned
        }

        protected void threadListener(BindingList<Judge> judgeListToClients)
        {
            try
            {
                server = new TcpListener(endPointAddressIP, port);
                server.Start();                                             // Start listening for client requests.

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    if (client.Connected)
                    {
                        var newClient = new HandleTcpClient(this, client, scoreQueue, judgeQueue);
                        lock (listHandleTcpClients)
                        {
                            listHandleTcpClients.Add(newClient);
                        }
                        sendJudgeList(client.GetStream(), judgeListToClients);
                    }
                }
            }
            catch (SocketException e)
            {
                WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();                                              // Stop listening for new clients.
            }
            
            WriteLine("\nHit enter to continue...");
            Read();
        }

        public void sendToAllClients(string message)
        {
            lock (listHandleTcpClients)
            {
                foreach (var client in listHandleTcpClients)
                {
                    client.streamWriter.WriteLine(message);
                    client.streamWriter.Flush();
                }
            }
        }

        public void killThreads()
        {
            threadServer.IsBackground = true;
            foreach (var client in listHandleTcpClients)
            {
                client.streamWriter.WriteLine("exit");
                client.streamWriter.Flush();
                client.threadClient.IsBackground = true;
            }
        }
        public void sendJudgeList(NetworkStream networkStream, BindingList<Judge> jl) //Argumentet var NetworkStream förut
        {
            lock (networkStream)
            {
                byte[] msg = Encoding.UTF8.GetBytes("judgelist@"+ serializer(jl));
                networkStream.Write(msg, 0, msg.Length);
            }
        }

        public void sendJumpToClients(Jump jump)
        {
            lock(listHandleTcpClients)
            {
                foreach(var client in listHandleTcpClients)
                {
                    byte[] msg = Encoding.UTF8.GetBytes("jump@" + serializer(jump));
                    client.networkStream.Write(msg, 0, msg.Length);
                    client.networkStream.Flush();
                }
            }
        }

        public void dostuff()
        {
            try
            {
                string str;
                do
                {
                    str = ReadLine();
                   
                    if (str != null && str.StartsWith("quit"))
                    {
                    }
                    else if (str != null && str.StartsWith("queue"))
                    {
                        while (!tcpServer.scoreQueue.IsEmpty)
                        {
                            Score msg;
                            if (tcpServer.scoreQueue.TryDequeue(out msg))
                            {
                                WriteLine(msg);
                            }
                        }
                    }
                    else if (str != null && str.StartsWith("list"))
                    {
                       
                    }
                    else if (str != null && str.StartsWith("go to next"))
                    {
                        tcpServer.sendToAllClients(str);
                    }
                    else if (str != null && str.StartsWith("go to end"))
                    {
                        tcpServer.sendToAllClients(str);
                    }
                    else if (str != null && str.StartsWith("force quit"))
                    {
                        tcpServer.sendToAllClients(str);
                    }
                    else if (str != null && str.StartsWith("send judges"))
                    {
                        tcpServer.sendToAllClients(str);
                    }
                   
                } while (!str.StartsWith("quit"));
            }
            finally
            {
                tcpServer.killThreads();
                WriteLine("Quit Program");
            }
        }
    }

    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        var server = Tcp_Server.instance();
    //        server.dostuff();
    //        #region testing 1,2,3
            //try
            //{
            //    string str;
            //    do
            //    {
            //        str = ReadLine();// + '\n';
            //        //string clientmsg = server.
            //        if (str != null && str.StartsWith("quit"))
            //        {
            //        }
            //        else if (str != null && str.StartsWith("queue"))
            //        {
            //            while (!server.scoreQueue.IsEmpty)
            //            {
            //                string msg;
            //                if (server.scoreQueue.TryDequeue(out msg))
            //                {
            //                    WriteLine(msg);
            //                }
            //            }
            //        }
            //        else if (str != null && str.StartsWith("list"))
            //        {
            //            server.listClients();
            //        }
            //        else if (str != null && str.StartsWith("go to next"))
            //        {
            //            server.sendToAllClients(str);
            //        }
            //        else if (str != null && str.StartsWith("go to end"))
            //        {
            //            server.sendToAllClients(str);
            //        }
            //        else if (str != null && str.StartsWith("force quit"))
            //        {
            //            server.sendToAllClients(str);
            //        }
            //        else if (str != null && str.StartsWith("send judges"))
            //        {
            //           server.sendToAllClients(str);
            //        }
            //        else if (str != null && str.StartsWith("Request score"))
            //        {
            //            server.sendToAllClients(str);

            //        }
            //        else
            //        {
            //            server.writeClientMsg(str);
            //        }
            //    } while (!str.StartsWith("quit"));
            //}
            //finally
            //{
            //    server.killThreads();
            //    WriteLine("Quit Program");
            //}
            //#endregion
        //}
    //}
}