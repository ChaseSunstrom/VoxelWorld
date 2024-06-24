using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Engine.Core.Events;
internal interface IEvent
{
    bool Handled { get; set; } 
    int Type { get; }
}