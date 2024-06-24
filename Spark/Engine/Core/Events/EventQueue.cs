using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Engine.Core.Events;

public class EventQueue
{
    private readonly Queue<Event> _queuedEvents = new();

    public void QueueEvent<T>(T @event) where T : Event
    {
        _queuedEvents.Enqueue(@event);
    }

    public void ProcessEvents(EventBus eventBus)
    {
        while (_queuedEvents.Count > 0)
        {
            var @event = _queuedEvents.Dequeue();
            eventBus.Publish(@event);
        }
    }
}