using Spark.ECS.EntityCore;
using System.Collections.Generic;

namespace Spark.ECS.ComponentCore;
internal class ComponentManager
{
    private Dictionary<Type, Dictionary<string, IComponent>> _components = new Dictionary<Type, Dictionary<string, IComponent>>();

    public void AddComponent<T>(string componentName, T component) where T : IComponent
    {
        Type type = typeof(T);
        if (!_components.ContainsKey(type))
        {
            _components[type] = new Dictionary<string, IComponent>();
        }

        _components[type][componentName] = component;
    }

    public T GetComponent<T>(string componentName) where T : IComponent
    {
        Type type = typeof(T);
        return (T)_components[type][componentName];
    }

    public List<IComponent> GetComponentList<T>() where T : IComponent
    {
        Type type = typeof(T);
        return new List<IComponent>(_components[type].Values);
    }

    public void RemoveComponent<T>(string componentName) where T : IComponent
    {
        Type type = typeof(T);
        if (_components.ContainsKey(type))
        {
            _components[type].Remove(componentName);
        }
    }
}