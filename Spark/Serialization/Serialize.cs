using System.Text.Json;

namespace Spark.Serialization;

public static class Serializer
{
    public static string SerializeObject(object obj)
    {
        return JsonSerializer.Serialize(obj);
    }

    public static T? DeserializeObject<T>(string data)
    {
        return JsonSerializer.Deserialize<T>(data);
    }
}
