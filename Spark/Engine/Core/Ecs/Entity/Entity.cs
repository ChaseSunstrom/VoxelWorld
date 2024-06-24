using Spark.Engine.Core.Ecs.Component;
using System.Collections.Generic;

namespace Spark.Engine.Core.Ecs.EntityNS;

public class Entity
{
    public string Id { get; }
    private Dictionary<(Type, string), IComponent> _components = new();

    internal Entity(string id) => Id = id;

    internal Entity(string id, Dictionary<(Type, string), IComponent> components) : this(id) => _components = components;

    public Dictionary<string, T> GetComponents<T>() where T : class, IComponent => _components.Where(kvp => kvp.Key.Item1 == typeof(T)).ToDictionary(kvp => kvp.Key.Item2, kvp => (T)kvp.Value);

    public T? GetComponent<T>(string componentName) where T : class, IComponent => (T)_components.GetValueOrDefault((typeof(T), componentName), null);

    internal void AddComponent<T>(string componentName, T component) where T : class, IComponent => _components[(typeof(T), componentName)] = component;

    internal void RemoveComponent<T>(string componentName) where T : class, IComponent => _components.Remove((typeof(T), componentName));

    internal void RemoveComponents<T>() where T : class, IComponent
    {
        foreach (var key in _components.Keys.Where(k => k.Item1 == typeof(T)).ToList())
            _components.Remove(key);
    }
}