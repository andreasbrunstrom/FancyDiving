using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using static System.Console;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using SpikeTCP_Client;
using SharedClasses;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SpikeTCP_Server
{



    public class HandleTcpClient
    {
        

        public HandleTcpClient(TcpServer _tcpServer, TcpClient _tcpClient, ConcurrentQueue<string> _concurrentQueue)
        {
            concurrentQueue = _concurrentQueue;
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
        public TcpServer tcpServer { get; set; }
        public ConcurrentQueue<string> concurrentQueue { get; set; }

        public void client()
        {
            try
            {
                networkStream = tcpClient.GetStream();
                streamReader = new StreamReader(networkStream);
                streamWriter = new StreamWriter(networkStream);
                
                //int i;
                bool quit = false;
                string message = "";

                while (!quit)
                {
                    message = streamReader.ReadLine();
                    if ( message.StartsWith("quit")) //||message == null)
                    {
                        quit = true;
                        Console.WriteLine($"Quit: {message} from {endPoint}");
                    }
                    else if (message.StartsWith("points"))
                    {
                        Console.WriteLine($"Queue message: {message} from {endPoint}");
                        concurrentQueue.Enqueue(message + ", " + endPoint);
                    }
                    else if (message.StartsWith("Data"))
                    {
                        points p = new points();
                        Console.WriteLine(message);
                        //int a = message.                       
                    }

                    else if(message == "score")
                    {
                        Console.WriteLine($"Message: {message} from {endPoint}");
                        //Console.Clear();
                        IFormatter formatter = new BinaryFormatter();
                        Score obj = (Score)formatter.Deserialize(networkStream);
                        TcpServer.receivedScore.Add(obj);
                        Console.WriteLine(obj.points.ToString());
                        networkStream.Flush();                        
                    }

                    else if(message == "judge")
                    {
                        Console.WriteLine($"Message: {message} from {endPoint}");
                        IFormatter formatter = new BinaryFormatter();
                        Judge obj = (Judge)formatter.Deserialize(networkStream);
                        TcpServer.receivedJudgeFromClient.Add(obj);
                        Console.WriteLine(obj.name);
                        networkStream.Flush();
                    }
                    else
                    {
                        Console.WriteLine($"Queue message: {message} from {endPoint}");
                        concurrentQueue.Enqueue(message + ", " + endPoint);
                    }
                }
            }
            catch (IOException ioe)
            {
                Console.WriteLine("IOException in client function: {0}", ioe);
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
    }

    public class TcpServer
    {
        public TcpServer()
        {
            ////tcpServer = this;
            ////threadServer = new Thread(tcpServer.threadListener());
            ////threadServer.Start();
        }

        protected static TcpServer tcpServer;

        private const int port = 9058;
       // protected IPAddress localAddr = IPAddress.Parse("127.0.0.1");
        //private IPAddress localAddr = IPAddress.Parse("130.243.104.98");
        private IPAddress localAddr = IPAddress.Any;
        protected TcpListener server;
        protected Thread threadServer;
        public ConcurrentQueue<string> concurrentQueue { get; set; } = new ConcurrentQueue<string>();
        public List<HandleTcpClient> listHandleTcpClients { get; set; } = new List<HandleTcpClient>();
        public static List<Score> receivedScore { get; set; } = new List<Score>();
        public static List<Judge> receivedJudgeFromClient { get; set; } = new List<Judge>();
        public List<Judge> j { get; set; } = new List<Judge>(); //Till för att testa sändning av en judgelista

        public static TcpServer instance()
        {
            return tcpServer ?? (tcpServer = new TcpServer());              //?? checks for null. if tcpServer == null, right hand operator is returned
        }

        protected void threadListener(BindingList<Judge> j)
        {
            try
            {
                server = new TcpListener(localAddr, port);
                server.Start();                                             // Start listening for client requests.

                while (true)
                {
                   // Console.WriteLine("Waiting for a connection... ");

                    TcpClient client = server.AcceptTcpClient();
                    if(client.Connected)
                    {
                        lock (listHandleTcpClients)
                        {
                            listHandleTcpClients.Add(new HandleTcpClient(this, client, concurrentQueue));
                        }
                        var crack = client.GetStream();
                        sendJudgeList(crack, j);
                        //listHandleTcpClients[listHandleTcpClients.Count - 1].networkStream.Flush();
                    }
                   
                    //Console.WriteLine("Connected!");
                    //Här skickas dommarlista tillbaka till dommarklienten

                    
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();                                              // Stop listening for new clients.
            }


            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

        public void sendToAllClients(string message)
        {
            lock (listHandleTcpClients)
            {
                if (message.StartsWith("send judges"))
                {
                    Judge j1 = new Judge(1, 2, "olle", "swe");
                    j.Add(j1);
                    IFormatter formatter = new BinaryFormatter();

                    foreach (var client in listHandleTcpClients)
                    {
                        client.streamWriter.WriteLine(message);
                        client.streamWriter.Flush();
                        formatter.Serialize(client.networkStream, j);
                        client.networkStream.Flush();
                    }
                }
                else
                {
                    foreach (var client in listHandleTcpClients)
                    {
                        //byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
                        //client.NetworkStream.Write(msg, 0, msg.Length);
                        client.streamWriter.WriteLine(message);
                        client.streamWriter.Flush();
                    }
                }
            }
        }

        public void writeClientMsg(string msg)
        {
            foreach(var client in listHandleTcpClients)
            {
                client.streamWriter.WriteLine(msg);
                client.streamWriter.Flush();
            }
        }

        public void listClients()
        {
            foreach (var client in listHandleTcpClients)
            {
                WriteLine(client.endPoint);
            }
        }

        public void killThreads()
        {
            threadServer.IsBackground = true;
            foreach (var client in listHandleTcpClients)
            {
                client.threadClient.IsBackground = true;
            }
        }
        public void sendJudgeList(NetworkStream nets, BindingList<Judge> jl)
        {
            lock (nets)
            {
                //byte[] myWriteBuffer = Encoding.ASCII.GetBytes("Are you receiving this message?");
                //nets.Write(myWriteBuffer, 0, myWriteBuffer.Length);
                var xmlser = new XmlSerializer(typeof(BindingList<Judge>));
                if (nets.CanWrite)
                {
                    xmlser.Serialize(nets, jl);                    
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
                    str = ReadLine();// + '\n';
                    //string clientmsg = server.
                    if (str != null && str.StartsWith("quit"))
                    {
                    }
                    else if (str != null && str.StartsWith("queue"))
                    {
                        while (!tcpServer.concurrentQueue.IsEmpty)
                        {
                            string msg;
                            if (tcpServer.concurrentQueue.TryDequeue(out msg))
                            {
                                WriteLine(msg);
                            }
                        }
                    }
                    else if (str != null && str.StartsWith("list"))
                    {
                        tcpServer.listClients();
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
                    else if (str != null && str.StartsWith("Request score"))
                    {
                        tcpServer.sendToAllClients(str);

                    }
                    else
                    {
                        tcpServer.writeClientMsg(str);
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

    class Program
    {
        static void Main(string[] args)
        {
            var server = TcpServer.instance();
            server.dostuff();
            #region testing 1,2,3
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
            #endregion
        }
    }
}