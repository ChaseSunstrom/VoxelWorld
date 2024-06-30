using Spark.Application.Core.WindowNS;
using Spark.Engine.Core.Resource;
using Spark.Common;

namespace Spark.Application.Core;
public class Application
{
    public delegate void StartupFunction(Application app, params object[] args);
    public delegate void UpdateFunction(Application app, float timeStep, params object[] args);

    private readonly List<(StartupFunction, object[])> _startupFunctions = new();
    private readonly List<(UpdateFunction, object[])> _updateFunctions = new();
    private readonly CancellationToken _ct = Cancellation.Token;
    private readonly Window _window;
    private readonly ResourceManager _resourceManager = new();
    private ApplicationType _applicationType = ApplicationType.Game;
    private float _timeStep = 60;

    public Application(WindowData windowData, ApplicationType applicationType)
    {
        _window = new Window(windowData, _ct);
        _applicationType = applicationType;
    }

    public Application(WindowData windowData, ApplicationType applicationType, CancellationToken ct)
    {
        _ct = ct;
        _window = new Window(windowData, _ct);
        _applicationType = applicationType;
    }

    public Application AddStartupFunction(StartupFunction function, params object[] args)
    {
        _startupFunctions.Add((function, args));
        return this;
    }

    public Application AddUpdateFunction(UpdateFunction function, params object[] args)
    {
        _updateFunctions.Add((function, args));
        return this;
    }
    public Application AddResource<T>(T resource, string name)
    {
        _resourceManager.AddResource(resource, name);
        return this;
    }
    public bool HasResource<T>(string name) => _resourceManager.HasResource<T>(name);

    public T? GetResource<T>(string name) => _resourceManager.GetResource<T>(name);
    
    public void RemoveResource<T>(string name) => _resourceManager.RemoveResource<T>(name);

    public Application SetTimeStep(float timeStep)
    {
        _timeStep = timeStep;
        return this;
    }

    public Application Initialize()
    {
        _window.Initialize();

        // Execute all startup functions
        foreach (var (function, args) in _startupFunctions)
        {
            function(this, args);
        }

        return this;
    }

    public void Run()
    {
        while (_window.Running())
        {
            // Execute all update functions
            foreach (var (function, args) in _updateFunctions)
            {
                function(this, _timeStep, args);
            }

            // Update and render the window
            //_window.Update(deltaTime);
            //_window.Render();
        }
    }
}