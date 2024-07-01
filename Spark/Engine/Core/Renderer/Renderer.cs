using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Engine.Core.Renderer;

public class Renderer : IRenderer
{
    public GraphicsBackend Backend { get; }
    
    public Renderer(GraphicsBackend backend)
    {
        Backend = backend;
    }
} 