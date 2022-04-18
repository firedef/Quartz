using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using Quartz.Ecs.ecs.archetypes;
using Quartz.Ecs.ecs.identifiers;
using Quartz.Ecs.ecs.queries;
using Quartz.Ecs.ecs.views;
using Quartz.Ecs.ecs.worlds;
using Quartz.Tests.data.ecs;

namespace Quartz.Tests.tests.ecs.worlds; 

[TestFixture]
public class IterationTests {
	[TestCase(1000)]
	[TestCase(10_000)]
	[TestCase(100_000)]
	public unsafe void TestIteration(int count) {
		World world = World.Create($"test world {Guid.NewGuid()}");
		
		Stopwatch sw = Stopwatch.StartNew();
		world.AddEntities(count, world.GetArchetype<TestNormalComponentC>(), _ => { });
		Console.WriteLine($"{sw.ElapsedMilliseconds}ms to create {count} entities");

		sw.Restart();
		QueryResult result = world.Select<TestNormalComponentC>().Result();
		Assert.AreEqual(1, result.archetypes.Count);
		foreach (Archetype archetype in result.archetypes) {
			var view = archetype.Components<TestNormalComponentC>();
			Assert.AreEqual(count, view.count);

			for (int i = 0; i < view.count; i++) {
				view.component0[i].a = i;
			}
		}
		Console.WriteLine($"{sw.ElapsedMilliseconds}ms per {count} entities");
		
		world.AddEntities(count, world.GetArchetype<TestNormalComponentC>(), _ => { });

		result = world.Select<TestNormalComponentC>().Result();
		Assert.AreEqual(1, result.archetypes.Count);
		foreach (Archetype archetype in result.archetypes) {
			var view = archetype.Components<TestNormalComponentC>();
			Assert.AreEqual(count * 2, view.count);

			for (int i = 0; i < count; i++) Assert.AreEqual(i, view.component0[i].a);
		}

		world.DestroyWorld();
	}
	
	[TestCase(1000)]
	[TestCase(10_000)]
	[TestCase(100_000)]
	[TestCase(500_000)]
	[TestCase(2_500_000)]
	public unsafe void TestIterationSpeed(int count) {
		World world = World.Create($"test world {Guid.NewGuid()}");
		
		Stopwatch sw = Stopwatch.StartNew();
		world.AddEntities(count, world.GetArchetype<TestNormalComponentA>(), _ => { });
		Console.WriteLine($"{sw.ElapsedMilliseconds}ms to create {count} entities");

		sw.Restart();
		
		QueryResult result = world.Select<TestNormalComponentA>().Result();
		foreach (Archetype archetype in result.archetypes) {
			var view = archetype.Components<TestNormalComponentA>();

			for (int i = 0; i < view.count; i++) {
				view.component0[i].a = i;
			}
		}
		
		Console.WriteLine($"{sw.ElapsedMilliseconds}ms per {count} entities");

		world.DestroyWorld();
	}
}