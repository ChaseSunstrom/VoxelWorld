namespace Spark.Engine.Core.Ecs.Systems;
public interface ISystem
{
    public int SystemId { get; internal set; }
    public int Priority { get; set; }
}