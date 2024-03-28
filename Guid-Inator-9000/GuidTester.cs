namespace Guid_Inator_9000;
internal class GuidTester
{
    internal void Start()
    {
        var numberOfProcessorsToUse = Environment.ProcessorCount - 2;
        Console.WriteLine($"Using {numberOfProcessorsToUse} processors:");
        while (true)
        {
            var guids = GenerateGuidsUsingParallelism(numberOfProcessorsToUse);
            //LogGuids(guids);
            if (CheckForDuplicates(guids))
            {
                Console.WriteLine("Duplicate guids found!");
                break;
            }
            Console.CursorTop = 1;
            Console.CursorLeft = 0;
        }
    }

    internal Guid[] GenerateGuidsUsingParallelism(int degreeOfParallelism)
    {
        Guid[] guids = new Guid[degreeOfParallelism];
        var options = new ParallelOptions { MaxDegreeOfParallelism = degreeOfParallelism };
        Parallel.For(0, degreeOfParallelism, options, i =>
        {
            guids[i] = Guid.NewGuid();
        });
        return guids;
    }

    internal void LogGuids(IEnumerable<Guid> guids)
    {
        Console.WriteLine(string.Join("\n", guids));
    }

    internal bool CheckForDuplicates(IEnumerable<Guid> guids)
    {
        return guids.Distinct().Count() != guids.Count();
    }
}
