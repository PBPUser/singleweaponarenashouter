using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class NameAttribute : Attribute
{
    public string Name;

    public NameAttribute()
    {
        throw new NotImplementedException("NameAttribute requires name");
    }

    public NameAttribute(string name)
    {
        Name = name;
    }
}
