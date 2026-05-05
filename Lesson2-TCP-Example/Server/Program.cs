using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPandUDP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TcpListener tcp = new(
                new IPEndPoint(IPAddress.Parse("127.0.0.1")
                , 3000));

            tcp.Start();
            while (true)
            {
                if (tcp.Pending())
                {

                    using TcpClient client = tcp.AcceptTcpClient();
                    Console.WriteLine($"Є з'єднання: {client.Client.RemoteEndPoint}");
                    //client.Close(); // відключення клієнта
                    using NetworkStream stream = client.GetStream();

                    byte[] buffer = new byte[1024];
                    int byteRead = stream.Read(buffer, 0, buffer.Length);
                    string request = Encoding.UTF8.GetString(buffer, 0, byteRead);
                    Console.WriteLine("Client say: {0}", request);

                    byte[] responseData = Encoding.UTF8.GetBytes("Hello from Server");
                    stream.Write(responseData, 0, responseData.Length);
                }
            }
        }
    }
}
