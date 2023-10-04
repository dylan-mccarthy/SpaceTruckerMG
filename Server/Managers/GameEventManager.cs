//Class for storing game events and then handling them in the game loop
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpaceTrucker.Shared;
using SpaceTrucker.Server.Models;
using SpaceTrucker.Shared.Models;

namespace SpaceTrucker.Server.Managers
{
    public class GameEventManager
    {
        Queue<GameEvent> gameEvents = new Queue<GameEvent>();
        public GameEventManager()
        {

        }

        public void AddGameEvent(GameEvent gameEvent)
        {
            gameEvents.Enqueue(gameEvent);
        }

        public Queue<GameEvent> GetNextGameEvents()
        {
            //Create a clone of the current game events and then empty the Queue
            Queue<GameEvent> currentGameEvents = new Queue<GameEvent>(gameEvents);
            gameEvents.Clear();
            //Return the cloned game events
            return currentGameEvents;
        }
    }
}