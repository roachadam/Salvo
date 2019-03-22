﻿using GenericProtocol.Implementation;
using System;
using System.Net;
using pa8_c00061075.Structs;

namespace pa8_c00061075.Networking
{
    class Client
    {
        private string _serverIp;
        private static ProtoClient<SalvoData> _client;

        public event AttackHandler Attack = delegate { };
        public delegate void AttackHandler(SalvoAttack attack);

        public event AttackResultHandler AttackResult = delegate { };
        public delegate void AttackResultHandler(SalvoAttackResult attackResult);

        public event WinResultHandler WinResult = delegate { };
        public delegate void WinResultHandler(SalvoWinResult winResult);

        public Client(string ipAddress)
        {
            _serverIp = ipAddress;
            _client = new ProtoClient<SalvoData>(IPAddress.Parse(_serverIp), 51111)
            {
                AutoReconnect = true, 
                ReceiveBufferSize = 1024 * 10, 
                SendBufferSize = 1024 * 10
            };
            _client.ReceivedMessage += ClientMessageReceived;
            _client.ConnectionLost += Client_ConnectionLost;
        }

        public void StartClient()
        {
            Console.WriteLine("Connecting");
            _client.Connect().GetAwaiter().GetResult();
            Console.WriteLine("Connected!");
        }

        public void LaunchAttack(SalvoData data)
        {
            _client?.Send(data);
        }

        public void SendAttackResult(SalvoData result)
        {
            _client?.Send(result);
        }
        public void SendWinResult(SalvoData data)
        {
            _client?.Send(data);
        }
        private void Client_ConnectionLost(IPEndPoint endPoint)
        {
            Console.WriteLine($"Connection lost! {endPoint.Address}");
        }

        private void ClientMessageReceived(IPEndPoint sender, SalvoData data)
        {
            Console.WriteLine(data.Message);
            if (data.IsAttack)
                Attack(data.Attack);

            if (data.IsAttackResult)
                AttackResult(data.AttackResult);
        }
    }
}
