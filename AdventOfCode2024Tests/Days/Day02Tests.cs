using AdventOfCode2024.Days;

namespace AdventOfCode2024Tests.Days;

[TestFixture]
public class Day02Tests
{
    private readonly Day02 _systemUnderTest = new();

    [Test]
    public void Part1Test()
    {
        Assert.That(_systemUnderTest.Part1("TestData/2.txt"), Is.EqualTo("2"));
    }
    
    [Test]
    public void Part2Test()
    {
        Assert.That(_systemUnderTest.Part2("TestData/2.txt"), Is.EqualTo("4"));
    }

    [Test]
    public void Report_IsSafe_LevelsAreSteadilyIncreasing_Safe()
    {
        var sut = Day02.Report.Parse("1 3 6 7 9");
        
        Assert.That(sut.IsSafe, Is.True);
    }
    
    [Test]
    public void Report_IsSafe_LevelsAreSteadilyDecreasing_Safe()
    {
        var sut = Day02.Report.Parse("7 6 4 2 1");
        
        Assert.That(sut.IsSafe, Is.True);
    }

    [Test]
    public void Report_IsSafe_LevelsIncreaseByMoreThanThree_Unsafe()
    {
        var sut = Day02.Report.Parse("1 5 6 7 9");

        Assert.That(sut.IsSafe, Is.False);
    }

    [Test]
    public void Report_IsSafe_LevelsDecreaseByMoreThanThree_Unsafe()
    {
        var sut = Day02.Report.Parse("9 7 6 5 1");
        
        Assert.That(sut.IsSafe, Is.False);
    }

    [Test]
    public void Report_IsSafe_LevelsIncreaseAndDecrease_Unsafe()
    {
        var sut = Day02.Report.Parse("1 3 6 4 5");
        
        Assert.That(sut.IsSafe, Is.False);
    }

    [Test]
    public void Report_IsSafe_LevelsRemainTheSame_Unsafe()
    {
        var sut = Day02.Report.Parse("1 3 3 4 5");
        
        Assert.That(sut.IsSafe, Is.False);
    }

    [Test]
    public void Report_IsSafeWithDampener_WasSafe_Safe()
    {
        var sut = Day02.Report.Parse("7 6 4 2 1");
        
        Assert.That(sut.IsSafeWithDampener, Is.True);
    }
    
    
    [Test]
    public void Report_IsSafeWithDampener_LevelsIncreaseByMoreThanThree_Unsafe()
    {
        var sut = Day02.Report.Parse("1 2 7 8 9");
        
        Assert.That(sut.IsSafeWithDampener, Is.False);
    }
    
    [Test]
    public void Report_IsSafeWithDampener_LevelsDecreaseByMoreThanThree_Unsafe()
    {
        var sut = Day02.Report.Parse("9 7 6 2 1");
        
        Assert.That(sut.IsSafeWithDampener, Is.False);
    }

    [Test]
    public void Report_IsSafeWithDampener_OneDeltaOfZero_Safe()
    {
        var sut = Day02.Report.Parse("8 6 4 4 1");
        
        Assert.That(sut.IsSafeWithDampener, Is.True);
    }
    
    [Test]
    public void Report_IsSafeWithDampener_OneAnomalousDirection_Safe()
    {
        var sut = Day02.Report.Parse("1 3 4 2 5");
        
        Assert.That(sut.IsSafeWithDampener, Is.True);
    }
}