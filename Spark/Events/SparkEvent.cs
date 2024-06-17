using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Events;
public interface IEvent
{
    bool Handled { get; set; }
    int Type { get; }
}

public class Event : IEvent
{
    public Event()
    {
        Type = EventType.NoEvent;
    }

    public Event(int type)
    {
        Type = type;
    }

    public bool Handled { get; set; }
    public int Type { get; }
}