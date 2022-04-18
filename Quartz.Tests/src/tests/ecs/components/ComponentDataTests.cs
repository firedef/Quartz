using System;
using NUnit.Framework;
using Quartz.Ecs.ecs.attributes;
using Quartz.Ecs.ecs.components;
using Quartz.Tests.data.ecs;

namespace Quartz.Tests.tests.ecs.components; 

[TestFixture]
public class ComponentDataTests {
	[Test]
	public void TestConvert() {
		ComponentTypeArray normalComponentsA = typeof(TestNormalComponentA).GetRequiredNormalComponents();
		ComponentTypeArray normalComponentsB = typeof(TestNormalComponentB).GetRequiredNormalComponents();
		ComponentTypeArray normalComponentsC = typeof(TestNormalComponentC).GetRequiredNormalComponents();
		
		Assert.AreEqual(1, normalComponentsA.componentCount);
		Assert.AreEqual(1, normalComponentsB.componentCount);
		Assert.AreEqual(2, normalComponentsC.componentCount);
		
		Assert.IsTrue(normalComponentsC.Contains(normalComponentsA));
		Assert.IsFalse(normalComponentsC.Contains(normalComponentsB));
		Assert.IsFalse(normalComponentsA.Contains(normalComponentsC));
		Assert.IsFalse(normalComponentsA.Contains(normalComponentsB));
		
		Assert.AreEqual(typeof(TestNormalComponentA), normalComponentsA[0].data.type);
		Assert.AreEqual(typeof(TestNormalComponentB), normalComponentsB[0].data.type);
	}
	
	[Test]
	public void TestKinds() {
		ComponentTypeArray normalComponentsA = typeof(TestNormalComponentA).GetRequiredNormalComponents();
		ComponentTypeArray normalComponentsB = typeof(TestNormalComponentB).GetRequiredNormalComponents();
		ComponentTypeArray normalComponentsC = typeof(TestNormalComponentC).GetRequiredNormalComponents();
		ComponentTypeArray normalComponentsD = typeof(TestSharedComponentD).GetRequiredNormalComponents();
		ComponentTypeArray normalComponentsE = typeof(TestNormalComponentE).GetRequiredNormalComponents();
		ComponentTypeArray normalComponentsF = typeof(TestNormalComponentF).GetRequiredNormalComponents();
		ComponentTypeArray normalComponentsG = typeof(TestNormalComponentG).GetRequiredNormalComponents();
		ComponentTypeArray normalComponentsH = typeof(TestNormalComponentH).GetRequiredNormalComponents();
		
		ComponentTypeArray sharedComponentsA = typeof(TestNormalComponentA).GetRequiredSharedComponents();
		ComponentTypeArray sharedComponentsB = typeof(TestNormalComponentB).GetRequiredSharedComponents();
		ComponentTypeArray sharedComponentsC = typeof(TestNormalComponentC).GetRequiredSharedComponents();
		ComponentTypeArray sharedComponentsD = typeof(TestSharedComponentD).GetRequiredSharedComponents();
		ComponentTypeArray sharedComponentsE = typeof(TestNormalComponentE).GetRequiredSharedComponents();
		ComponentTypeArray sharedComponentsF = typeof(TestNormalComponentF).GetRequiredSharedComponents();
		ComponentTypeArray sharedComponentsG = typeof(TestNormalComponentG).GetRequiredSharedComponents();
		ComponentTypeArray sharedComponentsH = typeof(TestNormalComponentH).GetRequiredSharedComponents();
		
		Assert.AreEqual(1, normalComponentsA.componentCount);
		Assert.AreEqual(1, normalComponentsB.componentCount);
		Assert.AreEqual(2, normalComponentsC.componentCount);
		Assert.AreEqual(1, normalComponentsD.componentCount);
		Assert.AreEqual(2, normalComponentsE.componentCount);
		Assert.AreEqual(3, normalComponentsF.componentCount);
		Assert.AreEqual(2, normalComponentsG.componentCount);
		Assert.AreEqual(2, normalComponentsH.componentCount);
		
		Assert.AreEqual(0, sharedComponentsA.componentCount);
		Assert.AreEqual(0, sharedComponentsB.componentCount);
		Assert.AreEqual(0, sharedComponentsC.componentCount);
		Assert.AreEqual(1, sharedComponentsD.componentCount);
		Assert.AreEqual(1, sharedComponentsE.componentCount);
		Assert.AreEqual(1, sharedComponentsF.componentCount);
		Assert.AreEqual(0, sharedComponentsG.componentCount);
		Assert.AreEqual(0, sharedComponentsH.componentCount);
		
		Assert.IsTrue(normalComponentsH.Contains(normalComponentsG));
		Assert.IsTrue(normalComponentsG.Contains(normalComponentsH));
		Assert.IsTrue(sharedComponentsH.Contains(sharedComponentsG));
		Assert.IsTrue(sharedComponentsG.Contains(sharedComponentsH));
		
		Assert.IsTrue(normalComponentsF.Contains(normalComponentsA));
		Assert.IsFalse(normalComponentsA.Contains(normalComponentsF));
		
		Assert.IsTrue(sharedComponentsE.Contains(sharedComponentsD));
		Assert.IsTrue(sharedComponentsE.Contains(sharedComponentsA));
	}

	[Test]
	public void TestConvertMultipleTypes() {
		Type[] t0 = {typeof(TestNormalComponentC), typeof(TestNormalComponentF)};
		Type[] t1 = {typeof(TestNormalComponentG), typeof(TestNormalComponentH)};
		Type[] t2 = {typeof(TestNormalComponentA)};
		Type[] t3 = {typeof(TestNormalComponentA), typeof(TestNormalComponentB)};
		Type[] t4 = {typeof(TestNormalComponentG)};
		
		Assert.IsTrue(t0.GetNormalComponents().Contains(t2.GetNormalComponents()));
		Assert.IsTrue(t0.GetSharedComponents().Contains(t2.GetSharedComponents()));
		Assert.IsFalse(t0.GetNormalComponents().Contains(t3.GetNormalComponents()));
		Assert.IsFalse(t2.GetNormalComponents().Contains(t3.GetNormalComponents()));
		Assert.IsTrue(t3.GetNormalComponents().Contains(t2.GetNormalComponents()));
		
		Assert.IsTrue(t1.GetNormalComponents().Contains(t4.GetNormalComponents()));
		Assert.IsTrue(t4.GetNormalComponents().Contains(t1.GetNormalComponents()));
	}
}
