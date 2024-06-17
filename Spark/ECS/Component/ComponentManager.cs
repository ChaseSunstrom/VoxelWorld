using Spark.ECS.EntityCore;
using System.Collections.Generic;

namespace Spark.ECS.ComponentCore;
internal class ComponentManager
{
    private Dictionary<(Entity entity, string componentName), IComponent> _components = new();

    public void AddComponent<T>(Entity entity, string componentName, T component) where T : class, IComponent => _components[(entity, componentName)] = component;

    public T GetComponent<T>(Entity entity, string componentName) where T : class, IComponent => (T)_components[(entity, componentName)];

    public void RemoveComponent<T>(Entity entity, string componentName) where T : class, IComponent => _components.Remove((entity, componentName));

    public List<T> GetComponentsOfType<T>() where T : class, IComponent
    {
        var result = new List<T>();

        foreach (var component in _components.Values)
            if (component is T typedComponent)
                result.Add(typedComponent);

        return result;
    }
}