using System.Diagnostics;
using System.Reflection;
using AdventOfCode2024.Days;
using Autofac;

var container = BuildContainer();

var days = container.Resolve<IEnumerable<IDay>>();
var day = GetDay();
Console.WriteLine("Day " + day.Day);
var inputPath = $"InputFiles/{day.Day}.txt";
DoPart(1, () => day.Part1(inputPath));
DoPart(2, () => day.Part2(inputPath));

IContainer BuildContainer()
{
    var builder = new ContainerBuilder();
    builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();
    return builder.Build();
}

IDay GetDay()
{
    if (args.Length == 0)
    {
        return days.MaxBy(x => x.Day) ?? throw new InvalidOperationException();
    }

    return days.Single(x => x.Day == int.Parse(args[0]));
}

void DoPart(int partNumber, Func<string> partFunc)
{
    var sw = new Stopwatch();
    sw.Start();

    var result = partFunc();
    
    sw.Stop();
    
    Console.WriteLine($"Part {partNumber}: {result}  ({sw.ElapsedMilliseconds}ms) ({sw.ElapsedTicks} ticks)");
}