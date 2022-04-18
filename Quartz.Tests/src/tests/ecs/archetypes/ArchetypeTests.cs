using System;
using NUnit.Framework;
using Quartz.Ecs.ecs.archetypes;
using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.identifiers;
using Quartz.Tests.data.ecs;

namespace Quartz.Tests.tests.ecs.archetypes; 

[TestFixture]
public class ArchetypeTests {
	[Test]
	public void TestArchetypesCreate() {
		ArchetypeRoot root = new();
		int i = 0;
		ArchetypeRoot.OnArchetypeCreate += _ => i++;
		
		Assert.AreEqual(0, i);
		Archetype? arch = root.TryGetArchetype(typeof(TestNormalComponentB), typeof(TestSharedComponentD));
		Assert.AreEqual(1, i);
		
		Assert.NotNull(arch);
		Assert.AreEqual(2, arch!.normalComponentCount);
		Assert.AreEqual(1, arch.sharedComponentCount);
	}

	[Test]
	public void TestEntityAdd() {
		ArchetypeRoot root = new();
		Archetype? arch = root.TryGetArchetype(typeof(TestNormalComponentB), typeof(TestSharedComponentD));
		
		Assert.NotNull(arch);
		Assert.AreEqual(0, arch!.entityCount);
		
		Assert.AreEqual(0, arch.AddEntity(666).position);
		Assert.AreEqual(1, arch.AddEntity(777).position);
		Assert.AreEqual(uint.MaxValue, arch.AddEntity(777).position);
		
		Assert.AreEqual(2, arch.entityCount);
		Assert.AreEqual(2, arch.components.entityComponentMapKeys);
		Assert.AreEqual(2, arch.components.entityComponentMapValues);
		
		Assert.IsTrue(arch.RemoveEntity(666));
		Assert.AreEqual(1, arch.entityCount);
		Assert.AreEqual(1, arch.components.entityComponentMapKeys);
		Assert.AreEqual(1, arch.components.entityComponentMapValues);
		
		Assert.IsFalse(arch.RemoveEntity(666));
		Assert.AreEqual(1, arch.entityCount);
		Assert.AreEqual(1, arch.components.entityComponentMapKeys);
		Assert.AreEqual(1, arch.components.entityComponentMapValues);
		
		Assert.IsFalse(arch.RemoveEntity(42));
		Assert.AreEqual(1, arch.entityCount);
		Assert.AreEqual(1, arch.components.entityComponentMapKeys);
		Assert.AreEqual(1, arch.components.entityComponentMapValues);
		
		Assert.IsTrue(arch.RemoveEntity(777));
		Assert.AreEqual(0, arch.entityCount);
		Assert.AreEqual(0, arch.components.entityComponentMapKeys);
		Assert.AreEqual(0, arch.components.entityComponentMapValues);
		
		Assert.IsFalse(arch.RemoveEntity(777));
		Assert.AreEqual(0, arch.entityCount);
		Assert.AreEqual(0, arch.components.entityComponentMapKeys);
		Assert.AreEqual(0, arch.components.entityComponentMapValues);
		
		Assert.AreEqual(0, arch.AddEntity(999).position);
		Assert.AreEqual(1, arch.AddEntity(666).position);
		Assert.AreEqual(2, arch.AddEntity(42).position);
		Assert.AreEqual(3, arch.entityCount);
		Assert.AreEqual(3, arch.components.entityComponentMapKeys);
		Assert.AreEqual(3, arch.components.entityComponentMapValues);
		
		Console.WriteLine(arch.allocatedEntityCount);
		arch.Clear();
		Assert.AreEqual(0, arch.entityCount);
		Assert.AreEqual(0, arch.components.entityComponentMapKeys);
		Assert.AreEqual(0, arch.components.entityComponentMapValues);

		arch.Cleanup();
		Console.WriteLine(arch.allocatedEntityCount);
		
		Assert.AreEqual(0, arch.AddEntity(999).position);
		Assert.AreEqual(1, arch.AddEntity(666).position);
		Assert.AreEqual(2, arch.AddEntity(42).position);
		Assert.AreEqual(3, arch.entityCount);
		Assert.AreEqual(3, arch.components.entityComponentMapKeys);
		Assert.AreEqual(3, arch.components.entityComponentMapValues);
		Console.WriteLine(arch.allocatedEntityCount);
	}

	[Test]
	public unsafe void TestEntityMove() {
		ArchetypeRoot root = new();
		EntityId e0 = 666;
		
		Assert.AreEqual(0, root.archetypeCount);
		TestNormalComponentA* ptr0 = (TestNormalComponentA*) root.GetOrAddComponent(e0, typeof(TestNormalComponentA).Get());
		Assert.AreNotEqual(0, (long) ptr0);
		ptr0->a = 645;
		ptr0->b = -333;
		
		ptr0 = (TestNormalComponentA*) root.GetOrAddComponent(e0, typeof(TestNormalComponentA).Get());
		Assert.AreNotEqual(0, (long) ptr0);
		Assert.AreEqual(645, ptr0->a);
		Assert.AreEqual(-333, ptr0->b);
		
		Assert.AreEqual(1, root.archetypeCount);
		Assert.AreEqual(1, root.archetypes[0].entityCount);
		
		TestNormalComponentB* ptr1 = (TestNormalComponentB*) root.GetOrAddComponent(e0, typeof(TestNormalComponentB).Get());
		ptr0 = (TestNormalComponentA*) root.GetOrAddComponent(e0, typeof(TestNormalComponentA).Get());
		Assert.AreNotEqual(0, (long) ptr1);
		Assert.AreNotEqual(0, (long) ptr0);
		ptr1->a = true;
		Assert.AreEqual(645, ptr0->a);
		Assert.AreEqual(-333, ptr0->b);
		
		Assert.AreEqual(2, root.archetypeCount);
		Assert.AreEqual(0, root.archetypes[0].entityCount);
		Assert.AreEqual(1, root.archetypes[1].entityCount);

		root.RemoveComponent(e0, typeof(TestNormalComponentB).Get());
		ptr0 = (TestNormalComponentA*) root.GetOrAddComponent(e0, typeof(TestNormalComponentA).Get());
		Assert.AreNotEqual(0, (long) ptr1);
		Assert.AreNotEqual(0, (long) ptr0);
		Assert.AreEqual(645, ptr0->a);
		Assert.AreEqual(-333, ptr0->b);
		
		Assert.AreEqual(2, root.archetypeCount);
		Assert.AreEqual(1, root.archetypes[0].entityCount);
		Assert.AreEqual(0, root.archetypes[1].entityCount);
		
		root.RemoveComponent(e0, typeof(TestNormalComponentA).Get());
		Assert.AreEqual(2, root.archetypeCount);
		Assert.AreEqual(0, root.archetypes[0].entityCount);
		Assert.AreEqual(0, root.archetypes[1].entityCount);
	}
}