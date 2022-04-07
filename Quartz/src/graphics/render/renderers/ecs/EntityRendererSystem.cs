using OpenTK.Graphics.OpenGL.Compatibility;
using OpenTK.Mathematics;
using Quartz.graphics.camera;
using Quartz.graphics.shaders.materials;
using Quartz.objects.ecs.systems;
using Quartz.objects.ecs.world;
using Quartz.objects.mesh;
using Quartz.other.events;

namespace Quartz.graphics.render.renderers.ecs; 

public class EntityRendererSystem : EntitySystem, IAutoEntitySystem {
	public EventTypes eventTypes => EventTypes.render;
	public bool repeating => true;
	public bool continueInvoke => true;
	public float lifetime => float.MaxValue;
	public bool invokeWhileInactive => false;

	protected override unsafe void Run() {
		Camera.main!.UpdateTransform();
		Matrix4 viewProjection = Camera.main.viewProjection;
		World.ForeachWorld(world => {
			if (!world.isVisible) return;

			world.Foreach<RendererComponent, MatrixComponent, MeshComponent>((renderer, matrixComp, meshComp) => {
				if (!renderer->enabled) return;
				Mesh? mesh = meshComp->value.v;
				Material? material = renderer->material.v;
		
				if (mesh == null || material == null) return;
				if (!mesh.PrepareForRender()) return;
				material.shader.Bind();

				Matrix4 matrix = matrixComp->value * viewProjection;
				GL.UniformMatrix4fv(0, 1, 1, (float*) &matrix);
				GL.DrawElements(mesh.topology, mesh.indices.count, DrawElementsType.UnsignedShort, 0);
			});
		});
	}
}