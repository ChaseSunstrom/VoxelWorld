namespace Spark.Events;

public interface IObserver<T> where T : IEvent
{
    void OnEvent(T @event);
}
