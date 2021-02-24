using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using log4net;

namespace SharedClasses
{
    public class SendReceive
    {
        public string receive(NetworkStream clientStream)
        {
            if (!clientStream.CanRead) return null;
            var myReadBuffer = new byte[2048];
            var myCompleteMessage = new StringBuilder();

            do
            {
                var numberOfBytesRead = clientStream.Read(myReadBuffer, 0, myReadBuffer.Length);
                myCompleteMessage.AppendFormat("{0}", Encoding.UTF8.GetString(myReadBuffer, 0, numberOfBytesRead));
            } while (clientStream.DataAvailable);
            return myCompleteMessage.ToString();
        }

        public void sendData<T>(string cmd, T data, NetworkStream networkStream)
        {
            try
            {
                var msg = Encoding.UTF8.GetBytes(cmd + "@" + serializer(data));
                networkStream.Write(msg, 0, msg.Length);
            }
            finally
            {
            }
        }

        public static string serializer<T>(T dataToSerialize)
        {
            try
            {
                var stringwriter = new StringWriter();
                var serializer = new XmlSerializer(dataToSerialize.GetType());
                serializer.Serialize(stringwriter, dataToSerialize);
                return stringwriter.ToString();
            }
            finally
            {
            }
        }

        public static T deserializer<T>(string xmlText)
        {
            try
            {
                var stringReader = new StringReader(xmlText);
                var serializer = new XmlSerializer(typeof(T));

                return (T)serializer.Deserialize(stringReader);
            }
            finally
            {
            }
        }
    }
}
