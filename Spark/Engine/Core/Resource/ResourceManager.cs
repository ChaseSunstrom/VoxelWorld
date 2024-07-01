namespace Spark.Engine.Core.Resources;

public class ResourceManager
{
    private readonly Dictionary<Type, Dictionary<string, object>> _resources = new();

    public void AddResource<T>(T resource, string name)
    {
        var type = typeof(T);
        if (!_resources.ContainsKey(type))
        {
            _resources[type] = new Dictionary<string, object>();
        }
        _resources[type][name] = resource;
    }

    public bool HasResource<T>(string name)
    {
        var type = typeof(T);
        return _resources.ContainsKey(type) && _resources[type].ContainsKey(name);
    }

    public T? GetResource<T>(string name)
    {
        var type = typeof(T);
        if (_resources.ContainsKey(type) && _resources[type].ContainsKey(name))
        {
            return (T)_resources[type][name];
        }
        return default;
    }
    
    public T GetResourceOrDefault<T>(string name, T defaultValue)
    {
        var type = typeof(T);
        if (_resources.ContainsKey(type) && _resources[type].ContainsKey(name))
        {
            return (T)_resources[type][name];
        }
        return defaultValue;
    }

    public IEnumerable<T> GetAllResources<T>()
    {
        var type = typeof(T);
        if (_resources.ContainsKey(type))
        {
            return _resources[type].Values.Cast<T>();
        }
        return Enumerable.Empty<T>();
    }

    public void RemoveResource<T>(string name)
    {
        var type = typeof(T);
        if (_resources.ContainsKey(type) && _resources[type].ContainsKey(name))
        {
            _resources[type].Remove(name);
        }
    }
}
