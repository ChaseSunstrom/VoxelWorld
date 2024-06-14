namespace Spark.ECS.SystemCore;
internal class SystemManager
{
    private SystemIdManager _systemIdManager = new SystemIdManager();
    private Dictionary<Type, IUpdateSystem?> _updateSystems = new Dictionary<Type, IUpdateSystem?>();
    private Dictionary<Type, IShutdownSystem?> _shutdownSystems = new Dictionary<Type, IShutdownSystem?>();
    private Dictionary<Type, IStartSystem?> _startSystems = new Dictionary<Type, IStartSystem?>();

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

    private class SystemIdManager
    {
        private Dictionary<string, string> _nameToId = new Dictionary<string, string>();
        private Dictionary<string, string> _idToName = new Dictionary<string, string>();
        private int _nextId = 0;

        public string GetId(string name)
        {
            if (!_nameToId.ContainsKey(name))
            {
                string id = _nextId.ToString();
                _nameToId[name] = id;
                _idToName[id] = name;
                _nextId++;
            }

            return _nameToId[name];
        }

        public string DisposeId(string name)
        {
            if (_nameToId.ContainsKey(name))
            {
                string id = _nameToId[name];
                _nameToId.Remove(name);
                _idToName.Remove(id);
                return id;
            }

            return string.Empty;
        }
    }
}