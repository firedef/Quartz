using Quartz.graphics.render;
using Quartz.graphics.shaders.materials;
using Quartz.objects.ecs.managed;
using Quartz.objects.mesh;

namespace Quartz.objects.ecs.components.types.graphics; 

public record struct RenderableComponent(Ref<Material> material, Ref<Mesh> mesh, short order, RenderingPass pass) : IComponent {
	public Ref<Material> material = material;
	public Ref<Mesh> mesh = mesh;
	public short order = order;
	public RenderingPass pass = pass;
}