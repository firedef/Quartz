using System;
using NUnit.Framework;
using Quartz.CoreCs.other;
using Quartz.Ecs.ecs.archetypes;
using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.components.data;
using Quartz.Ecs.ecs.identifiers;
using Quartz.Ecs.ecs.views;
using Quartz.Tests.data.ecs;

namespace Quartz.Tests.tests.ecs.components; 

[TestFixture]
public class ComponentArrayTests {
	[Test]
	public void TestCreation() {
		Type[] types = {typeof(TestNormalComponentA), typeof(TestNormalComponentB), typeof(TestSharedComponentD)};
		NormalComponentArray normal = new(types.GetNormalComponents());
		SharedComponentArray shared = new(types.GetSharedComponents());
		
		Assert.AreEqual(2, normal.types.componentCount);
		Assert.AreEqual(2, normal.data.Length);
		
		Assert.AreEqual(1, shared.types.componentCount);
		Assert.AreEqual(1, shared.data.Length);
	}
	
	[Test]
	public void TestArchetypeComponentsCreate() {
		Type[] types = {typeof(TestNormalComponentA), typeof(TestNormalComponentB), typeof(TestSharedComponentD)};
		ArchetypeComponents components = new Archetype(types.GetNormalComponents(), types.GetSharedComponents(), new(), 0).components;
		
		Assert.AreEqual(0, components.elementCount);
		Assert.AreEqual(2, components.normalComponentCount);
		Assert.AreEqual(1, components.sharedComponentCount);
		
		Assert.AreEqual(0, components.Add(56).position);
		Assert.AreEqual(1, components.Add(33).position);
		Assert.AreEqual(2, components.Add(98).position);
		
		Assert.IsTrue(components.ContainsArchetype(components));
		
		Assert.IsTrue(components.ContainsEntityId(56));
		Assert.IsTrue(components.ContainsEntityId(98));
		Assert.IsTrue(components.ContainsEntityId(33));
		Assert.IsFalse(components.ContainsEntityId(0));
		Assert.IsFalse(components.ContainsEntityId(EntityId.@null));
		Assert.IsFalse(components.ContainsEntityId(90));
		
		Assert.IsTrue(components.ContainsComponentId(2));
		Assert.IsTrue(components.ContainsComponentId(0));
		Assert.IsTrue(components.ContainsComponentId(1));
		Assert.IsFalse(components.ContainsComponentId(4));
		Assert.IsFalse(components.ContainsComponentId(5));
		Assert.IsFalse(components.ContainsComponentId(ComponentId.@null));

		Assert.AreEqual(3, components.elementCount);
		Console.WriteLine(components.allocatedElementCount);
		components.Trim();
		Console.WriteLine(components.allocatedElementCount);
		
		Assert.AreEqual(3, components.elementCount);
		components.Clear();
		Assert.AreEqual(0, components.elementCount);
		
		components.Trim();
		Console.WriteLine(components.allocatedElementCount);
	}
	
	[Test]
	public void TestAddSameEntity() {
		Type[] types = {typeof(TestNormalComponentA), typeof(TestNormalComponentB), typeof(TestSharedComponentD)};
		ArchetypeComponents components = new Archetype(types.GetNormalComponents(), types.GetSharedComponents(), new(), 0).components;

		Assert.AreEqual(0, components.Add(666).position);
		Assert.AreEqual(ComponentId.@null, components.Add(666));
		Assert.AreEqual(1, components.elementCount);
	}
	
	[Test]
	public void TestRemoveInvalidEntity() {
		Type[] types = {typeof(TestNormalComponentA), typeof(TestNormalComponentB), typeof(TestSharedComponentD)};
		ArchetypeComponents components = new Archetype(types.GetNormalComponents(), types.GetSharedComponents(), new(), 0).components;

		components.Add(666);
		Assert.AreEqual(1, components.elementCount);
		
		Assert.IsFalse(components.Remove(777));
		Assert.AreEqual(1, components.elementCount);
		
		Assert.IsTrue(components.Remove(666));
		Assert.AreEqual(0, components.elementCount);
	}
	
	[Test]
	public unsafe void TestComponentView() {
		Type[] types = {typeof(TestNormalComponentA), typeof(TestNormalComponentB), typeof(TestSharedComponentD)};
		ArchetypeComponents components = new Archetype(types.GetNormalComponents(), types.GetSharedComponents(), new(), 0).components;

		components.Add(666);
		components.Add(432);
		components.Add(997);
		components.Add(231);
		

		var view = components.GetViewS1<TestNormalComponentB, TestNormalComponentA, TestSharedComponentD>();
		view.component0[4].a = false;
		view.component1[0].a = -534.7f;
		view.component2[0] = 0;
		view.component2[1] = 0;
		view.component2[2] = 0;
		view.component2[3] = 0;
		
		Assert.AreEqual(4, view.count);
		for (int i = 0; i < 4; i++)
			Assert.AreEqual(0, view.component2[i]);
	}
}