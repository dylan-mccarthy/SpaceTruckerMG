//Client class that holds information about the client that is connecting to the server
using SpaceTrucker.Shared;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using SpaceTrucker.Shared.Models;

namespace SpaceTrucker.Server.Models
{
    public class GameClient
    {
        public int Id {get; set;}
        public Player? Player {get; set;}

        public TcpClient TcpClient;

        public GameClient(TcpClient tcpClient)
        {
            this.TcpClient = tcpClient;
        }

        public string Receive()
        {
            //Receive a message from the client
            byte[] buffer = new byte[1024];
            int bytesRead = TcpClient.GetStream().Read(buffer, 0, buffer.Length);
            //Convert the message to a string
            string message = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);
            //Return string
            return message;
        }

        internal void Send(GameEvent gameEvent)
        {
            //Send message to Client
            string message = JsonSerializer.Serialize(gameEvent);
            byte[] buffer = System.Text.Encoding.ASCII.GetBytes(message);
            TcpClient.GetStream().Write(buffer, 0, buffer.Length);
        }
    }
}