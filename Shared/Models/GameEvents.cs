//Class for describing game events
using System;
using System.Collections.Generic;

namespace SpaceTrucker.Shared.Models
{
    public class GameEvent
    {
        public Guid Guid {get; set;}
        public GameEventType EventType {get; set;}
        public List<object> Parameters {get; set;}
        public GameEvent()
        {
            Parameters = new List<object>();
        }

        public GameEvent(GameEventType eventType)
        {
            Parameters = new List<object>();
            this.EventType = eventType;
        }

        public GameEvent(GameEventType eventType, List<object> parameters)
        {
            this.EventType = eventType;
            this.Parameters = parameters;
        }
    }
}