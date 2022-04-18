using System.Runtime.InteropServices;
using MathStuff;
using Quartz.Ecs.ecs.attributes;
using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.managed;
using Quartz.graphics.shaders.materials;
using Quartz.objects.mesh;

namespace Quartz.graphics.render.renderers.ecs;

[Component("mesh")]
public record struct MeshComponent(Ref<Mesh> value) : IComponent, ICloneableComponent, IDisposable {
	public Ref<Mesh> value = value;
	
	public void Dispose() => value.Dispose();
	public unsafe void OnClone(void* _) => value.Copy();

	public void Parse(Dictionary<string, string> fields) {
		if (fields.TryGetValue("mesh", out string? v)) value = new(int.Parse(v));
	}
	public void Write(Dictionary<string, string> fields) {
		fields.Add("mesh", value.id.ToString());
	}
}

[Component("aabb")]
public record struct AabbComponent(rect value) : IComponent {
	public rect value = value;
	
	public void Parse(Dictionary<string, string> fields) {
		//if (fields.TryGetValue("value", out string? v)) value = new(int.Parse(v));
	}
	public void Write(Dictionary<string, string> fields) {
		//fields.Add("value", value.ToString());
	}
}

[Component("renderer")]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct RendererComponent(Ref<Material> material, short order, RenderingPass pass, bool enabled) : IComponent, IDisposable, ICloneableComponent {
	public Ref<Material> material = material;
	public short order = order;
	public RenderingPass pass = pass;
	public bool enabled = enabled;
	
	public void Dispose() => material.Dispose();
	public unsafe void OnClone(void* _) => material.Copy();
	
	public void Parse(Dictionary<string, string> fields) {
		//if (fields.TryGetValue("value", out string? v)) value = new(int.Parse(v));
	}
	public void Write(Dictionary<string, string> fields) {
		//fields.Add("value", value.ToString());
	}
}
