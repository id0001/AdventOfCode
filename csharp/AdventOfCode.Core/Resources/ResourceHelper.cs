using System.Reflection;

namespace AdventOfCode.Core.Resources;

public static class ResourceHelper
{
    public static string Read(string resourceName)
    {
        using var reader = OpenStream(resourceName);
        return reader.ReadToEnd();
    }
    
    public static Task<string> ReadAsync(string resourceName)
    {
        using var reader = OpenStream(resourceName);
        return reader.ReadToEndAsync();
    }

    private static StreamReader OpenStream(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var stream = assembly.GetManifestResourceStream(resourceName);
        
        if (stream is null)
            throw new InvalidOperationException($"Something went wrong while reading the resource: {resourceName}");
        
        return new StreamReader(stream);
    }
}