using Spark.Engine.Core.Ecs;
using Spark.Engine.Core.Ecs.Entities;
using Spark.Common;
using Spark.Common.Util;
using Spark.Application.Core;
using Spark.Application.Core.WindowNS;
using Spark.Common.Util;

public struct TestResource
{
    string _name;

    public TestResource(string name)
    {
        _name = name;
    }

    public void PrintName()
    {
        Console.WriteLine(_name);
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
        new Application(new WindowData(1000, 1000, "Voxel Game"))
            .AddStartupFunction(StartupFunction)
            .AddUpdateFunction(UpdateFunction1)
            .AddUpdateFunction(UpdateFunction2)
            .SetTimeStep(100)
            .Initialize()
            .Run();
    }
}