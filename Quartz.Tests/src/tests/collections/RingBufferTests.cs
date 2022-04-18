using NUnit.Framework;
using Quartz.collections;
using Quartz.CoreCs.collections;

namespace Quartz.Tests.tests.collections; 

[TestFixture]
public class RingBufferTests {
	[Test]
	public void AddRemoveTest() {
		RingBuffer<int> buffer = new(32);
		buffer.Add(4);
		buffer.Add(9);
		
		Assert.AreEqual(2, buffer.count);
		Assert.AreEqual(2, buffer.pos);
		Assert.AreEqual(9, buffer[0]);
		Assert.AreEqual(4, buffer[1]);

		Assert.AreEqual(9, buffer.Pop());
		Assert.AreEqual(4, buffer.Pop());
		Assert.AreEqual(0, buffer.count);
		Assert.AreEqual(0, buffer.pos);
	}

	[Test]
	public void OverflowTest() {
		RingBuffer<int> buffer = new(32);
		for (int i = 1; i <= 40; i++) buffer.Add(i * 4);
		
		Assert.AreEqual(40, buffer.count);
		Assert.AreEqual(8, buffer.pos);
		Assert.IsTrue(buffer.isOverflowed);
		
		Assert.AreEqual(40 * 4, buffer[0]);
		Assert.AreEqual(39 * 4, buffer[1]);
		Assert.AreEqual(38 * 4, buffer[2]);
		Assert.AreEqual(37 * 4, buffer[3]);
		Assert.AreEqual(36 * 4, buffer[4]);
		Assert.AreEqual(35 * 4, buffer[5]);
	}

	[Test]
	public void EnumeratorTest() {
		RingBuffer<int> buffer = new(32);
		for (int i = 1; i <= 40; i++) buffer.Add(i * 4);

		int c = 0;
		foreach (int i in buffer) {
			Assert.AreEqual((40 - c) * 4, i);
			c++;
		}
		Assert.AreEqual(c, buffer.capacity);
	}
}