using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Net;

public enum ServerType
{
    TCP,
    UDP
}

public class Server
{
    public ServerType Type { get; }
    public string Address { get; }
    public int Port { get; }

    public Server(ServerType type, string address, int port)
    {
        Type = type;
        Address = address;
        Port = port;
    }

    public void Start()
    {
        Console.WriteLine($"Server started on {Address}:{Port}");
    }

    public void Stop()
    {
    }
}
