namespace Spark.Engine.Core.Events;

internal class EventDispatcher
{
    private readonly IEvent _event;

    internal EventDispatcher(IEvent @event)
    {
        _event = @event;
    }

    internal bool Dispatch(Func<IEvent, bool> func)
    {
        return func(_event);
    }
}