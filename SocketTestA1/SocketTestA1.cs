using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace SocketTestA1
{
    class SocketTestA1
    {
        static void Main(string[] args)
        {
            string hostString = "oru.se";
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPHostEntry ipHostInfo = Dns.Resolve(hostString);

            IPAddress ipAdress = ipHostInfo.AddressList[0];

            IPEndPoint ipe = new IPEndPoint(ipAdress, 11000);

            Console.WriteLine(ipAdress.ToString());
            try
            {
                s.Connect(ipe);
            }
            catch (ArgumentNullException ae)
            {
                Console.WriteLine("ArgumentNullException : {0}", ae.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
        }
    }
}
