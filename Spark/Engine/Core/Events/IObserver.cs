namespace Spark.Engine.Core.Events;

public interface IObserver<T> where T : Event
{
    void OnEvent(T @event);
}
