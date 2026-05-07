using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDPClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using UdpClient client = new();
            string serverHost = "10.3.1.7";
            int port = 3000;

            try
            {

                byte[] request = Encoding.UTF8.GetBytes(Console.ReadLine());
                int bytes = await client.SendAsync(request, request.Length, serverHost, port);

                UdpReceiveResult result = await client.ReceiveAsync();
                Console.WriteLine($"Server[{result.RemoteEndPoint}]->{Encoding.UTF8.GetString(result.Buffer)}");
            }
            catch (Exception ex) {
                Console.WriteLine("Error connect to server: {0}", ex.Message);
            }
        }
    }
}
