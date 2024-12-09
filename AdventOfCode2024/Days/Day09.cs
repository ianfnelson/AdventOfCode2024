namespace AdventOfCode2024.Days;

public class Day09 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        var disk = new Disk(inputData.Single());
        
        disk.Compact(true);
        
        return disk.Checksum.ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        var disk = new Disk(inputData.Single());
        
        disk.Compact(false);
        
        return disk.Checksum.ToString();
    }

    public class Disk
    {
        public Disk(string input)
        {
            var isFile = true;
            var fileId = 0;
            int blockId = 0;

            foreach (var character in input.ToCharArray())
            {
                var length = int.Parse(character.ToString());

                var file = isFile ? new File(fileId, length) : null;
                
                for (var i = 0; i < length; i++)
                {
                    Blocks.Add(new Block(blockId, file));
                    blockId++;
                }

                if (isFile)
                {
                    fileId++;
                    Files.Add(file!);
                }
                
                isFile = !isFile;
            }
        }

        private IList<Block> Blocks { get; set; } = new List<Block>();
        private IList<File> Files { get; set; } = new List<File>();

        public void Compact(bool splitFiles = true)
        {
            var leftPointer = 0;
            var rightPointer = Blocks.Count-1;

            while (leftPointer < rightPointer - 1)
            {
                while (Blocks[leftPointer].File != null)
                {
                    leftPointer++;
                }

                while (Blocks[rightPointer].File == null)
                {
                    rightPointer--;
                }

                if (leftPointer >= rightPointer)
                {
                    break;
                }
                
                Blocks[leftPointer].File = Blocks[rightPointer].File;
                Blocks[rightPointer].File = null;
            }
        }
        
        public long Checksum
        {
            get
            {
                return Blocks
                    .Where(x => x.File!=null)
                    .Select(x => (long)(x.File!.Id * x.Index))
                    .Sum();
            }
        }

        public class Block(int index, File? file)
        {
            public int Index { get; set; } = index;

            public File? File { get; set; } = file;
        }
        
        public class File(int id, int length)
        {
            public int Id { get; init; } = id;

            public int Length { get; init; } = length;
        }
    }

    public override int Day => 9;
}