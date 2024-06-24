namespace Spark.Engine.Core.Ecs.EntityNS;

internal class EntityManager
{
    private Dictionary<string, Entity> _entities = new();
    private Stack<int> _oldIds = new();
    private int _nextEntityId = 0;

    public Entity CreateEntity()
    {
        if (_oldIds.Count > 0)
        {
            string entityId = $"entity_{_oldIds.Pop()}";
            Entity entity = new Entity(entityId);
            _entities[entityId] = entity;
            return entity;
        }
        else
        {
            string entityId = $"entity_{_nextEntityId++}";
            Entity entity = new Entity(entityId);
            _entities[entityId] = entity;
            return entity;
        }
    }

    public void DestroyEntity(string entityId)
    {
        _entities.Remove(entityId);
        int id = int.Parse(entityId.Split('_')[1]);
        _oldIds.Push(id);
    }

    public Entity GetEntity(string entityId)
    {
        return _entities[entityId];
    }
}