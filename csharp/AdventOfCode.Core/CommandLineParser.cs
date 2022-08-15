using AdventOfCode.Core.Resources;
using DocoptNet;

namespace AdventOfCode.Core;

public class CommandLineParser
{
    private const string UsageResourceName = "AdventOfCode.Core.Resources.docopt.txt";

    private readonly string _usage;
    
    public CommandLineParser()
    {
        _usage = ResourceHelper.Read(UsageResourceName);
    }

    public event EventHandler<int>? RunChallenge; 
    public event EventHandler? RunLatest;
    public event EventHandler? RunAll;

    public void Parse(string[] args)
    {
        var arguments = new Docopt().Apply(_usage, args, true, "1.0", true, false);
        if (arguments is null)
            return;
        
        if (!arguments["--challenge"].IsNullOrEmpty)
        {
            var day = arguments["--challenge"];
            if (!day.IsInt)
            {
                throw new InvalidOperationException("<day> argument must be an integer");
            }
            
            RunChallenge?.Invoke(this, day.AsInt);
        }
        else if (arguments["--latest"].IsTrue)
        {
            RunLatest?.Invoke(this, EventArgs.Empty);
        }
        else if (arguments["--all"].IsTrue)
        {
            RunAll?.Invoke(this, EventArgs.Empty);
        }
    }
}