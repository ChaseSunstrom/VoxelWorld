namespace Spark.Events;

public static partial class EventType
{
    public const int NoEvent = 0;
    public const int WindowClosed = 1;
    public const int WindowResized = 2;
    public const int WindowMoved = 3;
    public const int KeyPressed = 4;
    public const int KeyReleased = 5;
    public const int KeyRepeated = 6;
    public const int MousePressed = 7;
    public const int MouseReleased = 8;
    public const int MouseMoved = 9;
    public const int MouseScrolled = 10;
    public const int EntityRemovedFromECS = 11;
    public const int EntityAddedToECS = 12;
    public const int ComponentAddedToEntity = 13;
    public const int ComponentRemovedFromEntity = 14;
}