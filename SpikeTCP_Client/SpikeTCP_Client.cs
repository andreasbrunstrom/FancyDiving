using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using static System.Console;
using SharedClasses;

namespace SpikeTCP_Client
{
    [Serializable]
    public struct points
    {
        public double score { get; set; }
        public int id { get; set; }
    }
    
    public class Spike_TcpClient
    {
        public Spike_TcpClient(string _serverIPAddress, int _port = 9058)
        {
            serverIPAddress = _serverIPAddress;
            port = _port;
        }
        

        public int port { get; set; }
        public string serverIPAddress { get; set; }
        public List<Judge> js = new List<Judge>();
        public System.Net.Sockets.TcpClient tcpClient = null;
        public StreamReader streamReader { get; set; }
        public StreamWriter streamWriter { get; set; }
        public NetworkStream networkStream { get; set; }
        public Score s = new Score(1, 2, 7.5);

        public void connect()
        {
            try
            {
                tcpClient = new System.Net.Sockets.TcpClient(serverIPAddress, port);
                streamReader = new StreamReader(tcpClient.GetStream());
                streamWriter = new StreamWriter(tcpClient.GetStream());
                networkStream = tcpClient.GetStream();
            }
            catch(SocketException se)
            {
                Console.WriteLine(se.Message);
            }
            
            
        }

        public void sendScore()
        {
            streamWriter.WriteLine("score");
            streamWriter.Flush();
            Console.WriteLine("Enter score");
            double zeScore = double.Parse(Console.ReadLine());
            Score s = new Score(3, 9, zeScore);

            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(networkStream, s);
            networkStream.Flush();
            streamWriter.Flush();
        }

        public void sendMyJudgeID(Judge j)
        {
            streamWriter.WriteLine("judge");
            streamWriter.Flush();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(networkStream, j);
            networkStream.Flush();
            //streamWriter.Flush();
        }
        public void doStuff()
        {
            try
            {
                var str = "";
                while (str != null && !str.StartsWith("quit"))
                {
                    //str = streamReader.ReadLine();
                    //var x = new XmlSerializer(typeof(List<Judge>));
                    //var reader = new XmlNodeReader(str);
                    //using (reader)
                    //{
                    //    List<Judge> name = (List<Judge>) x.Deserialize(reader);
                    //}
                    //StringReader strReader = new StringReader(str);
                    //List<Judge> lj = (List<Judge>) x.Deserialize(strReader);

                    //foreach (var j in lj)
                    //{
                    //    WriteLine(j.name);
                    //}
                    //    //str = ReadLine();
                    //WriteLine(str);

                    if (str.StartsWith("go to next"))
                    {
                        Console.WriteLine("Go to next jump");
                        streamWriter.WriteLine("Next jump back to server");
                        // Go to next here
                    }

                    if (str.StartsWith("go to end"))
                    {
                        Console.WriteLine("Go to end");
                        streamWriter.WriteLine("Go to end back to server");
                        //Go to end here
                    }

                    if (str.StartsWith("force quit"))
                    {
                        Console.WriteLine("Force quit");
                        streamWriter.WriteLine("Force quit back to server");
                        //Force quit here

                        return;
                    }

                    if (str.StartsWith("Request score"))
                    {
                        streamWriter.WriteLine("score");
                        streamWriter.Flush();
                        Console.WriteLine("Enter score");
                        double zeScore = double.Parse(Console.ReadLine());
                        Score s2 = new Score(1, 2, zeScore);

                        IFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(networkStream, s2);
                        networkStream.Flush();
                        streamWriter.Flush();
                    }

                    if (str == "send judges") //Test för att se om man kan ta emot en lista med judges från servern.
                    {
                        lock(networkStream)
                        streamWriter.Flush();
                        IFormatter formatter = new BinaryFormatter();
                        List<Judge> j = (List<Judge>)formatter.Deserialize(networkStream);
                        js = j;
                        networkStream.Flush();
                        //var newMessage2 = ReadLine();
                        //streamWriter.WriteLine(newMessage2);
                        //streamWriter.Flush();
                    }

                    //var newMessage = ReadLine();
                    //streamWriter.WriteLine(newMessage);
                    //streamWriter.Flush();
                }
            }
            catch (IOException)
            {
                tcpClient?.Close();
            }
            finally
            {
                tcpClient?.Close();
            }
        }

