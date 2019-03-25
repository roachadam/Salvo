
using GenericProtocol.Implementation;
using System;
using System.Net;
using System.Windows.Forms;
using pa8_c00061075.Structs;

namespace pa8_c00061075.Networking
{
    class Server
    {
        private ProtoServer<SalvoData> _server;

        public event AttackHandler Attack = delegate { };
        public delegate void AttackHandler(SalvoAttack attack);

        public event AttackResultHandler AttackResult = delegate { };
        public delegate void AttackResultHandler(SalvoAttackResult attackResult);

        public event WinResultHandler WinResult = delegate { };
        public delegate void WinResultHandler(SalvoWinResult winResult);

        public Server()
        {
            _server = new ProtoServer<SalvoData>(IPAddress.Any, 51111)
            {
                ReceiveBufferSize = 1024 * 10,
                SendBufferSize = 1024 * 10
            };
            _server.ClientConnected += ClientConnected;
            _server.ClientDisconnected += _server_ClientDisconnected;
            _server.ReceivedMessage += ServerMessageReceived;
        }



        public void StartServer()
        {
            Console.WriteLine("Starting Server...");
            _server.Start();
            Console.WriteLine("Server started!");
        }

        public void StopServer()
        {
            _server?.Stop();
        }

        private void ServerMessageReceived(IPEndPoint sender, SalvoData data)
        {
            Console.WriteLine(data.Message);
            if (data.IsAttack)
                Attack(data.Attack);

            if (data.IsAttackResult)
                AttackResult(data.AttackResult);

            if (data.IsWinResult)
                WinResult(data.SalvoWinResult);
        }

        public void SendAttackResult(SalvoData result)
        {
            _server?.Broadcast(result);
        }

        public void LaunchAttack(SalvoData data)
        {
            _server?.Broadcast(data);
        }

        public void SendWinResult(SalvoData data)
        {
            _server?.Broadcast(data);
        }
        private void ClientConnected(IPEndPoint address)
        {
            Console.WriteLine("Client connected");
        }
        private void _server_ClientDisconnected(IPEndPoint endPoint)
        {
            StopServer();
            MessageBox.Show("Parterner disconnected. Game over!");
            
            Environment.Exit(0); // Force close to prevent close confirmation
        }
    }

}
