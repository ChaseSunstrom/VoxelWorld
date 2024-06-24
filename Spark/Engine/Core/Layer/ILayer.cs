using Spark.Engine.Core.Events;

namespace Spark.Engine.Core.Layer;

public interface ILayer
{
    public void OnAttach();
    public void OnDetach();
    public void OnUpdate();
    public void OnRender();
    public void OnEvent(Event @event);
}