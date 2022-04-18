using OpenTK.Mathematics;
using Quartz.CoreCs.other.events;
using Quartz.Ecs.ecs.jobs;
using Quartz.Ecs.ecs.views;
using Quartz.utils;

namespace Quartz.graphics.render.renderers.ecs; 

public class MatrixConstructionSystem : EntitiesJobBase {
	public override void Run() {
		JobScheduler.Schedule<MatrixComponent>(RunClear, JobScheduleSettings.immediateNow);
		JobScheduler.Schedule<MatrixComponent, PositionComponent>(RunPosition, JobScheduleSettings.immediateNow);
		JobScheduler.Schedule<MatrixComponent, Rotation2DComponent>(RunRotation2D, JobScheduleSettings.immediateNow);
		JobScheduler.Schedule<MatrixComponent, ScaleComponent>(RunScale, JobScheduleSettings.immediateNow);
	}

	private static unsafe void RunClear(ComponentsView<MatrixComponent> view, JobState state) {
		for (int i = 0; i < view.count; i++) view.component0[i].value = Matrix4.Identity;
	}
	
	private static unsafe void RunPosition(ComponentsView<MatrixComponent, PositionComponent> view, JobState state) {
		for (int i = 0; i < view.count; i++) view.component0[i].value = view.component0[i].value.SetPosition(view.component1[i].value);
	}
	
	private static unsafe void RunRotation2D(ComponentsView<MatrixComponent, Rotation2DComponent> view, JobState state) {
		for (int i = 0; i < view.count; i++) view.component0[i].value = view.component0[i].value.SetRotationZ(view.component1[i].value);
	}
	
	private static unsafe void RunScale(ComponentsView<MatrixComponent, ScaleComponent> view, JobState state) {
		for (int i = 0; i < view.count; i++) view.component0[i].value = view.component0[i].value.SetScale(view.component1[i].value);
	}
	
	[ExecuteOnce]
	private static void Invoke() => Dispatcher.global.PushMultipleRepeating(new MatrixConstructionSystem().Run, EventTypes.render);

	// protected override unsafe void Run() {
	// 	World.ForeachWorld(world => {
	// 		world.Foreach<Any<PositionComponent, Rotation2DComponent, ScaleComponent>, MatrixComponent>((matrixComp) => {
	// 			matrixComp->value = Matrix4.Identity;
	// 		});
	//
	// 		world.Foreach<PositionComponent, MatrixComponent>((comp, matrixComp) => {
	// 			matrixComp->value = matrixComp->value.SetPosition(comp->value);
	// 		});
	//
	// 		world.Foreach<Rotation2DComponent, MatrixComponent>((comp, matrixComp) => {
	// 			matrixComp->value = matrixComp->value.SetRotationZ(comp->value);
	// 		});
	//
	// 		world.Foreach<ScaleComponent, MatrixComponent>((comp, matrixComp) => {
	// 			matrixComp->value = matrixComp->value.SetScale(comp->value);
	// 		});
	// 	});
	// }
}