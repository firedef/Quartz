using MathStuff.vectors;
using Quartz.objects.ecs.components.types.graphics;
using Quartz.objects.ecs.components.types.transform;
using Quartz.objects.ecs.world;
using Quartz.other;
using Quartz.other.events;

namespace Quartz.objects.ecs.systems.types; 

public class OcclusionSystem : EntitySystem, IAutoEntitySystem {
	public EventTypes types => EventTypes.fixedUpdate;
	public bool repeating => true;
	public bool continueInvoke => true;
	public float lifetime => float.MaxValue;
	public bool invokeWhileInactive => false;

	public override unsafe void Run(World world) {
		world.Foreach<RenderableComponent, OcclusionComponent, TransformComponent>((rend, occlusion, tr) => {
			float3 pos = tr->position;
			rend->enabled = pos.x > -20 && pos.x < 20 && pos.y > -20 && pos.y < 20;

			tr->position = pos + new float3(1f, 0);
		});
	}
}