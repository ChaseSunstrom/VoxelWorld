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

public interface IObserver<T> where T : IEvent
{
    void OnEvent(T @event);
}

public static class EventSubject
{
    private static readonly Dictionary<Type, List<object>> _observers = new Dictionary<Type, List<object>>();
    private static readonly List<object> _allObservers = new List<object>();

    public static void Subscribe<T>(Action<T> callback) where T : IEvent
    {
        var observer = new TypedObserver<T>(callback);
        var type = typeof(T);

        if (!_observers.ContainsKey(type))
            _observers[type] = new List<object>();

        _observers[type].Add(observer);
        _allObservers.Add(observer);
    }

    public static void Unsubscribe<T>(Action<T> callback) where T : IEvent
    {
        var type = typeof(T);
        var observer = _observers[type].OfType<TypedObserver<T>>().FirstOrDefault(o => o.Callback == callback);

        if (observer != null)
        {
            _observers[type].Remove(observer);
            _allObservers.Remove(observer);
        }
    }

    public static void Publish<T>(T @event) where T : IEvent
    {
        var type = typeof(T);
        if (_observers.ContainsKey(type))
        {
            var observers = _observers[type];
            foreach (var observer in observers.OfType<TypedObserver<T>>())
            {
                observer.OnEvent(@event);
            }
        }

        foreach (var observer in _allObservers.OfType<IObserver<IEvent>>())
        {
            observer.OnEvent(@event);
        }
    }

    private class TypedObserver<T> : IObserver<T> where T : IEvent
    {
        public TypedObserver(Action<T> callback)
        {
            Callback = callback;
        }

        public Action<T> Callback { get; }

        public void OnEvent(T @event)
        {
            Callback(@event);
        }
    }
}

public class WindowClosedEvent : Event
{
    public WindowClosedEvent() : base(EventType.WindowClosed)
    {
    }
}

public class WindowResizedEvent : Event
{
    public WindowResizedEvent(int width, int height) : base(EventType.WindowResized)
    {
        Width = width;
        Height = height;
    }

    public int Width { get; }
    public int Height { get; }
}

public class WindowMovedEvent : Event
{
    public WindowMovedEvent(int xPos, int yPos) : base(EventType.WindowMoved)
    {
        XPos = xPos;
        YPos = yPos;
    }

    public int XPos { get; }
    public int YPos { get; }
}

public class KeyPressedEvent : Event
{
    public KeyPressedEvent(int keyCode) : base(EventType.KeyPressed)
    {
        KeyCode = keyCode;
    }

    public int KeyCode { get; }
}

public class KeyReleasedEvent : Event
{
    public KeyReleasedEvent(int keyCode) : base(EventType.KeyReleased)
    {
        KeyCode = keyCode;
    }

    public int KeyCode { get; }
}

public class KeyRepeatedEvent : Event
{
    public KeyRepeatedEvent(int keyCode) : base(EventType.KeyRepeated)
    {
        KeyCode = keyCode;
    }

    public int KeyCode { get; }
}

public class MousePressedEvent : Event
{
    public MousePressedEvent(int button) : base(EventType.MousePressed)
    {
        Button = button;
    }

    public int Button { get; }
}

public class MouseReleasedEvent : Event
{
    public MouseReleasedEvent(int button) : base(EventType.MouseReleased)
    {
        Button = button;
    }

    public int Button { get; }
}

public class MouseMovedEvent : Event
{
    public MouseMovedEvent(double xPos, double yPos) : base(EventType.MouseMoved)
    {
        XPos = xPos;
        YPos = yPos;
    }

    public double XPos { get; }
    public double YPos { get; }
}

public class MouseScrolledEvent : Event
{
    public MouseScrolledEvent(double xOffset, double yOffset) : base(EventType.MouseScrolled)
    {
        XOffset = xOffset;
        YOffset = yOffset;
    }

    public double XOffset { get; }
    public double YOffset { get; }
}