using GenericProtocol.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace pa8_c00061075.Networking
{
    class Client
    {
        private string _serverIp;
        private static ProtoClient<string> _client;


        public Client(string ipAddress)
        {
            _serverIp = ipAddress;
        }

        public void StartClient()
        {
            _client = new ProtoClient<string>(IPAddress.Parse(_serverIp), 51111) { AutoReconnect = true };
            _client.ReceivedMessage += ClientMessageReceived;
            _client.ConnectionLost += Client_ConnectionLost;

            Console.WriteLine("Connecting");
            _client.Connect().GetAwaiter().GetResult();
            Console.WriteLine("Connected!");
            _client.Send("Hello Server!").GetAwaiter().GetResult();
        }

        private void SendToServer(string message)
        {
            _client?.Send(message);
        }
        private void Client_ConnectionLost(IPEndPoint endPoint)
        {
            Console.WriteLine($"Connection lost! {endPoint.Address}");
        }
        private static void ClientMessageReceived(IPEndPoint sender, string message)
        {
            Console.WriteLine($"{sender}: {message}");
        }
    }
}
