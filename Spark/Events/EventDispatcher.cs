namespace Spark.Events;

public class EventDispatcher
{
    private readonly IEvent _event;

    public EventDispatcher(IEvent @event)
    {
        _event = @event;
    }

    public bool Dispatch(Func<IEvent, bool> func)
    {
        return func(_event);
    }
}