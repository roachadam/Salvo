﻿
using GenericProtocol.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            _server.ReceivedMessage += ServerMessageReceived;
        }

        public void StartServer()
        {
            Console.WriteLine("Starting Server...");
            _server.Start();
            Console.WriteLine("Server started!");
        }

        private void ServerMessageReceived(IPEndPoint sender, SalvoData data)
        {
            Console.WriteLine(data.Message);
            if (data.IsAttack)
                Attack(data.Attack);

            if (data.IsAttackResult)
                AttackResult(data.AttackResult);

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

    }

}
