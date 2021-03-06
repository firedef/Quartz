using System;
using System.Diagnostics;
using NUnit.Framework;
using Quartz.CoreCs.other;
using Quartz.CoreCs.other.events;
using Quartz.CoreCs.other.time;

namespace Quartz.Tests.tests.events; 

[TestFixture]
public class PipelineTests {
	[Test]
	public void TestEventsExecution() {
		FixedUpdatePipeline.Resume();

		int mainThreadId = Environment.CurrentManagedThreadId;
		
		FixedUpdatePipeline.EnqueueWithDelay(() => { Console.WriteLine($"+4 thread: {Environment.CurrentManagedThreadId} tick:{Time.currentTick}"); Assert.AreEqual(4, Time.currentTick); }, "test", 4, 0, executeByMainThread:false, waitForComplete: true);
		FixedUpdatePipeline.EnqueueWithDelay(() => { Console.WriteLine($"+0 thread: {Environment.CurrentManagedThreadId} tick:{Time.currentTick}"); Assert.AreEqual(0, Time.currentTick); }, "test", 0, 0, executeByMainThread:false, waitForComplete: true);
		FixedUpdatePipeline.EnqueueWithDelay(() => { Console.WriteLine($"+8 thread: {Environment.CurrentManagedThreadId} tick:{Time.currentTick}"); Assert.AreEqual(8, Time.currentTick); }, "test", 8, 0, executeByMainThread:false, waitForComplete: true);
		FixedUpdatePipeline.EnqueueWithDelay(() => { Console.WriteLine($"-2 thread: {Environment.CurrentManagedThreadId} tick:{Time.currentTick}"); Assert.AreEqual(0, Time.currentTick); }, "test", -2, 0, executeByMainThread:false, waitForComplete: true);
		FixedUpdatePipeline.EnqueueWithDelay(() => { Console.WriteLine($"main tick:{Time.currentTick}"); Assert.AreEqual(mainThreadId, Environment.CurrentManagedThreadId); }, "test", 3, 0, executeByMainThread:true, waitForComplete: true);
		FixedUpdatePipeline.EnqueueWithDelay(() => { Console.WriteLine($"main tick:{Time.currentTick}"); Assert.AreEqual(mainThreadId, Environment.CurrentManagedThreadId); }, "test", 3, 0, executeByMainThread:true, waitForComplete: true);
		FixedUpdatePipeline.EnqueueWithDelay(() => { Console.WriteLine($"main tick:{Time.currentTick}"); Assert.AreEqual(mainThreadId, Environment.CurrentManagedThreadId); }, "test", 3, 0, executeByMainThread:true, waitForComplete: true);
		

		EventManager.ProcessCurrentAssembly();
		for (int i = 0; i < 20; i++) {
			EventManager.OnFixedUpdate(1);
		}
	}
	
	[Test]
	public void TestLoadBalancing() {
		FixedUpdatePipeline.Resume();

		for (int i = 0; i < 32; i++) {
			int t = Rand.Int(0, 15);
			FixedUpdatePipeline.EnqueueWithDelay(() => {
				Console.WriteLine($"hi_+{t} thread: {Environment.CurrentManagedThreadId} tick:{Time.currentTick}");
				int i = 0;
				for (int j = 0; j < 25_000_000; j++) {
					i = j - 1 + i;
				}
			}, "test", t, 10, executeByMainThread:false, waitForComplete: true);
		}
		
		EventManager.ProcessCurrentAssembly();
		for (int i = 0; i < 20; i++) {
			Console.WriteLine($"------------- fixed update start #{i}");
			Stopwatch sw = Stopwatch.StartNew();
			EventManager.OnFixedUpdate(1);
			Console.WriteLine($"------------- fixed update end #{i} {sw.ElapsedMilliseconds}ms");
		}
	}
}