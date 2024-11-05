
using System.Text.Json;

namespace DopravniPodnik.Data.service;

public static class CopyUtilities
{
    public static T DeepClone<T>(this T obj)
    {
        var json = JsonSerializer.Serialize(obj);
        return JsonSerializer.Deserialize<T>(json);
    }
    
    
}