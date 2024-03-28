using BenchmarkDotNet.Running;
using Guid_Inator_9000;
using System.Diagnostics;

// var guidTester = new GuidTester();
// guidTester.Start();

var summary = BenchmarkRunner.Run<MaxNumberOfGuidsGenerator>();

//var stopwatch = new Stopwatch();
//stopwatch.Start();

//var MaxNumberOfGuidsGenerator = new MaxNumberOfGuidsGenerator();
//MaxNumberOfGuidsGenerator.Run();

//stopwatch.Stop();
//Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");
