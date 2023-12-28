namespace AdventOfCode.Core;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ChallengeAttribute(int day) : Attribute
{
    public int Day { get; set; } = day;
}