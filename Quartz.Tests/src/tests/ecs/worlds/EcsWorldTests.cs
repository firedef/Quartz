using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using Quartz.Ecs.ecs.archetypes;
using Quartz.Ecs.ecs.identifiers;
using Quartz.Ecs.ecs.worlds;
using Quartz.Tests.data.ecs;

namespace Quartz.Tests.tests.ecs.worlds; 

[TestFixture]
public class EcsWorldTests {
	[Test]
	public unsafe void TestEntityCreate() {
		World.ClearAll();
		World world = World.Create("test world");
		EntityId id = world.AddEntity<TestNormalComponentA, TestNormalComponentG>();
		Assert.AreEqual(1, world.entityCount);
		Assert.AreEqual(1, world.archetypes.archetypeCount);
		Assert.AreEqual(0, id.position);

		*world.SharedIndex<TestSharedComponentD>(id) = 5;
		Assert.AreEqual(1, world.entityCount);
		Assert.AreEqual(2, world.archetypes.archetypeCount);
		Assert.AreEqual(0, id.position);
		world.DestroyWorld();
	}
	
	[Test]
	public unsafe void TestSharedComponent() {
		World.ClearAll();
		World world = World.Create($"test world {Guid.NewGuid()}");
		World.ClearSharedComponents<TestSharedComponentD>();
		
		EntityId id = world.AddEntity<TestNormalComponentA, TestNormalComponentG>();
		Assert.AreEqual(1, world.entityCount);
		Assert.AreEqual(1, world.archetypes.archetypeCount);
		Assert.AreEqual(0, id.position);

		*world.SharedIndex<TestSharedComponentD>(id) = World.AddSharedComponent(new TestSharedComponentD {a = 55});
		Assert.AreEqual(1, world.entityCount);
		Assert.AreEqual(2, world.archetypes.archetypeCount);
		Assert.AreEqual(0, id.position);
		Assert.AreEqual(1, *world.SharedIndex<TestSharedComponentD>(id));
		Assert.AreEqual(55, world.SharedComponent<TestSharedComponentD>(id)->a);

		world.SharedComponent<TestSharedComponentD>(id)->a = 984324;
		Assert.AreEqual(984324, World.GetSharedComponent<TestSharedComponentD>(1)->a);
		
		world.DestroyEntity(id);
		Assert.AreEqual(0, world.entityCount);
		World.ClearSharedComponents<TestSharedComponentD>();
		
		world.DestroyWorld();
	}

	[TestCase(1)]
	[TestCase(100)]
	[TestCase(1_000)]
	[TestCase(10_000)]
	[TestCase(100_000)]
	[TestCase(500_000)]
	[TestCase(2_000_000)]
	public unsafe void TestLargeAllocation(int count) {
		World world = World.Create($"test world {Guid.NewGuid()}");
		Archetype arch = world.GetArchetype<TestNormalComponentA, TestNormalComponentG>();
		world.AddEntities(count, arch, id => {
			world.Comp<TestNormalComponentA>(id)->a = 4;
			world.Comp<TestNormalComponentG>(id)->a = 345;
		});

		Assert.AreEqual(count, world.entityCount);
		Archetype archetype = world.GetArchetype<TestNormalComponentA, TestNormalComponentH>();
		Assert.AreEqual(count, archetype.components.elementCount);
		Assert.AreEqual(1, world.archetypes.archetypeCount);
		
		world.DestroyWorld();
	}

	[Test]
	public unsafe void TestInitAndDispose() {
		World world = World.Create($"test world {Guid.NewGuid()}");

		EntityId e0 = world.AddEntity<TestDisposableComponent>();
		EntityId e1 = world.AddEntity<TestDisposableComponent>();
		EntityId e2 = world.AddEntity<TestDisposableComponent>();
		
		Assert.AreEqual(1, world.Comp<TestDisposableComponent>(e0)->instances);
		Assert.AreEqual(1, world.Comp<TestDisposableComponent>(e1)->instances);
		Assert.AreEqual(1, world.Comp<TestDisposableComponent>(e2)->instances);
		
		world.DestroyEntity(e0);
		world.DestroyEntity(e1);
		
		Assert.AreEqual(1, world.Comp<TestDisposableComponent>(e2)->instances);

		world.DestroyWorld();
	}
}