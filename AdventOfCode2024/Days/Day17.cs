namespace AdventOfCode2024.Days;

public class Day17 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        var parsed = ParseInput(inputData.ToList());

        var computer = new Computer(parsed.Registers);
        computer.Run(parsed.Program);
        return computer.Output;
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        var parsed = ParseInput(inputData.ToList());
        var b = parsed.Registers.B;
        var c = parsed.Registers.C;

        var a = 0;

        string output;
        do
        {
            a++;
            var computer = new Computer(a, b, c);
            computer.Run(parsed.Program);
            output = computer.Output;
        } while (output != parsed.Program);

        return a.ToString();
    }
    
    private (Computer.Registers Registers, string Program) ParseInput(IList<string> inputData)
    {
        var a = int.Parse(inputData[0].Replace("Register A: ", ""));
        var b = int.Parse(inputData[1].Replace("Register B: ", ""));
        var c = int.Parse(inputData[2].Replace("Register C: ", ""));

        var program = inputData[4].Replace("Program: ", "");

        return new ValueTuple<Computer.Registers, string>(new Computer.Registers(a, b, c), program);
    }

    public override int Day => 17;

    public class Computer
    {
        public class Registers(int a, int b, int c)
        {
            public int A { get; set; } = a;

            public int B { get; set; } = b;

            public int C { get; set; } = c;
        }

        public Computer(Registers registers)
        {
            Register = registers;
        }

        public Computer(int a, int b, int c) : this(new Registers(a, b, c))
        {
        }

        private readonly List<int> _program = new();

        private int InstructionPointer { get; set; }

        public Registers Register { get; }

        private readonly List<int> _output = new();

        public string Output => string.Join(",", _output);

        public void WriteOutput(int value)
        {
            _output.Add(value);
        }
        
        public void Run(string program)
        {
            _program.AddRange(
        program
                    .Split(",")
                    .Select(int.Parse)
                    );
            
            while (InstructionPointer < _program.Count)
            {
                var instruction = GetInstruction(_program[InstructionPointer]);
                instruction.Execute(_program[InstructionPointer + 1]);
            }
        }

        private IInstruction GetInstruction(int opcode)
        {
            IInstruction instruction = opcode switch
            {
                0 => new AdvInstruction(this),
                1 => new BxlInstruction(this),
                2 => new BstInstruction(this),
                3 => new JnzInstruction(this),
                4 => new BxcInstruction(this),
                5 => new OutInstruction(this),
                6 => new BdvInstruction(this),
                7 => new CdvInstruction(this),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            return instruction;
        }

        private interface IInstruction
        {
            void Execute(int operand);
        }

        private abstract class Instruction(Computer computer) : IInstruction
        {
            protected readonly Computer Computer = computer;

            protected int ComboOperand(int operand)
            {
                var comboOperand = operand switch
                {
                    0 => 0,
                    1 => 1,
                    2 => 2,
                    3 => 3,
                    4 => Computer.Register.A,
                    5 => Computer.Register.B,
                    6 => Computer.Register.C,
                    _ => throw new ArgumentOutOfRangeException()
                };

                return comboOperand;
            }

            public void Execute(int operand)
            {
                ExecuteImpl(operand);
                MoveInstructionPointer();
            }

            protected abstract void ExecuteImpl(int operand);

            protected virtual void MoveInstructionPointer()
            {
                Computer.InstructionPointer += 2;
            }
        }

        private class AdvInstruction(Computer computer) 
            : DivideInstruction(computer)
        {
            protected override void ExecuteImpl(int operand)
            {
                Computer.Register.A = DoDivision(operand);
            }
        }

        private class BxlInstruction(Computer computer)
            : Instruction(computer)
        {
            protected override void ExecuteImpl(int operand)
            {
                Computer.Register.B ^= operand;
            }
        }

        private class BstInstruction(Computer computer)
            : Instruction(computer)
        {
            protected override void ExecuteImpl(int operand)
            {
                Computer.Register.B = ComboOperand(operand) % 8;
            }
        }

        private class BdvInstruction(Computer computer) 
            : DivideInstruction(computer)
        {
            protected override void ExecuteImpl(int operand)
            {
                Computer.Register.B = DoDivision(operand);
            }
        }

        private class CdvInstruction(Computer computer) 
            : DivideInstruction(computer)
        {
            protected override void ExecuteImpl(int operand)
            {
                Computer.Register.C = DoDivision(operand);
            }
        }

        private class JnzInstruction(Computer computer)
            : Instruction(computer)
        {
            private int _nextInstructionPointer;
                
            protected override void ExecuteImpl(int operand)
            {
                _nextInstructionPointer = Computer.Register.A == 0 ? 
                    Computer.InstructionPointer += 2 : operand;
            }

            protected override void MoveInstructionPointer()
            {
                Computer.InstructionPointer = _nextInstructionPointer;
            }
        }

        private class BxcInstruction(Computer computer)
            : Instruction(computer)
        {
            protected override void ExecuteImpl(int operand)
            {
                Computer.Register.B ^= Computer.Register.C;
            }
        }

        private class OutInstruction(Computer computer)
            : Instruction(computer)
        {
            protected override void ExecuteImpl(int operand)
            {
                Computer.WriteOutput(ComboOperand(operand) % 8);
            }
        }

        private abstract class DivideInstruction(Computer computer) 
            : Instruction(computer)
        {
            protected int DoDivision(int operand)
            {
                var numerator = Computer.Register.A;
                var denominator = 1 << ComboOperand(operand);

                return numerator / denominator;
            }
        }
    }
}