namespace Spark.ECS.SystemCore;
internal class SystemManager
{
    private Dictionary<Type, IUpdateSystem?> _updateSystems = new();
    private Dictionary<Type, IShutdownSystem?> _shutdownSystems = new();
    private Dictionary<Type, IStartSystem?> _startSystems = new();

    public void AddSystem(ISystem system)
    {
        Type systemType = system.GetType();

        if (system is IUpdateSystem updateSystem)
        {
            _updateSystems[systemType] = updateSystem;
        }

        if (system is IShutdownSystem shutdownSystem)
        {
            _shutdownSystems[systemType] = shutdownSystem;
        }

        if (system is IStartSystem startSystem)
        {
            _startSystems[systemType] = startSystem;
        }
    }

    public void RemoveSystem<T>() where T : ISystem
    {
        Type systemType = typeof(T);

        if (_updateSystems.ContainsKey(systemType))
        {
            _updateSystems[systemType] = null;
        }

        if (_shutdownSystems.ContainsKey(systemType))
        {
            _shutdownSystems[systemType] = null;
        }

        if (_startSystems.ContainsKey(systemType))
        {
            _startSystems[systemType] = null;
        }
    }

    public T? GetSystem<T>() where T : ISystem
    {
        return (T?)_updateSystems[typeof(T)] ?? (T?)_shutdownSystems[typeof(T)] ?? (T?)_startSystems[typeof(T)];
    }

    public void OnUpdate(float deltaTime)
    {
        if (deltaTime == -1)
        {
            foreach (var system in _updateSystems.Values)
            {
                system?.OnUpdate();
            }
        }
    }

    public void OnStart()
    {
        foreach (var system in _startSystems.Values)
        {
            system?.OnStart();
        }
    }

    public void OnShutdown()
    {
        foreach (var system in _shutdownSystems.Values)
        {
            system?.OnShutdown();
        }
    }
}