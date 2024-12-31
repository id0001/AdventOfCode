using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Lib.Tests;

[TestClass]
public class LinqAsExtensionTests
{
    [TestMethod]
    [DataRow("1 2 3 4 5")]
    [DataRow("")]
    public void As_IList_Converts_Correctly(string input)
    {
        var list = input.SplitBy(" ");
        var result = list.As<int>();

        Assert.AreEqual(input, string.Join(" ", result));
    }

    [TestMethod]
    [DataRow(["1", "2", "3", "4", "5"])]
    [DataRow([])]
    public void As_Array_Converts_Correctly(string[] input)
    {
        var result = input.As<int>();

        for (var i = 0; i < input.Length; i++)
            Assert.AreEqual(input[i], result[i].ToString());
    }

    [TestMethod]
    [DataRow(["1", "2", "3", "4", "5"])]
    [DataRow([])]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public void As_Array_Converts_Correctly(IEnumerable<string> input)
    {
        var result = input.As<int>().ToList();

        Assert.IsTrue(input.SequenceEqual(result.Select(x => x.ToString())));
    }
}