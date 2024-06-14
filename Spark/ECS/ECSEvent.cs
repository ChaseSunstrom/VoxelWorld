using Spark.Events;
using Spark.ECS.EntityCore;
using Spark.ECS.ComponentCore;
using Spark.ECS.SystemCore;

namespace Spark.ECS;

public class EntityAddedToECSEvent : Event
{
    public Entity Entity { get; }

    public EntityAddedToECSEvent(Entity entity) : base(EventType.EntityAddedToECS)
    {
        Entity = entity;
    }
}

public class EntityRemovedFromECSEvent : Event
{
    public Entity Entity { get; }

    public EntityRemovedFromECSEvent(Entity entity) : base(EventType.EntityRemovedFromECS)
    {
        Entity = entity;
    }
}

public class ComponentAddedToEntityEvent : Event
{
    public Entity Entity { get; }
    public object Component { get; }

    public ComponentAddedToEntityEvent(Entity entity, object component) : base(EventType.ComponentAddedToEntity)
    {
        Entity = entity;
        Component = component;
    }
}

public class ComponentRemovedFromEntityEvent : Event
{
    public Entity Entity { get; }
    public object Component { get; }

    public ComponentRemovedFromEntityEvent(Entity entity, object component) : base(EventType.ComponentRemovedFromEntity)
    {
        Entity = entity;
        Component = component;
    }
}