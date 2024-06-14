using Spark.ECS.ComponentCore;
using System.Collections.Generic;

namespace Spark.ECS.EntityCore;

public class Entity
{
    public string Id { get; }
    private Dictionary<Type, Dictionary<string, IComponent>> _components = new();

    internal Entity(string id)
    {
        Id = id;
    }

    internal Entity(string id, Dictionary<Type, Dictionary<string, IComponent>> components) : this(id)
    {
        _components = components;
    }

    public Dictionary<string, T> GetComponents<T>() where T : IComponent
    {
        Type type = typeof(T);
        if (_components.TryGetValue(type, out var componentDict))
        {
            return componentDict.ToDictionary(x => x.Key, x => (T)x.Value);
        }
        return new Dictionary<string, T>();
    }

    public T? GetComponent<T>(string componentName) where T : class, IComponent
    {
        Type type = typeof(T);
        if (_components.TryGetValue(type, out var componentDict))
        {
            if (componentDict.TryGetValue(componentName, out var component))
            {
                return (T)component;
            }
        }
        return null;
    }

    internal void AddComponent<T>(string componentName, T component) where T : class, IComponent
    {
        Type type = typeof(T);
        if (!_components.ContainsKey(type))
        {
            _components[type] = new Dictionary<string, IComponent>();
        }
        _components[type][componentName] = component;
    }

    internal void RemoveComponent<T>(string componentName) where T : class, IComponent
    {
        Type type = typeof(T);
        if (_components.TryGetValue(type, out var componentDict))
        {
            componentDict.Remove(componentName);
            if (componentDict.Count == 0)
            {
                _components.Remove(type);
            }
        }
    }

    internal void RemoveComponents<T>() where T : class, IComponent
    {
        Type type = typeof(T);
        _components.Remove(type);
    }
}