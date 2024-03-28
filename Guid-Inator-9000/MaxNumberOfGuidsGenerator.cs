using BenchmarkDotNet.Attributes;
using System.Collections.Concurrent;
using System.Numerics;

namespace Guid_Inator_9000;

public class MaxNumberOfGuidsGenerator
{
    //public BigInteger MaxNumberOfGuids = (BigInteger)50000000;
    [Params(5000)]
    public BigInteger MaxNumberOfGuids = (BigInteger)Math.Pow(2, 128);

    public void Run()
    {
        //using StreamWriter file = new(Path.Combine(Path.GetTempPath(), "Guid-Inator-9000", "MaxNumberOfGuids.txt"));
        //file.WriteLine(MaxNumberOfGuids);
        GenerateGuidsParallelSimple();
        //GenerateGuidsSingleThreaded();
        Console.WriteLine("Done!");
    }

    [Benchmark]
    public void GenerateGuidsSingleThreaded()
    {
        var Guids = new List<Guid>();
        using StreamWriter file = new(Path.Combine(Path.GetTempPath(), "MaxNumberOfGuids.txt"));
        for (BigInteger i = 0; i < MaxNumberOfGuids;)
        {
            Guids.Add(Guid.NewGuid());
            i++;
            if (i % 10000000 == 0)
            {
                Console.WriteLine(i.ToString("N0") + " of " + MaxNumberOfGuids.ToString("N0") + " guids generated.");
                // write guids to file and clear the array to prevent memory overflow
                foreach (var guid in Guids)
                {
                    file.WriteLine(guid);
                }
                Guids.Clear();
            }
        }
    }

    [Benchmark]
    public void GenerateGuidsParallelSimple()
    {
        using StreamWriter file = new(Path.Combine(Path.GetTempPath(), "MaxNumberOfGuids.txt"));
        for (BigInteger i = 0; i < MaxNumberOfGuids;)
        {
            var Guids = new ConcurrentBag<Guid>();
            var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount - 2 };
            Parallel.For(0, 10000000, (index) =>
            {
                Guids.Add(Guid.NewGuid());
            });
            i += 10000000;
            Console.WriteLine(i.ToString("N0") + " of " + MaxNumberOfGuids.ToString("N0") + " guids generated.");
            // write guids to file and clear the array to prevent memory overflow
            foreach (var guid in Guids)
            {
                file.WriteLine(guid);
            }
            Guids.Clear();
        }
    }

    //[Benchmark]
    public void GenerateGuidsParallelBasic()
    {
        var Guids = new ConcurrentBag<Guid>();
        using StreamWriter file = new(Path.Combine(Path.GetTempPath(), "MaxNumberOfGuids.txt"));
        BigInteger i = 0;
        While(
            () => i < MaxNumberOfGuids,
            loopState =>
            {
                Guids.Add(Guid.NewGuid());
                i++;
                if (i % 10000000 == 0)
                {
                    Console.WriteLine(i.ToString("N0") + " of " + MaxNumberOfGuids.ToString("N0") + " guids generated.");
                    // write guids to file and clear the array to prevent memory overflow
                    lock (Guids)
                    {
                        foreach (var guid in Guids)
                        {
                            file.WriteLine(guid);
                        }
                    }
                    Guids.Clear();
                }
            }
        );
    }

    //[Benchmark]
    public void GenerateGuidsParallelComplex()
    {
        using StreamWriter file = new(Path.Combine(Path.GetTempPath(), "MaxNumberOfGuids.txt"));
        for (BigInteger i = 0; i < MaxNumberOfGuids;)
        {
            var Guids = new ConcurrentBag<Guid>();
            var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount - 2 };
            Parallel.ForEach(new InfinitePartitioner(), parallelOptions, (ignored, loopState) =>
            {
                Guids.Add(Guid.NewGuid());
                i++;
                if (i % 10000000 == 0)
                {
                    loopState.Stop();
                }
            });
            Console.WriteLine(i.ToString("N0") + " of " + MaxNumberOfGuids.ToString("N0") + " guids generated.");
            // write guids to file and clear the array to prevent memory overflow
            foreach (var guid in Guids)
            {
                file.WriteLine(guid);
            }
            Guids.Clear();
        }
    }


    private static void While(Func<bool> condition, Action<ParallelLoopState> body)
    {
        Parallel.ForEach(Infinite(), (ignored, loopState) =>
        {
            if (condition()) body(loopState);
            else loopState.Stop();
        });
    }

    private static IEnumerable<bool> Infinite()
    {
        while (true) yield return true;
    }
}