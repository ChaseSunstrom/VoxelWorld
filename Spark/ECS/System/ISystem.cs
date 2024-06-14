namespace Spark.ECS.SystemCore;
public interface ISystem
{
    public int SystemId { get; internal set; }
    public int Priority { get; set; }
}