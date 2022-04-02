using MathStuff;
using OpenTK.Graphics.OpenGL.Compatibility;
using OpenTK.Mathematics;
using Quartz.graphics.camera;
using Quartz.graphics.shaders.materials;
using Quartz.objects.ecs.components.types.graphics;
using Quartz.objects.ecs.components.types.transform;
using Quartz.objects.ecs.world;
using Quartz.objects.mesh;
using Quartz.other.events;

namespace Quartz.objects.ecs.systems.types; 

public class EntityRendererSystem : EntitySystem, IAutoEntitySystem {
	public EventTypes types => EventTypes.render;
	public bool repeating => true;
	public bool continueInvoke => true;
	public float lifetime => float.MaxValue;
	public bool invokeWhileInactive => false;
	
	public override unsafe void Run(World world) {
		Camera.main!.UpdateTransform();
		Matrix4 viewProjection = Camera.main.viewProjection;
		
		world.ForeachComponent<RenderableComponent, TransformComponent>((rend, tr, entity) => {
			Mesh? mesh = rend.mesh.v;
			if (mesh == null) return;
			if (!mesh.PrepareForRender()) return;

			Material? mat = rend.material.v;
			if (mat == null) return;
			mat.shader.Bind();

			Matrix4 matrix = tr.matrix * viewProjection;

			GL.UniformMatrix4fv(0, 1, 1, (float*) &matrix);
			GL.DrawElements(mesh.topology, mesh.indices.count, DrawElementsType.UnsignedShort, 0);
		});
	}
}