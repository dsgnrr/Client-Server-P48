using System.Net.Sockets;
using System.Text;

namespace TCPandUDPClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using TcpClient client = new("127.0.0.1", 3000);
            using NetworkStream stream = client.GetStream();

            byte[] data = Encoding.UTF8.GetBytes("Hi server");
            stream.Write(data, 0, data.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0,buffer.Length);
            Console.WriteLine("Server say: {0}", Encoding.UTF8.GetString(buffer, 0, bytesRead));
        }
    }
}
