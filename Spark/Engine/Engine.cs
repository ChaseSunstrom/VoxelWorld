using Spark.Engine.Core.Ecs;
using Spark.Engine.Core.Resources;
using Spark.Common.Util;

namespace Spark.Engine;
public class Engine
{
    private CancellationToken _ct;
    
    private ResourceManager _resourceManager = new();
    public Ecs Ecs { get; } = new();

    public Engine(CancellationToken ct)
    {
        _ct = ct;
    }

    public void Initialize()
    {
        Ecs.OnStart();
    }
    
    public void Update()
    {
        Ecs.OnUpdate();
    }
    
    public void Shutdown()
    {
        Ecs.OnShutdown();
    }
    public void AddResource<T>(T resource, string name)
    {
        _resourceManager.AddResource(resource, name);
    }

    public void AddResources<T>(params (T, string)[] resources)
    {
        foreach (var (type, name) in resources)
            _resourceManager.AddResource(type, name);
    }
    
    public bool HasResource<T>(string name) => _resourceManager.HasResource<T>(name);

    public T? GetResource<T>(string name) => _resourceManager.GetResource<T>(name);

    public IEnumerable<T> GetAllResources<T>() => _resourceManager.GetAllResources<T>();
    
    public void RemoveResource<T>(string name) => _resourceManager.RemoveResource<T>(name);
}
