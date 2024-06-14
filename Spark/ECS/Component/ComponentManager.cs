using Spark.ECS.EntityCore;
using System.Collections.Generic;

namespace Spark.ECS.ComponentCore;
internal class ComponentManager
{
    private Dictionary<Type, Dictionary<Entity, Dictionary<string, IComponent>>> _components = new();

    public void AddComponent<T>(Entity entity, string componentName, T component) where T : IComponent
    {
        Type type = typeof(T);
        if (!_components.ContainsKey(type))
        {
            _components[type] = new Dictionary<Entity, Dictionary<string, IComponent>>();
        }

        if (!_components[type].ContainsKey(entity))
        {
            _components[type][entity] = new Dictionary<string, IComponent>();
        }

        _components[type][entity][componentName] = component;
    }

    public T GetComponent<T>(Entity entity, string componentName) where T : IComponent
    {
        Type type = typeof(T);
        return (T)_components[type][entity][componentName];
    }

    public List<T> GetComponentsOfType<T>() where T : IComponent
    {
        var type = typeof(T);
        var result = new List<T>();
        if (_components.ContainsKey(type))
        {
            foreach (var entityComponents in _components[type].Values)
            {
                foreach (var component in entityComponents.Values)
                {
                    result.Add((T)component);
                }
            }
        }
        return result;
    }

    public void RemoveComponent<T>(Entity entity, string componentName) where T : IComponent
    {
        var type = typeof(T);
        if (_components.ContainsKey(type) && _components[type].ContainsKey(entity))
        {
            _components[type][entity].Remove(componentName);
            if (_components[type][entity].Count == 0)
            {
                _components[type].Remove(entity);
            }
            if (_components[type].Count == 0)
            {
                _components.Remove(type);
            }
        }
    }
}