namespace AdventOfCode2024.Days;

public class Day05 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        var printer = new Printer(inputData.ToList());

        return printer
            .Updates
            .Where(x => x.AdheresToRules)
            .Select(x => x.MiddlePageNumber)
            .Sum()
            .ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        var printer = new Printer(inputData.ToList());

        var incorrectlyOrderedUpdates = printer
            .Updates
            .Where(x => !x.AdheresToRules).ToList();

        foreach (var update in incorrectlyOrderedUpdates)
        {
            update.Reorder();
        }

        return incorrectlyOrderedUpdates
            .Select(x => x.MiddlePageNumber)
            .Sum()
            .ToString();
    }

    public override int Day => 5;
    
    private class Printer
    {
        public Printer(IList<string> inputData)
        {
            var rulesList = inputData.TakeWhile(x => x != "").ToList();
            var updatesList = inputData.SkipWhile(x => x != "").Skip(1).ToList();

            Rules = new PageOrderingRules(rulesList);
            Updates = updatesList.Select(x => new Update(x, this)).ToList();
        }
        
        public IList<Update> Updates { get; }
        
        private PageOrderingRules Rules { get; }
        
        public class Update(string input, Printer printer)
        {
            private IList<int> PageNumbers { get; set; } = input.Split(",").Select(int.Parse).ToList();

            public int MiddlePageNumber => PageNumbers[(PageNumbers.Count-1)/2];

            public bool AdheresToRules
            {
                get
                {
                    for (int i = 0; i < PageNumbers.Count-1; i++)
                    {
                        var pagesBefore = printer.Rules.PagesBefore.GetValueOrDefault(PageNumbers[i]);

                        if (pagesBefore == null) continue;
                        
                        if (pagesBefore.Any(x => PageNumbers.Skip(i+1).Contains(x))) return false;
                    }

                    return true;
                }
            }

            public void Reorder()
            {
                var newList = new List<int>();
                
                foreach (var unused in PageNumbers)
                {
                    newList.Add(PageNumbers.First(x =>
                        !newList.Contains(x) &&
                        (printer.Rules.PagesBefore.GetValueOrDefault(x) == null ||
                         !printer.Rules.PagesBefore.GetValueOrDefault(x)!.Any(y =>
                             PageNumbers.Except(newList).Contains(y)))));
                }
                
                PageNumbers = newList;
            }
        }

        private class PageOrderingRules
        {
            public Dictionary<int, List<int>> PagesBefore { get; set; }
            
            public PageOrderingRules(IList<string> inputData)
            {
                PagesBefore = inputData
                    .Select(x => x.Split("|"))
                    .Select(x => new { Before = int.Parse(x[0]), After = int.Parse(x[1]) })
                    .GroupBy(x => x.After)
                    .ToDictionary(
                        x => x.Key, 
                        x => x.Select(y => y.Before).ToList());
            }
        }
    }
}