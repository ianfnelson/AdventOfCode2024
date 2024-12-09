namespace AdventOfCode2024.Days;

public class Day09 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        var disk = new Disk(inputData.Single());
        
        disk.Compact();
        
        return disk.Checksum.ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        throw new NotImplementedException();
    }

    public class Disk
    {
        public Disk(string input)
        {
            var isFile = true;
            var id = 0;
            
            foreach (var character in input.ToCharArray())
            {
                var blocks = int.Parse(character.ToString());
                
                for (var i = 0; i < blocks; i++)
                {
                    Blocks.Add(isFile ? id : null);
                }

                if (isFile)
                {
                    id++;
                }
                
                isFile = !isFile;
            }
        }

        private IList<int?> Blocks { get; set; } = new List<int?>();

        public void Compact()
        {
            var leftPointer = 0;
            var rightPointer = Blocks.Count-1;

            while (leftPointer < rightPointer - 1)
            {
                while (Blocks[leftPointer] != null)
                {
                    leftPointer++;
                }

                while (Blocks[rightPointer] == null)
                {
                    rightPointer--;
                }

                if (leftPointer >= rightPointer)
                {
                    break;
                }
                
                Blocks[leftPointer] = Blocks[rightPointer];
                Blocks[rightPointer] = null;
            }
        }
        
        public long Checksum
        {
            get
            {
                long total = 0;

                for (var i = 0; i < Blocks.Count; i++)
                {
                    if (!Blocks[i].HasValue)
                    {
                        continue;
                    }
                    total += i * Blocks[i]!.Value;
                }

                return total;
            }
        }
    }

    public override int Day => 9;
}