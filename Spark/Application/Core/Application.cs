using Spark.Application.Core.WindowNS;
using Spark.Engine.Core.Resources;
using Spark.Common;
using Spark.Engine;

namespace Spark.Application.Core;
public class Application : IApplication
{
    public delegate void StartupFunction(Application app, params object[] args);
    public delegate void UpdateFunction(Application app, float timeStep, params object[] args);

    private readonly List<(StartupFunction, object[])> _startupFunctions = new();
    private readonly List<(UpdateFunction, object[])> _updateFunctions = new();
    private readonly CancellationToken _ct = Cancellation.Token;
    private readonly Window _window;
    private readonly Engine.Engine _engine;
    private ApplicationType _applicationType = ApplicationType.Game;
    private float _timeStep = 60;

    public Application(WindowData windowData)
    {
        _window = new Window(windowData, _ct);
        _engine = new Engine.Engine(_ct);
    }
    
    public Application(WindowData windowData, ApplicationType applicationType)
    {
        _applicationType = applicationType;
        _window = new Window(windowData, _ct);
        _engine = new Engine.Engine(_ct);
    }

    public Application(WindowData windowData, ApplicationType applicationType, CancellationToken ct)
    {
        _ct = ct;
        _applicationType = applicationType;
        _window = new Window(windowData, _ct);
        _engine = new Engine.Engine(_ct);
    }

    public Application AddStartupFunction(StartupFunction function, params object[] args)
    {
        _startupFunctions.Add((function, args));
        return this;
    }

    public Application AddStartupFunctions(params (StartupFunction, object[])[] functions)
    {
        foreach(var (function, args) in functions)
            _startupFunctions.Add((function, args));

        return this;
    }

    public Application AddUpdateFunction(UpdateFunction function, params object[] args)
    {
        _updateFunctions.Add((function, args));
        return this;
    }

    public Application AddUpdateFunctions(params (UpdateFunction, object[])[] functions)
    {
        foreach(var (function, args) in functions)
            _updateFunctions.Add((function, args));

        return this;
    }

    public Application AddResource<T>(T resource, string name)
    {
        _engine.AddResource(resource, name);
        return this;
    }

    public Application AddResources<T>(params (T, string)[] resources)
    {
        foreach (var (type, name) in resources)
            _engine.AddResource(type, name);

        return this;
    }
    
    public Application SetApplicationType(ApplicationType applicationType)
    {
        _applicationType = applicationType;
        return this;
    } 
    
    public Application SetTimeStep(float timeStep)
    {
        _timeStep = timeStep;
        return this;
    }

    public Application Initialize()
    {
        if (_applicationType == ApplicationType.Game)
            _window.Initialize();

        // Execute all startup functions
        foreach (var (function, args) in _startupFunctions)
        {
            function(this, args);
        }
        
        _engine.Initialize();

        return this;
    }

    public bool HasResource<T>(string name) => _engine.HasResource<T>(name);

    public T? GetResource<T>(string name) => _engine.GetResource<T>(name);

    public IEnumerable<T> GetAllResources<T>() => _engine.GetAllResources<T>();
    
    public void RemoveResource<T>(string name) => _engine.RemoveResource<T>(name);
    
    public void Run()
    {
        while (_window.Running() || _applicationType == ApplicationType.Headless)
        {
            Update();
        }
    }

    public void Update()
    {
        // Execute all update functions
        foreach (var (function, args) in _updateFunctions)
        {
            function(this, _timeStep, args);
        }
        
        _engine.Update();
    }
    
    private void ExecuteFunction(Delegate function, params object[] args)
    {
        var actualArgs = args.Select(arg => arg is IQuery query ? query.Execute(this) : arg).ToArray();
        function.DynamicInvoke(new object[] { this }.Concat(actualArgs).ToArray());
    }
}