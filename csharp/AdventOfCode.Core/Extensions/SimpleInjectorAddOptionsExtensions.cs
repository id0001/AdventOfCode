using AdventOfCode.Core.IO;
using SimpleInjector.Integration.ServiceCollection;

namespace AdventOfCode.Core.Extensions;

public static class SimpleInjectorAddOptionsExtensions
{
    public static SimpleInjectorAddOptions AddInput(this SimpleInjectorAddOptions source)
    {
        source.Container.RegisterSingleton<IInputReader, InputReader>();
        return source;
    }
}