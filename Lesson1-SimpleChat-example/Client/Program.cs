using System.Net;
using System.Net.Sockets;
using static System.Text.Encoding;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("10.3.1.7"), 3000);
            try
            {
                socket.Connect(remoteEP);
                Console.WriteLine("Connected: {0}", socket.RemoteEndPoint);

                Console.Write("Enter message to send: ");
                byte[] buffer = new byte[1024];
                int count = 0;
                while (count<5)
                {
                    Console.Write(">");
                    string message = Console.ReadLine();
                    byte[] data = UTF8.GetBytes(message);
                    socket.Send(data);

                    int bytesRead = socket.Receive(buffer);
                    Console.WriteLine("Server: {0}", UTF8.GetString(buffer, 0, bytesRead));
                    count++;
                }
                socket.Shutdown(SocketShutdown.Receive);
                Console.WriteLine("Connection closed");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection error: {0}", ex.Message);
            }
        }
    }
}
