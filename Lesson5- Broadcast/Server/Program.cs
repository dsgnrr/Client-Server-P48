using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var reciever = new UdpClient(3000);

            while (true)
            {
                var remoteEP = new IPEndPoint(IPAddress.Any, 0);

                byte[]recieveData = reciever.Receive(ref remoteEP);

                Console.WriteLine($"{remoteEP.Address}> {Encoding.UTF8.GetString(recieveData)}");
            }
        }
    }
}
