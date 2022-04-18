using NUnit.Framework;
using Quartz.CoreCs.other;
using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.identifiers;

namespace Quartz.Tests.tests.ecs.components; 

[TestFixture]
public class ComponentTypeArrayTests {
	[TestCase(1)]
	[TestCase(4)]
	[TestCase(16)]
	[TestCase(23)]
	public void TestCreateInstance(int c) {
		ComponentType[] componentTypes = new ComponentType[c];
		for (int i = 0; i < c; i++) componentTypes[i] = Rand.Int(0, 50);

		ComponentTypeArray types = new(componentTypes);
		Assert.AreEqual(componentTypes.Length, types.componentCount);
		
		for (int i = 0; i < c; i++) Assert.AreEqual(componentTypes[i].typeId, types[i].typeId);
	}
	
	[Test]
	public void TestContains() {
		ComponentTypeArray arr0 = new(4, 6, 2, 11, 31, 22, 1, 0, 5);
		ComponentTypeArray arr1 = new(4, 31, 6);
		ComponentTypeArray arr2 = new(4, 31, 7);
		ComponentTypeArray arr3 = new(4, 7, 31);
		ComponentTypeArray arr4 = new();
		
		Assert.IsTrue(arr0.Contains(4));
		Assert.IsTrue(arr0.Contains(0));
		Assert.IsTrue(arr0.Contains(22));
		Assert.IsFalse(arr0.Contains(23));
		Assert.IsFalse(arr0.Contains(9));
		Assert.IsFalse(arr4.Contains(4));
		
		Assert.IsTrue(arr0.Contains(arr1));
		Assert.IsTrue(arr0.Contains(arr4));
		Assert.IsTrue(arr2.Contains(arr3));
		Assert.IsTrue(arr3.Contains(arr2));
		Assert.IsFalse(arr4.Contains(arr0));
		Assert.IsFalse(arr1.Contains(arr0));
		Assert.IsFalse(arr0.Contains(arr2));
	}

	[Test]
	public void TestMerge() {
		ComponentTypeArray arr0 = new(4, 6, 2, 11, 31, 22, 1, 0, 5);
		ComponentTypeArray arr1 = new(4, 31, 6);
		ComponentTypeArray arr2 = new(4, 31, 7);
		ComponentTypeArray arr3 = new(4, 7, 31);
		ComponentTypeArray arr4 = new();
		
		ComponentTypeArray arr0_1 = ComponentTypeArray.Merge(arr0, arr1);
		Assert.AreEqual(arr0.componentCount, arr0_1.componentCount);
		Assert.IsTrue(arr0_1.Contains(arr0));
		Assert.IsTrue(arr0_1.Contains(arr1));
		Assert.IsFalse(arr0_1.Contains(arr2));
		
		ComponentTypeArray arr1_3_4 = ComponentTypeArray.Merge(arr1, arr3, arr4);
		Assert.IsTrue(arr1_3_4.Contains(arr1));
		Assert.IsTrue(arr1_3_4.Contains(arr3));
		Assert.IsTrue(arr1_3_4.Contains(arr4));
		Assert.IsFalse(arr1_3_4.Contains(arr0));
		Assert.IsFalse(arr0_1.Contains(arr1_3_4));
		Assert.IsFalse(arr1_3_4.Contains(arr0_1));
	}
}