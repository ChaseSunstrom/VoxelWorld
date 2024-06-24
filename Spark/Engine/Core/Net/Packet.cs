using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Engine.Core.Net;

public class Packet
{
    public string Type { get; set; }
    public string Version { get; set; }
    public string Data { get; set; }

    public Packet(string type, string version, string data)
    {
        Type = type;
        Version = version;
        Data = data;
    }
}

public class Envelope
{
    public string Source { get; set; }
    public string Destination { get; set; }
    public Packet Packet { get; set; }

    public Envelope(string source, string destination, Packet packet)
    {
        Source = source;
        Destination = destination;
        Packet = packet;
    }
}
