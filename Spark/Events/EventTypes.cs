using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Events;

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