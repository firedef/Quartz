using MathStuff;
using Quartz.graphics.camera;
using Quartz.objects.ecs.systems;
using Quartz.objects.ecs.world;

namespace Quartz.graphics.render.renderers.ecs;

[ExecuteByEventPipeline(250)]
public class CullingSystem : EntitySystem {
	protected override unsafe void Run(World world) {
		if (Camera.main == null) return;
		rect cameraAabb = Camera.main.worldBounds;
		
		world.Foreach<AabbComponent, PositionComponent>((aabb, pos) => {
			aabb->value = new(pos->value.x, pos->value.y, 1, 1);
		});
		
		world.Foreach<AabbComponent, RendererComponent>((aabb, renderer) => {
			renderer->enabled = aabb->value.Contains(cameraAabb);
		});
	}
}