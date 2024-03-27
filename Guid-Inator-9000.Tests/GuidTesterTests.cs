namespace Guid_Inator_9000.Tests;
public class GuidTesterTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(10)]
    public void GenerateGuidsUsingParallelismTest(int degreeOfParallelism)
    {
        var guidTester = new GuidTester();
        var guids = guidTester.GenerateGuidsUsingParallelism(degreeOfParallelism);
        Assert.Equal(degreeOfParallelism, guids.Length);
        Assert.All(guids, guid => Assert.NotEqual(Guid.Empty, guid));
    }

    [Fact]
    public void CheckForDuplicatesTest()
    {
        var guidTester = new GuidTester();
        var guids = new Guid[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        Assert.False(guidTester.CheckForDuplicates(guids));
        guids = new Guid[] { Guid.NewGuid(), guids[0], guids[0] };
        Assert.True(guidTester.CheckForDuplicates(guids));
    }

    [Fact]
    public void LogGuidsTest()
    {
        var guidTester = new GuidTester();
        var guids = new Guid[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        guidTester.LogGuids(guids);
        var expectedOutput = string.Join("\n", guids) + Environment.NewLine;
        Assert.Equal(expectedOutput, consoleOutput.ToString());
    }
}