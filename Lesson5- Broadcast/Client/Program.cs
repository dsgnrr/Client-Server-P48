using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Client
{
    internal class Program
    {


        static void BroadcastExample()
        {
            using var client = new UdpClient();

            client.EnableBroadcast = true;

            var endpoint = new IPEndPoint(IPAddress.Broadcast, 3000);
            byte[] data = Encoding.UTF8.GetBytes("Hello network");

            try
            {
                client.Send(data, data.Length, endpoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Sending error: {0}", ex.Message);
            }
        }

        static List<string> chatHistory = new List<string>();

        enum MessageType
        {
            Chat,
            HistoryRequest,
            HistoryResponse,
            System
        }
        class ChatMessage
        {
            public MessageType MessageType { get; set; }
            public string Message { get; set; }
            public string Username { get; set; }
            public List<string>? History { get; set; }
        }


        static void BroadcastChat()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();

            Console.WriteLine($"Hello, {name}. You can start typing.");

            using var client = new UdpClient();

            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            client.ExclusiveAddressUse = false;

            client.Client.Bind(new IPEndPoint(IPAddress.Any, 11000));

            Task.Run(() => RecieveMessages(client, name));

            SendSystemMessage(MessageType.HistoryRequest, name);

            while (true)
            {
                string msg = Console.ReadLine();
                if (string.IsNullOrEmpty(msg)) continue;
                SendChatMesage(name, msg, chatHistory);
            }
        }

        static void SendChatMesage(string user, string message, List<string> history)
        {
            var msg = new ChatMessage { MessageType = MessageType.Chat, Username = user, History = history, Message = message };
            history.Add($"{user}-> {message}");
            SendMessage(msg);
        }

        static void SendSystemMessage(MessageType type, string user, List<string>? history = null)
        {
            var msg = new ChatMessage { MessageType = type, Username = user, History = history };
            SendMessage(msg);
        }

        static void SendMessage(ChatMessage msg)
        {
            using var sender = new UdpClient();
            sender.EnableBroadcast = true;
            byte[] data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(msg));
            sender.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, 11000));
        }

        static void RecieveMessages(UdpClient udpClient, string name)
        {
            var remoteEP = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                while (true)
                {
                    byte[] buffer = udpClient.Receive(ref remoteEP);
                    var msg = JsonSerializer.Deserialize<ChatMessage>(Encoding.UTF8.GetString(buffer));
                    if (msg == null) continue;
                    switch (msg.MessageType)
                    {
                        case MessageType.Chat:
                            string line = $"{msg.Username}-> {msg.Message}";
                            chatHistory.Add(line);
                            Console.WriteLine(line);
                            break;
                        case MessageType.HistoryRequest:
                            if (msg.Username != name)
                            {
                                if (chatHistory.Count > 0)
                                {
                                    SendSystemMessage(MessageType.HistoryResponse, name, chatHistory);
                                }
                            }
                            break;
                        case MessageType.HistoryResponse:
                            if(chatHistory.Count == 0)
                            {
                                foreach(var historyline in msg.History)
                                {
                                    if (!chatHistory.Contains(historyline))
                                    {
                                        chatHistory.Add(historyline);
                                        Console.WriteLine(historyline);
                                    }
                                }
                            }
                            break;

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Recieve error: {0}", ex.Message);
            }
        }

        static void Main(string[] args)
        {
            BroadcastChat();
        }
    }
}
