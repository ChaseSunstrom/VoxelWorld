using Spark.ECS.ComponentCore;
using Spark.ECS.EntityCore;
using Spark.ECS.SystemCore;

namespace Spark.ECS;
public class ECS
{
    private EntityManager _entityManager = new EntityManager();
    private ComponentManager _componentManager = new ComponentManager();
    private SystemManager _systemManager = new SystemManager();

    public Entity CreateEntity()
    {
        return _entityManager.CreateEntity();
    }

    public Entity CreateEntity(Dictionary<string, IComponent> components)
    {
        Entity entity = _entityManager.CreateEntity();
        AddComponents(entity, components);
        return entity;
    }

    public void DestroyEntity(Entity entity)
    {
        _entityManager.DestroyEntity(entity.Id);
    }

    public T GetComponent<T>(Entity entity, string componentName) where T : class, IComponent
    {
        return _componentManager.GetComponent<T>(entity, componentName);
    }

    public void AddComponent<T>(Entity entity, string componentName, T component) where T : class, IComponent
    {
        _componentManager.AddComponent(entity, componentName, component);
        entity.AddComponent(componentName, component);
    }

    public void AddComponents(Entity entity, Dictionary<string, IComponent> components)
    {
        foreach (var kvp in components)
        {
            _componentManager.AddComponent(entity, kvp.Key, kvp.Value);
            entity.AddComponent(kvp.Key, kvp.Value);
        }
    }

    public void RemoveComponent<T>(Entity entity, string componentName) where T : class, IComponent
    {
        _componentManager.RemoveComponent<T>(entity, componentName);
        entity.RemoveComponent<T>(componentName);
    }

    public void RemoveComponents<T>(Entity entity) where T : class, IComponent
    {
        var componentNames = entity.GetComponents<T>().Keys;
        foreach (var componentName in componentNames)
        {
            _componentManager.RemoveComponent<T>(entity, componentName);
        }
        entity.RemoveComponents<T>();
    }

    public List<T> GetComponentsOfType<T>() where T : IComponent
    {
        return _componentManager.GetComponentsOfType<T>();
    }

    public void AddSystem(ISystem system)
    {
        _systemManager.AddSystem(system);
    }

    public void RemoveSystem<T>() where T : ISystem
    {
        _systemManager.RemoveSystem<T>();
    }

    public void OnUpdate(float deltaTime = -1)
    {
        _systemManager.OnUpdate(deltaTime);
    }

    public void OnStart()
    {
        _systemManager.OnStart();
    }

    public void OnShutdown()
    {
        _systemManager.OnShutdown();
    }
}