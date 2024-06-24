using System.Text.Json;

namespace Spark.Engine.Core.Serialization;

public static class Serializer
{
    public static string Serialize(object obj)
    {
        return JsonSerializer.Serialize(obj);
    }

    public static T? Deserialize<T>(string data)
    {
        return JsonSerializer.Deserialize<T>(data);
    }
}
