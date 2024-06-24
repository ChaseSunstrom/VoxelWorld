using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Engine.Core.Events;
public class EventBus
{
    private readonly Dictionary<Type, List<object>> _observers = new();
    private readonly List<object> _allObservers = new();

    public void Subscribe<T>(Action<T> callback) where T : Event
    {
        var observer = new TypedObserver<T>(callback);
        var type = typeof(T);

        if (!_observers.ContainsKey(type))
            _observers[type] = new List<object>();

        _observers[type].Add(observer);
        _allObservers.Add(observer);
    }

    public void Unsubscribe<T>(Action<T> callback) where T : Event
    {
        var type = typeof(T);
        if (_observers.ContainsKey(type))
        {
            var observer = _observers[type].OfType<TypedObserver<T>>().FirstOrDefault(o => o.Callback == callback);

            if (observer != null)
            {
                _observers[type].Remove(observer);
                _allObservers.Remove(observer);
            }
        }
    }

    public void Publish<T>(T @event) where T : Event
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

        foreach (var observer in _allObservers.OfType<IObserver<Event>>())
        {
            observer.OnEvent(@event);
        }
    }

    private class TypedObserver<T> : IObserver<T> where T : Event
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