        public void readStuff()
        {
            List<Judge> testlist = null;
            try
            {
                var xs = new XmlSerializer(typeof(List<Judge>));
                var tr = new StreamReader(tcpClient.GetStream());
                testlist = (List<Judge>) xs.Deserialize(tr);
                tr.Close();

                var testlist2 = testlist.ToString();

                foreach (var line in testlist2)
                {
                    WriteLine(line);
                }
            }
            finally
            {
                
            }
        }

        public void ReceiveJudgelist()
        {
            try
            {
                while(networkStream.CanRead)
                {
                    //str = streamReader.ReadLine();
                    if (networkStream.CanRead)
                    {
                        byte[] myReadBuffer = new byte[1024];
                        StringBuilder myCompleteMessage = new StringBuilder();
                        int numberOfBytesRead = 0;

                        do
                        {
                            numberOfBytesRead = networkStream.Read(myReadBuffer, 0, myReadBuffer.Length);

                            myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));

                        } while (networkStream.DataAvailable);
                        Console.WriteLine("You received the following message : " + myCompleteMessage);

                        XmlSerializer serializer = new XmlSerializer(typeof(List<Judge>));
                        using (TextReader reader = new StringReader(myCompleteMessage.ToString()))
                        {
                            var result = (List<Judge>)serializer.Deserialize(reader);
                            js = result;
                        }
                         foreach (var sak in js)
                        {
                            Console.WriteLine(sak.name);
                        }   
                    }

                }
            }
            catch(IOException)
            {
                tcpClient.Close();
            }
            finally
            {
                tcpClient.Close();
            }
            
        }
        private static void Main(string[] args)
        {
            Console.WriteLine("Ange ip eller slå enter för localhost:");
            var str = Console.ReadLine();
            if (str == "\n")
            {
                str = "127.0.0.1";
            }
            SpikeTCP_Client.Spike_TcpClient klient = new Spike_TcpClient("127.0.0.1");
            Judge j1 = new Judge(1, 2, "Anton Starck", "SWE");
            klient.connect();
            klient.ReceiveJudgelist();
            //klient.doStuff();
            //klient.sendScore();
            //klient.sendMyJudgeID(j1);
            Console.ReadLine();

            #region gammalt skit   
        //    Score s = new Score(1, 2, 7.5);
        //    List<Judge> js = new List<Judge>();
        ////Connection start
        //System.Net.Sockets.TcpClient tcpClient = null;
        //    try
        //    {
        //        const int port = 9058;
        //        //tcpClient = new System.Net.Sockets.TcpClient("130.243.104.98", port);
        //        tcpClient = new System.Net.Sockets.TcpClient("127.0.0.1", port);

        //        var streamReader = new StreamReader(tcpClient.GetStream());
        //        var streamWriter = new StreamWriter(tcpClient.GetStream());
        //        NetworkStream stream = tcpClient.GetStream();

        //        var str = "";
        //        while (str != null && !str.StartsWith("quit"))
        //        {
        //            str = streamReader.ReadLine();
        //            WriteLine(str);

        //            if (str.StartsWith("go to next"))
        //            {
        //                Console.WriteLine("Go to next jump");
        //                streamWriter.WriteLine("Next jump back to server");
        //                // Go to next here
        //            }

        //            if (str.StartsWith("go to end"))
        //            {
        //                Console.WriteLine("Go to end");
        //                streamWriter.WriteLine("Go to end back to server");
        //                //Go to end here
        //            }

        //            if (str.StartsWith("force quit"))
        //            {
        //                Console.WriteLine("Force quit");
        //                streamWriter.WriteLine("Force quit back to server");
        //                //Force quit here

        //                return;
        //            }

        //            if (str.StartsWith("Request score"))
        //            {
        //                streamWriter.WriteLine("score");
        //                streamWriter.Flush();
        //                IFormatter formatter = new BinaryFormatter();
        //                formatter.Serialize(stream, s);
        //                stream.Flush();

        //                var newMessage2 = ReadLine();
        //                streamWriter.WriteLine(newMessage2);
        //                streamWriter.Flush();
        //            }

        //            if (str == "send judges") //Test för att se om man kan ta emot en lista med judges från servern.
        //            {
        //                //streamWriter.Flush();
        //                IFormatter formatter = new BinaryFormatter();
        //                List<Judge> j = (List<Judge>)formatter.Deserialize(stream);
        //                js = j;
        //                stream.Flush();
        //                //var newMessage2 = ReadLine();
        //                //streamWriter.WriteLine(newMessage2);
        //                //streamWriter.Flush();
        //            }

        //            var newMessage = ReadLine();
        //            streamWriter.WriteLine(newMessage);
        //            streamWriter.Flush();
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
}

