using System;
using System.Diagnostics;
using NUnit.Framework;
using Quartz.CoreCs.other.events;
using Quartz.Ecs.ecs.jobs;
using Quartz.Ecs.ecs.worlds;
using Quartz.Tests.data.ecs;

namespace Quartz.Tests.tests.ecs.systems; 

[TestFixture]
public class EntitySystemTests {
	[TestCase(0)]
	[TestCase(100)]
	[TestCase(1000)]
	[TestCase(10_000)]
	[TestCase(50_000)]
	[TestCase(500_000)]
	[TestCase(2_000_000)]
	public void TestJobSchedule(int count) {
		World world = World.Create($"test world {Guid.NewGuid()}");
		
		Stopwatch sw = Stopwatch.StartNew();
		world.AddEntities(count, world.GetArchetype<TestNormalComponentA>(), _ => { });
		world.AddEntities(count/2, world.GetArchetype<TestNormalComponentA, TestNormalComponentB>(), _ => { });
		world.AddEntities(count/4, world.GetArchetype<TestNormalComponentA, TestSharedComponentD>(), _ => { });
		Console.WriteLine($"---------- {sw.ElapsedMilliseconds}ms for creation ----------");
		

		for (int i = 0; i < 4; i++) {
			Console.WriteLine($"----------------------- iteration {i}");
			RunParallel_();
			RunSingleThread_();
		}
		
		world.DestroyWorld();

		void RunParallel_() {
			sw.Restart();
			TestEntitiesSystem sys = new(JobScheduleSettings.immediateNextUpdate, world.Select<TestNormalComponentA>());
			sys.Schedule(JobScheduleSettings.immediateNow);
			FixedUpdatePipeline.OnFixedUpdate();
			Console.WriteLine($"---------- {sw.ElapsedMilliseconds}ms for parallel iteration ----------");
		}
		
		void RunSingleThread_() {
			sw.Restart();
			TestEntitiesSystem sys = new(JobScheduleSettings.immediateNextUpdate with {multithreaded = false}, world.Select<TestNormalComponentA>());
			sys.Schedule(JobScheduleSettings.immediateNow);
			FixedUpdatePipeline.OnFixedUpdate();
			Console.WriteLine($"---------- {sw.ElapsedMilliseconds}ms for single-threaded iteration ----------");
		}
	}
}