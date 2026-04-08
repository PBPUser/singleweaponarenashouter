using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class SceneAttribute : Attribute
{
    public string ScenePath;

    public SceneAttribute()
    {
        throw new NotImplementedException("SceneAttribute requires Scene Path");
    }

    public SceneAttribute(string scenePath)
    {
        ScenePath = scenePath;
    }
}
