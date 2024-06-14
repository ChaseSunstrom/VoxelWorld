using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Spark.ECS;
using Spark.ECS.EntityCore;
using Spark.ECS.ComponentCore;
using Spark.ECS.SystemCore;

namespace SparkTest;

class TestComponent : IComponent
{
    public int Value { get; set; } = 1;
}

public class SparkECSTest
{
    ECS _ECS = new ECS();
    [Fact]
    public void TestEntityCreate()
    {
        Entity entity = _ECS.CreateEntity();

        Assert.NotNull(entity);
        Assert.Equal("entity_0", entity.Id);
    }

    [Fact]
    public void TestAddComponent()
    {
        Entity entity = _ECS.CreateEntity();
        TestComponent component = new TestComponent();

        _ECS.AddComponent(entity, "component1", component);
        _ECS.AddComponent(entity, "component2", new TestComponent());

        Assert.Equal(2, entity.GetComponents<TestComponent>().Count);
    }

    [Fact]
    public void TestRemoveComponent()
    {
        Entity entity = _ECS.CreateEntity();
        TestComponent component = new TestComponent();

        _ECS.AddComponent(entity, "component1", component);
        _ECS.AddComponent(entity, "component2", new TestComponent());

        _ECS.RemoveComponent<TestComponent>(entity, "component1");

        Assert.Equal(1, entity.GetComponents<TestComponent>().Count);
        Assert.NotNull(entity.GetComponent<TestComponent>("component2"));
    }

    [Fact]
    public void TestEntityDestroy()
    {
        Entity entity = _ECS.CreateEntity();
        Entity entity2 = _ECS.CreateEntity();

        Assert.NotNull(entity);
        Assert.Equal("entity_0", entity.Id);

        Assert.NotNull(entity2);
        Assert.Equal("entity_1", entity2.Id);

        _ECS.DestroyEntity(entity);

        Entity entity3 = _ECS.CreateEntity();

        Assert.NotNull(entity3);
        Assert.Equal("entity_0", entity3.Id);
    }

    [Fact]
    public void TestComponentName()
    {
        var component = new TestComponent();
        var component2 = new TestComponent();
        var component3 = new TestComponent();

        Assert.NotEqual(component, component2);
        Assert.NotEqual(component2, component3);
        Assert.NotEqual(component, component3);
    }
}