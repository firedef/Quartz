using OpenTK.Mathematics;
using Quartz.objects.ecs.filters;
using Quartz.objects.ecs.systems;
using Quartz.objects.ecs.world;
using Quartz.utils;

namespace Quartz.graphics.render.renderers.ecs; 

[ExecuteByEventPipeline(50)]
public class MatrixConstructionSystem : EntitySystem {
	protected override unsafe void Run(World world) {
		//Monitor.Enter(EntityRendererSystem._rendererLock);
		
		world.Foreach<Any<PositionComponent, Rotation2DComponent, ScaleComponent>, MatrixComponent>((matrixComp) => {
			matrixComp->value = Matrix4.Identity;
		});
	
		world.Foreach<PositionComponent, MatrixComponent>((comp, matrixComp) => {
			matrixComp->value = matrixComp->value.SetPosition(comp->value);
		});
	
		world.Foreach<Rotation2DComponent, MatrixComponent>((comp, matrixComp) => {
			matrixComp->value = matrixComp->value.SetRotationZ(comp->value);
		});
	
		world.Foreach<ScaleComponent, MatrixComponent>((comp, matrixComp) => {
			matrixComp->value = matrixComp->value.SetScale(comp->value);
		});
		
		//Monitor.Exit(EntityRendererSystem._rendererLock);
	}
}