//Class for managing client connections
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text.Json;
using SpaceTrucker.Shared;
using SpaceTrucker.Shared.Models;
using SpaceTrucker.Server.Models;

namespace SpaceTrucker.Server.Managers
{
    public class ClientManager
    {
        public List<GameClient> Clients = new List<GameClient>();

        public ClientManager()
        {}

        public void AddClient(GameClient client)
        {
            //Add the client to the list of clients
            Clients.Add(client);
            //Set the client's id to the index of the client in the list
            client.Id = Clients.IndexOf(client);
            //Start listening for messages from the client
            Task.Factory.StartNew(() => ListenForMessages(client));
        }


        private void ListenForMessages(GameClient client)
        {
            //Listen for messages from the client
            while (true)
            {
                var message = client.Receive();
                //If the message is empty, the client has disconnected
                if (message == "")
                {
                    //Remove the client from the list of clients
                    Clients.Remove(client);
                    //Break out of the loop
                    break;
                }
                //Parse the message json into a GameEvent
                GameEvent? gameEvent = JsonSerializer.Deserialize<GameEvent>(message);

                if(gameEvent == null)
                {
                    //If the GameEvent is null, the message was not a valid GameEvent
                    Console.WriteLine("Invalid message received from client " + client.Id);
                    continue;
                }

                //Handle the GameEvent
                HandleGameEvent(client, gameEvent);
            }
        }

        private void HandleGameEvent(GameClient client, GameEvent gameEvent)
        {
            //Handle Game Event
            switch (gameEvent.EventType)
            {
                case GameEventType.ClientConnect:
                    //Send the client their id
                    client.Send(new GameEvent(GameEventType.ClientConnectAccepted, new List<object>(){client.Id}));
                    break;
                case GameEventType.PlayerUpdate:
                    //Update the player
                    Player player = (Player)gameEvent.Parameters[0];
                    client.Player = player;
                    //Send the player to all clients
                    SendToAllClients(new GameEvent(GameEventType.PlayerUpdate, new List<object>(){player}));
                    break;
                case GameEventType.PlayerConnect:
                    //Update Client with Player Details
                    client.Player = (Player)gameEvent.Parameters[0];
                    //Send the player to all clients
                    SendToAllClients(new GameEvent(GameEventType.PlayerConnect, new List<object>(){client.Player}));
                    break;
                case GameEventType.PlayerDisconnect:
                    //Check if player is null
                    if(client.Player == null)
                    {
                        //If the player is null, the client has not connected yet
                        Console.WriteLine("Player disconnected before connecting");
                        break;
                    }
                    //Send the player to all clients
                    SendToAllClients(new GameEvent(GameEventType.PlayerDisconnect, new List<object>(){client.Player}));
                    break;
                case GameEventType.PlayerList:
                    //Send the player list to the client
                    client.Send(new GameEvent(GameEventType.PlayerList, new List<object>(){Clients.Select(c => c.Player).ToList()}));
                    break;
                case GameEventType.ChatMessage:
                    //Validate Client has a player
                    if(client.Player == null)
                    {
                        //If the player is null, the client has not connected yet
                        Console.WriteLine("Player disconnected before connecting");
                        break;
                    }
                    //Send the chat message to all clients
                    SendToAllClients(new GameEvent(GameEventType.ChatMessage, new List<object>(){client.Player, gameEvent.Parameters[0]}));
                    break;
                default:
                    //If the GameEvent is not handled, log it
                    Console.WriteLine("Unhandled GameEvent received from client " + client.Id);
                    break;
            }
        }

        private void SendToAllClients(GameEvent gameEvent)
        {
            //Send a GameEvent to all clients
            foreach (GameClient client in Clients)
            {
                client.Send(gameEvent);
            }
        }
    }
}