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
        var puzzle = new Puzzle(inputData);

        return puzzle.GetLanPartyPassword();
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

        public string GetLanPartyPassword()
        {
            var cliques = new List<HashSet<string>>();
            var p = new HashSet<string>(_adjacencies.Keys); // Set of all nodes
            var r = new HashSet<string>();          // Currently growing clique
            var x = new HashSet<string>();          // Already processed nodes

            BronKerbosch(r, p, x, _adjacencies, cliques);
            
            return
                string.Join(",",
                    cliques
                        .OrderByDescending(c => c.Count)
                        .First()
                        .OrderBy(c => c));
        }
        
        private static void BronKerbosch(
            HashSet<string> r, 
            HashSet<string> p, 
            HashSet<string> x,
            Dictionary<string, HashSet<string>> graph, 
            List<HashSet<string>> cliques)
        {
            if (p.Count == 0 && x.Count == 0)
            {
                // R is a maximal clique
                cliques.Add([..r]);
                return;
            }

            // Make a copy of P to iterate safely
            var pCopy = new HashSet<string>(p);

            foreach (var v in pCopy)
            {
                // R ∪ {v}
                var newR = new HashSet<string>(r) { v };

                // P ∩ N(v)
                var newP = new HashSet<string>(p.Intersect(graph[v]));

                // X ∩ N(v)
                var newX = new HashSet<string>(x.Intersect(graph[v]));

                // Recursive call
                BronKerbosch(newR, newP, newX, graph, cliques);

                // Remove v from P and add to X
                p.Remove(v);
                x.Add(v);
            }
        }
    }

    public override int Day => 23;
}