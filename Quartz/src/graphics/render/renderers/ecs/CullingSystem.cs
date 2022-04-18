using MathStuff;
using MathStuff.vectors;
using Quartz.CoreCs.other.events;
using Quartz.Ecs.ecs.jobs;
using Quartz.Ecs.ecs.views;
using Quartz.Ecs.ecs.worlds;
using Quartz.graphics.camera;

namespace Quartz.graphics.render.renderers.ecs;

public class CullingSystem : EntitiesJobBase {
	public override void Run() {
		if (Camera.main == null) return;
		JobScheduler.Schedule<AabbComponent, PositionComponent>(RunAabbUpdate, JobScheduleSettings.@default);
		JobScheduler.Schedule<AabbComponent, RendererComponent>(RunRendererUpdate, JobScheduleSettings.@default);
		this.Schedule(JobScheduleSettings.@default with{tickDelay = 10});
	}

	private static unsafe void RunAabbUpdate(ComponentsView<AabbComponent, PositionComponent> components, JobState state) {
		int c = components.count;
		for (int i = 0; i < c; i++) {
			float3 pos = components.component1[i].value;
			components.component0[i].value = new(pos.x, pos.y, 1, 1);
		}
	}
	
	private static unsafe void RunRendererUpdate(ComponentsView<AabbComponent, RendererComponent> components, JobState state) {
		rect cameraAabb = Camera.main!.worldBounds;
		
		int c = components.count;
		for (int i = 0; i < c; i++)
			components.component1[i].enabled = components.component0[i].value.Contains(cameraAabb);
	}
	
	// protected override unsafe void Run() {
	// 	World.ForeachWorld(world => {
	// 		if (Camera.main == null) return;
	// 		rect cameraAabb = Camera.main.worldBounds;
	// 	
	// 		world.Foreach<AabbComponent, PositionComponent>((aabb, pos) => {
	// 			aabb->value = new(pos->value.x, pos->value.y, 1, 1);
	// 		});
	// 	
	// 		// world.Foreach<AabbComponent, RendererComponent>((aabb, renderer) => {
	// 		// 	renderer->enabled = aabb->value.Contains(cameraAabb);
	// 		// });
	// 	});
	// 	
	// 	this.DispatchExecution(10, mainThread: false);
	// }

	[ExecuteOnce]
	private static void Invoke() => new CullingSystem().Run();
}