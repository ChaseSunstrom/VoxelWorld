using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Spark.Engine.Util;

namespace Spark.Engine;
public class Engine
{
    private CancellationToken _ct;

    public Engine(CancellationToken ct)
    {
        _ct = ct;
    }
}
