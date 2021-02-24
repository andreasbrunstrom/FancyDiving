using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Collections;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace TCPServerTests
{
    // Holds the arguments for the StatusChanged event
    public class StatusChangedEventArgs : EventArgs
    {
        // The argument we're interested in is a message describing the event
        private string EventMsg;

        // Property for retrieving and setting the event message
        public string EventMessage
        {
            get
            {
                return EventMsg;
            }
            set
            {
                EventMsg = value;
            }
        }

        // Constructor for setting the event message
        public StatusChangedEventArgs(string strEventMsg)
        {
            EventMsg = strEventMsg;
        }
    }

    // This delegate is needed to specify the parameters we're passing with our event
    public delegate void StatusChangedEventHandler(object sender, StatusChangedEventArgs e);

    class Server
    {
        // This hash table stores users and connections (browsable by user)
        public static Hashtable htUsers = new Hashtable(30); // 30 users at one time limit
        // This hash table stores connections and users (browsable by connection)
        public static Hashtable htConnections = new Hashtable(30); // 30 users at one time limit
        // Will store the IP address passed to it
        private IPAddress ipAddress;
        private TcpClient tcpClient;
        // The event and its argument will notify the form when a user has connected, disconnected, send message, etc.
        public static event StatusChangedEventHandler StatusChanged;
        private static StatusChangedEventArgs e;

        // The constructor sets the IP address to the one retrieved by the instantiating object
        public Server(IPAddress address)
        {
            ipAddress = address;
        }

        // The thread that will hold the connection listener
        private Thread thrListener;

        // The TCP object that listens for connections
        private TcpListener tlsClient;

        // Will tell the while loop to keep monitoring for connections
        bool ServRunning = false;

        // Add the user to the hash tables
        public static void AddUser(TcpClient tcpUser, string strUsername)
        {
            // First add the username and associated connection to both hash tables
            Server.htUsers.Add(strUsername, tcpUser);
            Server.htConnections.Add(tcpUser, strUsername);

            // Tell of the new connection to all other users and to the server form
            //SendAdminMessage(htConnections[tcpUser] + " har anslutit");
            e = new StatusChangedEventArgs("Server: " + htConnections[tcpUser] + " har anslutit");
            OnStatusChanged(e);
        }

        // Remove the user from the hash tables
        public static void RemoveUser(TcpClient tcpUser)
        {
            // If the user is there
            if (htConnections[tcpUser] != null)
            {
                // First show the information and tell the other users about the disconnection
                SendAdminMessage(htConnections[tcpUser] + " har kopplat ifrån");

                // Remove the user from the hash table
                Server.htUsers.Remove(Server.htConnections[tcpUser]);
                Server.htConnections.Remove(tcpUser);
            }
        }

        // This is called when we want to raise the StatusChanged event
        public static void OnStatusChanged(StatusChangedEventArgs e)
        {
            StatusChanged?.Invoke(null, e);
        }

        // Send administrative messages
        public static void SendAdminMessage(string Message)
        {
            StreamWriter swSenderSender;

            // First of all, show in our application who says what
            e = new StatusChangedEventArgs("Server: " + Message);
            OnStatusChanged(e);

            // Create an array of TCP clients, the size of the number of users we have
            TcpClient[] tcpClients = new TcpClient[Server.htUsers.Count];
            // Copy the TcpClient objects into the array
            Server.htUsers.Values.CopyTo(tcpClients, 0);
            // Loop through the list of TCP clients
            for (int i = 0; i < tcpClients.Length; i++)
            {
                // Try sending a message to each
                try
                {
                    // If the message is blank or the connection is null, break out
                    if (Message.Trim() == "" || tcpClients[i] == null)
                    {
                        continue;
                    }
                    // Send the message to the current user in the loop
                    swSenderSender = new StreamWriter(tcpClients[i].GetStream());
                    swSenderSender.WriteLine("Server: " + Message);
                    swSenderSender.Flush();
                    swSenderSender = null;

                }
                catch // If there was a problem, the user is not there anymore, remove him
                {
                    RemoveUser(tcpClients[i]);
                }
            }
        }

        // Send messages from one user to all the others
        public static void SendMessage(string From, string Message)
        {
            StreamWriter swSenderSender;

            // First of all, show in our application who says what
            e = new StatusChangedEventArgs(From + ":\r\n" + Message);
            OnStatusChanged(e);

            //// Create an array of TCP clients, the size of the number of users we have
            //TcpClient[] tcpClients = new TcpClient[Server.htUsers.Count];
            //// Copy the TcpClient objects into the array
            //Server.htUsers.Values.CopyTo(tcpClients, 0);
            //// Loop through the list of TCP clients
            //for (int i = 0; i < tcpClients.Length; i++)
            //{
            //    // Try sending a message to each
            //    try
            //    {
            //        // If the message is blank or the connection is null, break out
            //        if (Message.Trim() == "" || tcpClients[i] == null)
            //        {
            //            continue;
            //        }
            //        // Send the message to the current user in the loop
            //        swSenderSender = new StreamWriter(tcpClients[i].GetStream());
            //        swSenderSender.WriteLine(From + " skriver: " + Message);
            //        swSenderSender.Flush();
            //        swSenderSender = null;
            //    }
            //    catch // If there was a problem, the user is not there anymore, remove him
            //    {
            //        RemoveUser(tcpClients[i]);
            //    }
            //}
        }

        public void StartListening()
        {

            // Get the IP of the first network device, however this can prove unreliable on certain configurations
            IPAddress ipaLocal = ipAddress;

            // Create the TCP listener object using the IP of the server and the specified port
            tlsClient = new TcpListener(9058);

            // Start the TCP listener and listen for connections
            tlsClient.Start();

            // The while loop will check for true in this before checking for connections
            ServRunning = true;

            // Start the new tread that hosts the listener
            thrListener = new Thread(KeepListening);
            thrListener.Start();
        }

        private void KeepListening()
        {

            // While the server is running
            while (ServRunning)
            {
                try
                {
                    // Accept a pending connection
                    tcpClient = tlsClient.AcceptTcpClient();

                    // Create a new instance of Connection
                    Connection newConnection = new Connection(tcpClient);
                }
                catch (SocketException e)
                {
                    ; // Do nothing
                }
            }
        }


        public void RequestStop()
        {
            if (thrListener != null && thrListener.IsAlive) // thread is active
            {
                
                TcpClient[] tcpClients = new TcpClient[Server.htUsers.Count];
                Server.htUsers.Values.CopyTo(tcpClients, 0);
                foreach (var t in tcpClients)
                {
                    RemoveUser(t);
                }


                // set event \"Stop\"
                tlsClient.Stop();
                
                ServRunning = false;
                htConnections.Clear();
                htUsers.Clear();


                while (thrListener.IsAlive)
                {
                    //Application.DoEvents(); ersat nedan
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                                          new Action(delegate { }));
                }
            }
        }

        public void sendData<T>(string cmd, T data, NetworkStream networkStream = null)
        {
            if (networkStream == null)
            {
                networkStream = tcpClient.GetStream();
            }
            try
            {
                var _data = "";
                if (data is string)
                {
                    _data = data.ToString();
                }
                else
                {
                    _data = serializer(data);
                }
                var msg = Encoding.UTF8.GetBytes(cmd + "@" + _data);
                networkStream.Write(msg, 0, msg.Length);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message+e.Source);
            }
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

    }

    // This class handels connections; there will be as many instances of it as there will be connected users
    class Connection
    {
        TcpClient tcpClient;
        // The thread that will send information to the client
        private Thread thrSender;
        private StreamReader srReceiver;
        private StreamWriter swSender;
        private string currUser;
        private string strResponse;

        // The constructor of the class takes in a TCP connection
        public Connection(TcpClient tcpCon)
        {
            tcpClient = tcpCon;
            // The thread that accepts the client and awaits messages
            thrSender = new Thread(AcceptClient);
            // The thread calls the AcceptClient() method
            thrSender.Start();
        }

        public void CloseConnection()
        {
            // Close the currently open objects
            tcpClient.Close();
            srReceiver.Close();
            swSender.Close();
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

                var splitmessage = myCompleteMessage.ToString().Split('@');

                return myCompleteMessage.ToString();
            }
            return null;
        }


        // Occures when a new client is accepted
        private void AcceptClient()
        {
            srReceiver = new System.IO.StreamReader(tcpClient.GetStream());
            swSender = new System.IO.StreamWriter(tcpClient.GetStream());

            var ip = ((IPEndPoint) tcpClient.Client.RemoteEndPoint).Address.ToString();

            Server.AddUser(tcpClient, ip);

            try
            {
                // Keep waiting for a message from the user
                while ((strResponse = receive(tcpClient.GetStream())) != "")
                {
                    // If it's invalid, remove the user
                    if (strResponse == null)
                    {
                    }
                    else
                    {
                        // Otherwise send the message to all the other users
                        Server.SendMessage(ip, strResponse);
                    }
                }
            }
            catch
            {
                // If anything went wrong with this user, disconnect him
            }
        }
    }
}
