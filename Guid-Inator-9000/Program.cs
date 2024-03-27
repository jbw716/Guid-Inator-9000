var numberOfProcessorsToUse = Environment.ProcessorCount - 4;
Console.WriteLine($"Using {numberOfProcessorsToUse} processors:");
while (true)
{
    Guid[] guids = new Guid[numberOfProcessorsToUse];
    var options = new ParallelOptions { MaxDegreeOfParallelism = numberOfProcessorsToUse };
    Parallel.For(0, numberOfProcessorsToUse, options, i =>
    {
        guids[i] = Guid.NewGuid();
    });
    Console.WriteLine(string.Join("\n", guids));
    if (guids.Distinct().Count() != guids.Length)
    {
        Console.WriteLine("Duplicate guids found!");
        break;
    }
    Console.CursorTop = 1;
    Console.CursorLeft = 0;
}
