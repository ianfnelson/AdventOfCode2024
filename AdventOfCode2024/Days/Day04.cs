namespace AdventOfCode2024.Days;

public class Day04 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        return new WordSearch(inputData)
            .CountThings("XMAS", false)
            .ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        return new WordSearch(inputData)
            .CountThings("MAS", true)
            .ToString();
    }

    private class WordSearch
    {
        public WordSearch(IEnumerable<string> inputData)
        {
            var inputList = inputData.ToList();
            Rows = inputList.Count;
            Columns = inputList[0].Length;

            Grid = new char[Rows, Columns];

            for (var y = 0; y < Rows; y++)
            {
                for (var x = 0; x < Columns; x++)
                {
                    Grid[y,x] = inputList[y][x];
                }
            }
        }

        public int Rows { get; }
        
        public int Columns { get; }
        
        public char[,] Grid { get; }
        
        public int CountThings(string word, bool crosses)
        {
            var count = 0;
            var reversedWord = new string(word.Reverse().ToArray());
            
            for (var y = 0; y < Rows; y++)
            {
                for (var x = 0; x < Columns; x++)
                {
                    if (crosses)
                    {
                        if (IsCrossSouthEastOf(word, reversedWord, y, x))
                        {
                            count++;
                        }
                    }
                    else
                    {
                        count += CountWordsSouthOrEastOf(word, reversedWord, y, x);
                    }
                }
            }

            return count;
        }

        private int CountWordsSouthOrEastOf(string word, string reversedWord, int row, int column)
        {
            return new[]
            {
                SeekNorthEast,
                SeekEast,
                SeekSouthEast,
                SeekSouth
            }.Count(seeker => seeker(word, reversedWord, row, column));
        }

        private bool IsCrossSouthEastOf(string word, string reversedWord, int row, int column)
        {
            return SeekSouthEast(word, reversedWord, row, column) 
                   && SeekNorthEast(word, reversedWord, row + word.Length - 1, column);
        }
        
        private bool SeekNorthEast(string word, string reversedWord, int row, int column)
        {
            if (row - (word.Length - 1) < 0 || column + word.Length > Columns) return false;
            
            return new []{ word, reversedWord }.Any(
                w => !w.Where((t, d) => Grid[row - d, column + d] != t).Any());
        }
        
        private bool SeekEast(string word, string reversedWord, int row, int column)
        {
            if (column + word.Length > Columns) return false;

            return new []{ word, reversedWord }.Any(
                w => !w.Where((t, d) => Grid[row, column + d] != t).Any());
        }
        
        private bool SeekSouthEast(string word, string reversedWord, int row, int column)
        {
            if (row + word.Length > Rows || column + word.Length > Columns) return false;

            return new []{ word, reversedWord }.Any(
                w => !w.Where((t, d) => Grid[row + d, column + d] != t).Any());
        }

        private bool SeekSouth(string word, string reversedWord, int row, int column)
        {
            if (row + word.Length > Rows) return false;

            return new []{ word, reversedWord }.Any(
                w => !w.Where((t, d) => Grid[row + d, column] != t).Any());
        }
    }

    public override int Day => 4;
}