using AdventOfCode2024.Days;

namespace AdventOfCode2024Tests.Days;

[TestFixture]
public class Day17Tests
{
    [Test]
    public void Test1()
    {
        var computer = new Day17.Computer(0, 0, 9);
        
        computer.Run("2,6");
        
        Assert.That(computer.Register.B, Is.EqualTo(1));
    }
    
    [Test]
    public void Test2()
    {
        var computer = new Day17.Computer(10, 0, 0);
        
        computer.Run("5,0,5,1,5,4");

        Assert.That(computer.Output, Is.EqualTo("0,1,2"));
    }
    
    [Test]
    public void Test3()
    {
        var computer = new Day17.Computer(2024, 0, 0);
        
        computer.Run("0,1,5,4,3,0");

        Assert.That(computer.Output, Is.EqualTo("4,2,5,6,7,7,7,7,3,1,0"));
        Assert.That(computer.Register.A, Is.EqualTo(0));
    }
    
    [Test]
    public void Test4()
    {
        var computer = new Day17.Computer(0, 29, 0);
        
        computer.Run("1,7");

        Assert.That(computer.Register.B, Is.EqualTo(26));
    }
    
    [Test]
    public void Test5()
    {
        var computer = new Day17.Computer(0, 2024, 43690);
        
        computer.Run("4,0");

        Assert.That(computer.Register.B, Is.EqualTo(44354));
    }

    [Test]
    public void Part1Test()
    {
        var sut = new Day17();
        
        Assert.That(sut.Part1("TestData/17a.txt"), Is.EqualTo("4,6,3,5,6,3,5,2,1,0"));
    }
    
    [Test]
    public void Part2Test()
    {
        var sut = new Day17();

        Assert.That(sut.Part2("TestData/17b.txt"), Is.EqualTo("117440"));
    }
}