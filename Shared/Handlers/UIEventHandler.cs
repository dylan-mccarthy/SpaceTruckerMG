using SpaceTrucker.Shared.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrucker.Shared.Handlers
{
    public class UIEventHandler : IEventHandler
    {
        public Dictionary<string, EventHandler> Events = new Dictionary<string, EventHandler>();

        public void RegisterNewEvent(string name, EventHandler eventHandler)
        {
            Events.Add(name, eventHandler);
        }

        public void DeregisterEvent(string name)
        {
            Events.Remove(name);
        }

        public void Connect(string name, object sender, EventArgs args)
        {
            var handler = Events[name];
            handler(sender, args);
        }
    }
}
