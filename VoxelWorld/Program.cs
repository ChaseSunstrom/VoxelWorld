using Spark.Engine.Core.Ecs;
using Spark.Engine.Core.Ecs.EntityNS;
using Spark.Common;
using Spark.Application.Core;
using Spark.Application.Core.WindowNS;

public struct TestResource
{
    string _name = "TEST";

    TestResource(string name)
    {
        _name = name;
    }

    public void PrintName()
    {
        Console.WriteLine("TESTTESTEST");
    }
}

class Program
{
    static void StartupFunction(Application app, params object[] objects)
    {
        Console.WriteLine("Hello on startup function!");
    }

    static void UpdateFunction1(Application app, float deltaTime, params object[] objects)
    {
        Console.WriteLine("Hello from update function 1!");
    }

    static void UpdateFunction2(Application app, float deltaTime, params object[] objects)
    {
        Console.WriteLine("Hello from update function 2!");
        app.GetResource<TestResource>("TestResource").PrintName();
    }

    public static void Main()
    {
        Application app = new(new WindowData(1000, 1000, "Hi"), CancellationState.Token);

        app.AddResource(new TestResource(), "TestResource")
           .AddStartupFunction(StartupFunction)
           .AddUpdateFunction(UpdateFunction1)
           .AddUpdateFunction(UpdateFunction2)
           .SetTimeStep(100)
           .Initialize()
           .Run();
    }
}