namespace Spark.Common;

public interface IApplication
{
    public void Run();
    public void Update();
    public IEnumerable<T> GetAllResources<T>();
}