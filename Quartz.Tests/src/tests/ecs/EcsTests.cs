using System;
using NUnit.Framework;
using Quartz.objects.ecs.components;
using Quartz.objects.ecs.entities;
using Quartz.objects.ecs.filters;
using Quartz.objects.ecs.world;
using Quartz.objects.memory;

namespace Quartz.Tests.tests.ecs; 

[TestFixture]
public class EcsTests {
	[Test]
	public void TestWorldCreateAndDestroy() {
		World world = World.general;
		Assert.IsTrue(world.isAlive);
		Assert.AreEqual(1, World.worldCount);
		
		world.Destroy();
		Assert.IsFalse(world.isAlive);
		Assert.AreEqual(0, World.worldCount);
	}
	
	[Test]
	public void TestEntityCreateAndDestroy() {
		World world = World.general;
		world.Clear();
		Assert.AreEqual(0, world.currentEntityCount);
		
		EntityId entity = world.CreateEntity();
		Assert.AreEqual(1, world.currentEntityCount);
		
		world.DestroyEntity(entity);
		Assert.AreEqual(0, world.currentEntityCount);
	}
	
	[Test]
	public unsafe void TestEntityCreateWithComponentAndDestroy() {
		World world = World.general;
		world.Clear();
		Assert.AreEqual(0, world.currentEntityCount);
		
		EntityId entity = world.CreateEntity<TestComponent>();
		Assert.AreEqual(1, world.currentEntityCount);
		Assert.AreEqual(*world.Comp<TestComponent>(entity), new TestComponent());
		
		entity = world.CreateEntity<TestComponent>(InitMode.zeroed);
		Assert.AreEqual(2, world.currentEntityCount);
		Assert.AreNotEqual(IntPtr.Zero, (IntPtr) world.TryComp<TestComponent>(entity));
		Assert.AreEqual(*world.Comp<TestComponent>(entity), new TestComponent(0,0));
		
		entity = world.CreateEntity();
		Assert.AreEqual(3, world.currentEntityCount);
		Assert.AreEqual(IntPtr.Zero, (IntPtr) world.TryComp<TestComponent>(entity));
		Assert.AreEqual(*world.Comp<TestComponent>(entity), new TestComponent());
		
		world.DestroyEntity(entity);
		Assert.AreEqual(2, world.currentEntityCount);
	}

	[Test]
	public unsafe void TestMultipleEntityCreateAndDestroy() {
		World world = World.general;
		world.Clear();
		
		Assert.AreEqual(0, world.currentEntityCount);
		world.CreateEntities(100, e => Assert.GreaterOrEqual(e.id, 0));
		Assert.AreEqual(100, world.currentEntityCount);
		Assert.AreEqual(0, world.GetTotalComponentCount());
		
		world.CreateEntities<TestComponent>(100);
		Assert.AreEqual(200, world.currentEntityCount);
		Assert.AreEqual(100, world.GetTotalComponentCount());
		
		world.CreateEntitiesForeachComp<TestComponent>(100, _ => { });
		Assert.AreEqual(300, world.currentEntityCount);
		Assert.AreEqual(200, world.GetTotalComponentCount());
		
		world.DestroyEntities(null);
		Assert.AreEqual(200, world.currentEntityCount);
		Assert.AreEqual(200, world.GetTotalComponentCount());
		
		world.DestroyEntities(world.GetArchetype<TestComponent>());
		Assert.AreEqual(0, world.currentEntityCount);
		Assert.AreEqual(0, world.GetTotalComponentCount());
	}

	[Test]
	public unsafe void TestEntityIteration() {
		World world = World.general;
		world.Clear();
		
		Assert.AreEqual(0, world.currentEntityCount);
		world.CreateEntities<TestComponent>(100);
		world.CreateEntities<TestComponentB>(200);
		world.CreateEntities<TestComponent, TestComponentB>(300);
		
		Assert.AreEqual(600, world.currentEntityCount);
		Assert.AreEqual(900, world.GetTotalComponentCount());
		Assert.AreEqual(2, world.GetUniqueComponentCount());

		int c = 0;
		world.Foreach<TestComponent, TestComponentB>((a, b) => c++);
		Assert.AreEqual(300, c);
		
		c = 0;
		world.Foreach<TestComponent>(a => c++);
		Assert.AreEqual(400, c);
		
		c = 0;
		world.Foreach<TestComponentB>(b => c++);
		Assert.AreEqual(500, c);
		
		c = 0;
		world.Foreach<All<TestComponent, TestComponentB>, TestComponent>(a => c++);
		Assert.AreEqual(300, c);
		
		c = 0;
		world.Foreach<Any<TestComponent, TestComponentB>, TestComponent>(a => c++);
		Assert.AreEqual(400, c);
		
		c = 0;
		world.Foreach<None<TestComponentB>, TestComponent>(a => c++);
		Assert.AreEqual(100, c);
		
		c = 0;
		world.Foreach<None<TestComponent, TestComponentB>, TestComponent>(a => c++);
		Assert.AreEqual(0, c);
	}
}

public record struct TestComponent : IComponent {
	public int a = 66;
	public float b = 31;
	public TestComponent(int a, float b) {
		this.a = a;
		this.b = b;
	}
}

public record struct TestComponentB : IComponent {
	public int a = 32;
	public float b = 12;
	public TestComponentB(int a, float b) {
		this.a = a;
		this.b = b;
	}
}