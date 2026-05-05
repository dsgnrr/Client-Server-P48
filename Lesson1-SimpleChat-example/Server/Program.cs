using System.Net;
using System.Net.Sockets;
using static System.Text.Encoding;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //IPEndPoint localEP = new IPEndPoint(IPAddress.Any, 3000);
            IPEndPoint localEP = new IPEndPoint(IPAddress.Parse("10.3.1.7"), 3000);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEP);
                listener.Listen(10);

                Console.WriteLine("Waiting...");

                using Socket handler = listener.Accept();
                Console.WriteLine("Client #{0} connected", handler.RemoteEndPoint);

                byte[] buffer = new byte[1024];
                int count = 0;
                while (count<5)
                {
                    int bytesRead = handler.Receive(buffer);
                    string recievedData = UTF8.GetString(buffer, 0, bytesRead);

                    Console.WriteLine("Client: {0}", recievedData);

                    Console.Write("Answer client: ");
                    string response = Console.ReadLine();
                    handler.Send(UTF8.GetBytes(response));

                    count++;
                }
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Server error: {0}", ex.Message);
            }
        }
    }
}
