namespace AdventOfCode2024.Days;

public class Day23 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        var puzzle = new Puzzle(inputData);

        return puzzle
            .GetTriangles()
            .Count(x => 
                x.Item1.StartsWith("t") || 
                x.Item2.StartsWith("t") || 
                x.Item3.StartsWith("t"))
            .ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        throw new NotImplementedException();
    }

    public class Puzzle
    {
        public Puzzle(IEnumerable<string> inputData)
        {
            foreach (var line in inputData)
            {
                var parts = line.Split('-');
                var a = parts[0];
                var b = parts[1];
                
                if (!_adjacencies.ContainsKey(a)) _adjacencies[a] = [];
                if (!_adjacencies.ContainsKey(b)) _adjacencies[b] = [];

                _adjacencies[a].Add(b);
                _adjacencies[b].Add(a);
            }
        }

        private readonly Dictionary<string, HashSet<string>> _adjacencies = new();

        public HashSet<(string, string, string)> GetTriangles()
        {
            var triangles = new HashSet<(string, string, string)>();
            foreach (var node in _adjacencies.Keys)
            {
                var neighbours = _adjacencies[node];
                foreach (var n1 in neighbours)
                {
                    foreach (var n2 in neighbours)
                    {
                        if (string.CompareOrdinal(n1, n2) < 0 && _adjacencies[n1].Contains(n2))
                        {
                            var sortedTriangle = new List<string> { node, n1, n2 };
                            sortedTriangle.Sort();
                            triangles.Add((sortedTriangle[0], sortedTriangle[1], sortedTriangle[2]));
                        }
                    }
                }
            }

            return triangles;
        }
    }
    

    public override int Day => 23;
}