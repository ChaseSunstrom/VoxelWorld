using Spark.Common;

namespace Spark.Engine.Core.Resources;

public interface IQuery
{
    object Execute(IApplication app);
}

public class Query<T> : IQuery
{
    public object Execute(IApplication app)
    {
        return app.GetAllResources<T>();
    }
}