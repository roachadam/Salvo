
using GenericProtocol;
using GenericProtocol.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;

namespace pa8_c00061075.Networking
{
    class Server
    {
        private ProtoServer<string> _server;
        public Server()
        {

        }

        public void StartServer()
        {
            _server =  new ProtoServer<string>(IPAddress.Any, 51111);
            Console.WriteLine("Starting Server...");
            _server.Start();
            Console.WriteLine("Server started!");
            _server.ClientConnected += ClientConnected;
            _server.ReceivedMessage += ServerMessageReceived;
        }
        private async void ServerMessageReceived(IPEndPoint sender, string message)
        {
            Console.WriteLine($"{sender}: {message}");
            await _server.Send($"Hello {sender}!", sender);
        }

        private void ClientMessageReceived(IPEndPoint sender, string message)
        {
            Console.WriteLine($"{sender}: {message}");
        }

        private async void ClientConnected(IPEndPoint address)
        {
            await _server.Send($"Hello {address}!", address);
        }
    }
    [ZeroFormattable]
    public struct MessageObject
    {
        [Index(0)]
        public string Sender { get; set; }
        [Index(1)]
        public string Recipient { get; set; }
        [Index(2)]
        public string Message { get; set; }
        [Index(3)]
        public DateTime Timestamp { get; set; }
    }
}
