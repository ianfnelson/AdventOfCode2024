namespace AdventOfCode2024.Days;

public class Day09 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        var disk = new Disk(inputData.Single());
        
        disk.CompactPart1();
        
        return disk.Checksum.ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        var disk = new Disk(inputData.Single());
        
        disk.CompactPart2();
        
        return disk.Checksum.ToString();
    }

    public class Disk
    {
        public Disk(string input)
        {
            var isFile = true;
            var fileId = 0;
            var blockIndex = 0;

            foreach (var character in input.ToCharArray())
            {
                var length = int.Parse(character.ToString());

                var file = isFile ? new File(fileId, length, blockIndex) : null;
                
                for (var i = 0; i < length; i++)
                {
                    Blocks.Add(new Block(blockIndex, file));
                    blockIndex++;
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

        public void CompactPart1()
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

        public void CompactPart2()
        {
            for (var f = Files.Count-1; f >= 0; f--)
            {
                var file = Files[f];

                if (TryFindNewLocation(file, out Block? block))
                {
                    MoveFileToLocation(file, block!);
                }
            }
        }

        private bool TryFindNewLocation(File file, out Block? block)
        {
            var blockIndex = 0;
            var gapLength = 0;
            do
            {
                if (Blocks[blockIndex].File == null)
                {
                    gapLength++;

                    if (gapLength == file.Length)
                    {
                        block = Blocks[blockIndex - gapLength + 1];
                        return true;
                    }
                }
                else
                {
                    gapLength = 0;
                }
                
                blockIndex++;
            } while (blockIndex < file.StartIndex);

            block = null;
            return false;
        }

        private void MoveFileToLocation(File file, Block block)
        {
            for (var i = 0; i < file.Length; i++)
            {
                Blocks[block.Index+i].File = file;
                Blocks[file.StartIndex+i].File = null;
            }
            
            file.StartIndex = block.Index;
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
        
        public class File(int id, int length, int startIndex)
        {
            public int Id { get; init; } = id;

            public int Length { get; init; } = length;
            
            public int StartIndex { get; set; } = startIndex;
        }
    }

    public override int Day => 9;
}