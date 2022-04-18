using OpenTK.Graphics.OpenGL.Compatibility;
using OpenTK.Mathematics;
using Quartz.CoreCs.other;
using Quartz.CoreCs.other.events;
using Quartz.Ecs.ecs.jobs;
using Quartz.Ecs.ecs.views;
using Quartz.graphics.camera;
using Quartz.graphics.shaders.materials;
using Quartz.objects.mesh;

namespace Quartz.graphics.render.renderers.ecs; 

public class EntityRendererSystem : EntitiesJobBase {
	public override void Run() {
		Camera.main!.UpdateTransform();

		JobScheduler.Schedule<RendererComponent, MatrixComponent, MeshComponent>(RunRenderer, JobScheduleSettings.immediateNow with
		{
			interactableWorlds = State.any, 
			activeWorlds = State.any, 
			visibleWorlds = State.@on
		});
	}

	private static unsafe void RunRenderer(ComponentsView<RendererComponent, MatrixComponent, MeshComponent> view, JobState state) {
		Matrix4 viewProjection = Camera.main!.viewProjection;

		for (int i = 0; i < view.count; i++) {
			if (!view.component0[i].enabled) continue;
			
			Mesh? mesh = view.component2[i].value.v;
			Material? material = view.component0[i].material.v;
			
			if (mesh == null || material == null) return;
			if (!mesh.PrepareForRender()) return;
			material.shader.Bind();
			
			Matrix4 matrix = view.component1[i].value * viewProjection;
			GL.UniformMatrix4fv(0, 1, 1, (float*) &matrix);
			GL.DrawElements(mesh.topology, mesh.indices.count, DrawElementsType.UnsignedShort, 0);
		}
	}
	
	[ExecuteOnce]
	private static void Invoke() => Dispatcher.global.PushMultipleRepeating(new EntityRendererSystem().Run, EventTypes.render);
}