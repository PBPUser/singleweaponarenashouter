using System;
using System.Reflection;
using System.Text.Json.Serialization;

public class InfoValue
{
    [JsonInclude]
    public string Field;
    [JsonInclude]
    public string Value;
    public T ConvertValueTo<T>()
    {
        return (T)ConvertValueTo(typeof(T));
    }

    public object ConvertValueTo(Type genericType)
    {
        if (genericType == typeof(String))
            return (object)Value;
        else if (genericType == typeof(int))
            return (object)int.Parse(Value);
        else if (genericType == typeof(double))
            return (object)double.Parse(Value);
        else if (genericType == typeof(float))
            return (object)float.Parse(Value);
        else if (genericType == typeof(long))
            return (object)long.Parse(Value);
        throw new NotImplementedException($"Convertion from String to {genericType} is not implemented");
    }
}
