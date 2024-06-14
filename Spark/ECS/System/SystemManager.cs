namespace Spark.ECS.SystemCore;
internal class SystemManager
{
    private SystemIdManager _systemIdManager = new SystemIdManager();
    private Dictionary<string, IUpdateSystem?> _updateSystems = new Dictionary<string, IUpdateSystem?>();
    private Dictionary<string, IShutdownSystem?> _shutdownSystems = new Dictionary<string, IShutdownSystem?>();
    private Dictionary<string, IStartSystem?> _startSystems = new Dictionary<string, IStartSystem?>();

    public void AddSystem(string systemName, ISystem system)
    {
        string systemId = _systemIdManager.GetId(systemName);

        if (system is IUpdateSystem updateSystem)
        {
            _updateSystems[systemId] = updateSystem;
        }

        if (system is IShutdownSystem shutdownSystem)
        {
            _shutdownSystems[systemId] = shutdownSystem;
        }

        if (system is IStartSystem startSystem)
        {
            _startSystems[systemId] = startSystem;
        }
    }

    public void RemoveSystem(string systemName)
    {
        string systemId = _systemIdManager.DisposeId(systemName);

        if (_updateSystems.ContainsKey(systemId))
        {
            _updateSystems[systemId] = null;
        }

        if (_shutdownSystems.ContainsKey(systemId))
        {
            _shutdownSystems[systemId] = null;
        }

        if (_startSystems.ContainsKey(systemId))
        {
            _startSystems[systemId] = null;
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
