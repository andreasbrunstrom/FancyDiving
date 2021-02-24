using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedClasses;
namespace TestTCP
{
    [TestClass]
    public class TestTCP
    {
        [TestMethod]
        public void TestTcp()
        {
            var tcp_server = Tcp_Server.instance();
            var tcp_client = new Tcp_Client();             
        }
    }
}
