//Space Truckers Game Server

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using SpaceTrucker.Shared;
using SpaceTrucker.Server.Models;
using SpaceTrucker.Server.Managers;
using System.Diagnostics;
using SpaceTrucker.Shared.Models;

namespace SpaceTrucker.Server
{
    //TCP based Server that handles all the game logic
    public class Server
    {
        public int port = 7777;

        private ClientManager _clientManager;
        private GameEventManager _gameEventManager;
        
        public Server()
        {
            _clientManager = new ClientManager();
            _gameEventManager = new GameEventManager();
        }

        public void Start()
        {
            Console.WriteLine("Starting server...");
            //Start the server on a new thread
            Thread thread = new Thread(new ThreadStart(StartServer));
            thread.Start();
        }

        private void StartServer()
        {
            //Start the server on the specified port
            TcpListener server = new TcpListener(IPAddress.Any, port);
            server.Start();
            Console.WriteLine("Server started on port " + port);
            //Start the game loop on new thread
            Thread thread = new Thread(new ThreadStart(GameLoop));
            //Listen for new clients
            while (true)
            {
                //Accept the client
                TcpClient client = server.AcceptTcpClient();
                //Create a new client object
                GameClient newClient = new GameClient(client);
                //Add the client to the list of clients
                _clientManager.AddClient(newClient);
            }
        }

        private void GameLoop()
        {
            //Game loop to run every 200ms
            while (true)
            {
                //Handle all the game events
                HandleGameEvents();
                //Send the game state to all the clients
                SendGameState();
                //Wait 200ms
                Thread.Sleep(200);
            }
        }

        private void HandleGameEvents()
        {
            var eventsToProcess = _gameEventManager.GetNextGameEvents();
            //Handle all the game events
            while(eventsToProcess.Count > 0)
            {
                var e = eventsToProcess.Dequeue();
                //Handle Game Event
                switch(e.EventType)
                {
                    case GameEventType.PlayerBuy:
                        //Handle Player Buy Even
                    case GameEventType.PlayerSell:
                        //Handle Player Sell Event
                    case GameEventType.ShipDock:
                        //Handle Ship Dock Event
                    case GameEventType.ShipUndock:
                        //Handle Ship Undock Event  
                    case GameEventType.ShipMove:
                        //Handle Ship Move Event
                    default:
                        //Invalid Game Event
                        Console.WriteLine("Invalid Game Event");
                        break;
                }
            }
        }

        private void SendGameState(){

        }
    }
}