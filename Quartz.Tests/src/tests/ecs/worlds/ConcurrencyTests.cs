using NUnit.Framework;
using Quartz.CoreCs.other.events;
using Quartz.Ecs.ecs;
using Quartz.Ecs.ecs.identifiers;
using Quartz.Ecs.ecs.worlds;
using Quartz.Tests.data.ecs;

namespace Quartz.Tests.tests.ecs.worlds; 

[TestFixture]
public class ConcurrencyTests {
	[TestCase(10)]
	[TestCase(1_000)]
	[TestCase(100_000)]
	public void TestEntities(int count) {
		World.ClearAll();
		World world = World.Create("test world");

		for (int i = 0; i < count; i++) {
			FixedUpdatePipeline.Enqueue(() => {
				EntityId entity = world.AddEntity<TestNormalComponentA, TestSharedComponentD>();
				entity.Set(new TestNormalComponentH());
			}, null);
		}
		
		FixedUpdatePipeline.Step();
		
		Assert.AreEqual(count, world.entityCount);
		Assert.AreEqual(2, world.archetypeCount);
		
		for (int i = 0; i < count / 2; i++) {
			int i1 = i;
			FixedUpdatePipeline.Enqueue(() => {
				world.DestroyEntity(i1);
			}, null);
		}
		
		FixedUpdatePipeline.Step();
		Assert.AreEqual(count / 2, world.entityCount);
		
		world.DestroyWorld();
	}
}