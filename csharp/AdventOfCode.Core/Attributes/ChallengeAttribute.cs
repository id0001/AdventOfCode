namespace AdventOfCode.Core;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class ChallengeAttribute : Attribute
{
    public ChallengeAttribute(int day)
    {
        Day = day;
    }
    
    public int Day { get; set; }
}