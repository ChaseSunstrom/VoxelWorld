using Spark.Engine.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Engine.Core.Layer;
internal class LayerStack
{
    private List<ILayer> _layers = new();

    public void PushLayer(ILayer layer)
    {
        _layers.Add(layer);
        layer.OnAttach();
    }

    public void PopLayer(ILayer layer)
    {
        _layers.Remove(layer);
        layer.OnDetach();
    }

    public void Update()
    {
        foreach (var layer in _layers)
        {
            layer.OnUpdate();
        }
    }

    public void Render()
    {
        foreach (var layer in _layers)
        {
            layer.OnRender();
        }
    }

    public void OnEvent(Event @event)
    {
        foreach (var layer in _layers)
        {
            layer.OnEvent(@event);
        }
    }

    public void Clear()
    {
        foreach (var layer in _layers)
        {
            layer.OnDetach();
        }
        _layers.Clear();
    }
}
