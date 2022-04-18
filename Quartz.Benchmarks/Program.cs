using System.Diagnostics;
using BenchmarkDotNet.Running;
using Quartz.Benchmarks.benchmarks;

BenchmarkRunner.Run<EcsEntityCreateAndIterateBenchmark>();

// run speedscope
ProcessStartInfo ps = new(@"/bin/bash") {
	Arguments = $@"-c ""find {Path.GetFullPath("BenchmarkDotNet.Artifacts/")}*.speedscope.json -exec speedscope ""{{}}"" \;""", 
	UseShellExecute = false, 
	RedirectStandardOutput = true
};
Process.Start(ps)!.WaitForExit();