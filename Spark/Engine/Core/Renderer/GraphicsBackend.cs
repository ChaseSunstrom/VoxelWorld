using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Engine.Core.Renderer;

public enum GraphicsBackend : byte
{
    /// <summary>
    /// Direct3D 11.
    /// </summary>
    Direct3D11,
    /// <summary>
    /// Vulkan.
    /// </summary>
    Vulkan,
    /// <summary>
    /// OpenGL.
    /// </summary>
    OpenGL,
    /// <summary>
    /// Metal.
    /// </summary>
    Metal,
    /// <summary>
    /// OpenGL ES.
    /// </summary>
    OpenGLES,
}