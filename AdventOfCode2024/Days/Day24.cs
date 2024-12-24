namespace AdventOfCode2024.Days;

public class Day24 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        var device = new Device(inputData.ToList());

        return device.DecimalOutput.ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        throw new NotImplementedException();
    }

    public override int Day => 24;

    public class Device
    {
        private Dictionary<string, Wire> Wires { get; } = new();
        
        public Device(IList<string> inputData)
        {
            var line = 0;
            while (inputData[line] != "")
            {
                var parts = inputData[line].Split(": ");
                var wire = new Wire(parts[0], this)
                {
                    Value = parts[1] != "0"
                };
                Wires.Add(wire.Name, wire);

                line++;
            }

            line++;

            while (line < inputData.Count)
            {
                var parts = inputData[line].Split(" ");

                var wire = new Wire(parts[4], this)
                {
                    Operand1 = parts[0],
                    Operand2 = parts[2],
                    Operation = parts[1]
                };
                Wires.Add(wire.Name, wire);

                line++;
            }
        }

        public class Wire(string name, Device device)
        {
            private Device Device { get; } = device;
            public string Name { get; } = name;

            private bool? _value;
            
            public bool Value
            {
                get
                {
                    _value ??= DetermineValue();
                    return _value.Value;
                }
                init => _value = value;
            }

            private bool DetermineValue()
            {
                var op1Value = Device.Wires[Operand1!].Value;
                var op2Value = Device.Wires[Operand2!].Value;

                switch (Operation)
                {
                    case "AND":
                        return op1Value && op2Value;
                    case "OR":
                        return op1Value || op2Value;
                    case "XOR":
                        return op1Value ^ op2Value;
                    default:
                        throw new InvalidOperationException();
                }
            }

            public override string ToString()
            {
                return Value ? "1" : "0";
            }

            public string? Operand1 { get; init; }
            
            public string? Operand2 { get; init; }
            
            public string? Operation { get; init; }
        }

        public long DecimalOutput
        {
            get
            {
                var binaryString = Wires
                    .Where(x => x.Key.StartsWith("z"))
                    .OrderByDescending(x => x.Key)
                    .Select(x => x.Value.ToString())
                    .Aggregate((c, n) => $"{c}{n}");

                return Convert.ToInt64(binaryString, 2);
            }
        }
    }
}