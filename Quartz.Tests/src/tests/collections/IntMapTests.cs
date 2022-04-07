using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using Quartz.collections;
using Quartz.core.collections;
using Quartz.other;
using Quartz.Tests.utils;

namespace Quartz.Tests.tests.collections; 

[TestFixture]
public class IntMapTests {
	[Test]
	public void CreateAndDisposeTest() {
		IntMap map = new();
		map.Dispose();

		DualIntMap dualMap = new();
	}

	[TestCase(1)]
	[TestCase(10)]
	[TestCase(100)]
	[TestCase(1000)]
	[TestCase(5_000)]
	[TestCase(10_000)]
	[TestCase(25_000)]
	[TestCase(50_000)]
	[TestCase(100_000)]
	[Parallelizable]
	public void AddAndGetTest(int count) {
		List<IntInt> items = new();
		uint[] keys = RandomData.RndUInts(count, 5);
		uint[] values = RandomData.RndUInts(count, 5);

		DualIntMap map = new();
		
		for (int i = 0; i < count;) {
			uint k = keys[i];
			uint v = values[i];
			items.Add(new(k,v));
			map.Set(k,v);
			
			i++;
		}

		for (int i = 0; i < count; i++) {
			uint k0 = items[i].key;
			uint v0 = items[i].val;
		
			uint k1 = map.GetKey(v0);
			uint v1 = map.GetVal(k0);
			
			Assert.AreEqual(k0, k1);
			Assert.AreEqual(v0, v1);
		}
	}
}