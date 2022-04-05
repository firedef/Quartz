using Quartz.objects.ecs.systems;
using Quartz.objects.ecs.world;

namespace Quartz.graphics.render.renderers.ecs;

[ExecuteByEventPipeline(250)]
public class CullingSystem : EntitySystem {
	protected override unsafe void Run(World world) {
		world.Foreach<AabbComponent, RendererComponent>((aabb, renderer) => {
			
		});
		Console.WriteLine("amogus");
	}
}