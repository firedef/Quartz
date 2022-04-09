using System.Runtime.InteropServices;
using MathStuff;
using Quartz.graphics.shaders.materials;
using Quartz.objects.ecs.components;
using Quartz.objects.ecs.managed;
using Quartz.objects.mesh;

namespace Quartz.graphics.render.renderers.ecs; 

public record struct MeshComponent(Ref<Mesh> value) : IComponent { public Ref<Mesh> value = value; }
public record struct AabbComponent(rect value) : IComponent { public rect value = value; }

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct RendererComponent(Ref<Material> material, short order, RenderingPass pass, bool enabled) : IComponent, IDisposable {
	public Ref<Material> material = material;
	public short order = order;
	public RenderingPass pass = pass;
	public bool enabled = enabled;
	
	public void Dispose() => material.Dispose();
}
