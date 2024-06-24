using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Engine.Core.Events;

public class Event : IEvent
{
    public bool Handled { get; set; } = false;
    public int Type { get; } = EventType.NoEvent;
    public Event(int type) => Type = type;
}