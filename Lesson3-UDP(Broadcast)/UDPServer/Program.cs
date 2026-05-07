using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDPServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            int port = 3000;
            //using UdpClient server = new UdpClient(
            //    new IPEndPoint(
            //        IPAddress
            //        .Parse("10.3.1.7"), 
            //        port)
            //    );

            using UdpClient server = new UdpClient(port);

            while (true)
            {
                try
                {
                    UdpReceiveResult result = await server.ReceiveAsync();
                    byte[] recieveResult = result.Buffer;

                    Console.WriteLine($"{result.RemoteEndPoint} -> {Encoding.UTF8.GetString(recieveResult)}");

                    byte[] response = Encoding.UTF8.GetBytes("Hello from server");

               
                    await server.SendAsync(
                        response,
                        response.Length,
                        result.RemoteEndPoint);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Can`t send response, because is dead");
                }
            }
        }
    }
}
