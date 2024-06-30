using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Application.Core;
public enum ApplicationType
{
    /// <summary>
    /// Default application type, will have a game and renderer.
    /// </summary>
    Game,

    /// <summary>
    /// Used for running on a server, or without a window.
    /// </summary>
    Headless
}
