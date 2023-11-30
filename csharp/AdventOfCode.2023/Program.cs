using AdventOfCode.Core;
using AdventOfCode.Core.Extensions;
using Microsoft.Extensions.Hosting;

AdventOfCodeHost
    .Create(args, options =>
    {
        options.AddInput();
    })
    .Run();