using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Quartz.CoreCs.other.events;
using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.jobs;
using Quartz.Ecs.ecs.views;
using Quartz.Ecs.ecs.worlds;

namespace Quartz.Benchmarks.benchmarks; 

[MemoryDiagnoser]
[EventPipeProfiler(EventPipeProfile.CpuSampling)]
[SimpleJob(launchCount:1, warmupCount:4, targetCount:4)]
public class EcsEntityCreateAndIterateBenchmark {
	[Benchmark]
	public void RunParallel5M() => RunSystem(true, 10_000_000);

	private static void RunSystem(bool parallel, int count) {
		World world = World.Create($"test world {Guid.NewGuid()}");
		// world.AddEntities(count, world.GetArchetype<TestNormalComponentA>(), _ => { });

		for (int i = 0; i < count; i++) {
			world.AddEntity<TestNormalComponentA>();
		}

		TestEntitiesSystem sys = new();
		sys.Schedule(JobScheduleSettings.immediateNow);
		FixedUpdatePipeline.Step();
		world.DestroyWorld();
	}
}

public struct TestNormalComponentA : IComponent {
	public float a = -4.5f;
	public int b = 9;
	public void Parse(Dictionary<string, string> fields) { throw new NotImplementedException(); }
	public void Write(Dictionary<string, string> fields) { throw new NotImplementedException(); }
}

public struct TestNormalComponentB : IComponent {
	public bool a = true;
	public void Parse(Dictionary<string, string> fields) { throw new NotImplementedException(); }
	public void Write(Dictionary<string, string> fields) { throw new NotImplementedException(); }
}

public class TestEntitiesSystem : EntitiesJobBase {
	public override void Run() {
		JobScheduler.Schedule<TestNormalComponentA>(RunSystem, JobScheduleSettings.immediateNextUpdate);
	}
	
	private static unsafe void RunSystem(ComponentsView<TestNormalComponentA> components, JobState state) {
		Stopwatch sw = Stopwatch.StartNew();
		Console.WriteLine($"TestEntitiesSystem job start: {state.currentIteration} of {state.maxIteration}; completed: {state.completedIterations}; size: {components.count}");
		for (int i = 0; i < components.count; i++) {
			components.component0[i].a = 4 * components.component0[i].b;
		}
		Console.WriteLine($"TestEntitiesSystem job end in {sw.ElapsedMilliseconds}ms: {state.currentIteration} of {state.maxIteration}; completed: {state.completedIterations}");
	}
}