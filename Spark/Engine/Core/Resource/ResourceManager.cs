using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Engine.Core.Resource;

public class ResourceManager
{
    private readonly Dictionary<(Type, string), object?> _resources = new();

    public void AddResource<T>(T resource, string id)
    {
        var key = (typeof(T), id);
        _resources[key] = resource;
    }

    public T? GetResource<T>(string id)
    {
        var key = (typeof(T), id);
        if (_resources.TryGetValue(key, out var resource))
        {
            return (T?)resource;
        }

        throw new InvalidOperationException($"Resource of type {typeof(T)} with ID '{id}' not found.");
    }

    public bool HasResource<T>(string id)
    {
        var key = (typeof(T), id);
        return _resources.ContainsKey(key);
    }

    public void RemoveResource<T>(string id)
    {
        var key = (typeof(T), id);
        _resources.Remove(key);
    }
}