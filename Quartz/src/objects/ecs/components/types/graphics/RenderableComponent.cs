using Quartz.graphics.render;
using Quartz.graphics.shaders.materials;
using Quartz.objects.ecs.managed;
using Quartz.objects.mesh;

namespace Quartz.objects.ecs.components.types.graphics; 

public record struct RenderableComponent: IComponent, IDisposable {
	public Ref<Material> material;
	public Ref<Mesh> mesh;
	public short order;
	public bool enabled = true;
	public RenderingPass pass;

	public RenderableComponent(Material material, Mesh mesh, short order, RenderingPass pass) {
		this.material = material;
		this.mesh = mesh;
		this.order = order;
		this.pass = pass;
	}

	public void Dispose() {
		material.Dispose();
		mesh.Dispose();
	}
}