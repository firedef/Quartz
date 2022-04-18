using MathStuff;
using MathStuff.vectors;
using OpenTK.Mathematics;
using Quartz.Ecs.ecs.attributes;
using Quartz.Ecs.ecs.components;

namespace Quartz.graphics.render.renderers.ecs;

[Component("matrix")]
public record struct MatrixComponent(Matrix4 value) : IComponent {
	public Matrix4 value = value;
	
	public void Parse(Dictionary<string, string> fields) {
		//if (fields.TryGetValue("value", out string? v)) value = new(int.Parse(v));
	}
	public void Write(Dictionary<string, string> fields) {
		//fields.Add("value", value.ToString());
	}
}

[Component("position")]
public record struct PositionComponent(float3 value) : IComponent {
	public float3 value = value;
	
	public void Parse(Dictionary<string, string> fields) {
		string? v;
		if (fields.TryGetValue("x", out v)) value.x = float.Parse(v);
		if (fields.TryGetValue("y", out v)) value.y = float.Parse(v);
		if (fields.TryGetValue("z", out v)) value.z = float.Parse(v);
	}
	public void Write(Dictionary<string, string> fields) {
		//fields.Add("value", value.ToString());
	}
}

[Component("scale")]
public record struct ScaleComponent(float3 value) : IComponent {
	public float3 value = value;
	
	public void Parse(Dictionary<string, string> fields) {
		string? v;
		if (fields.TryGetValue("x", out v)) value.x = float.Parse(v);
		if (fields.TryGetValue("y", out v)) value.y = float.Parse(v);
		if (fields.TryGetValue("y", out v)) value.z = float.Parse(v);
	}
	public void Write(Dictionary<string, string> fields) {
		//fields.Add("value", value.ToString());
	}
}

[Component("rotation")]
public record struct Rotation2DComponent(float value) : IComponent {
	public float value = value;
	
	public void Parse(Dictionary<string, string> fields) {
		string? v;
		if (fields.TryGetValue("degrees", out v)) value = float.Parse(v) / 360 * MathF.PI;
		if (fields.TryGetValue("radians", out v)) value = float.Parse(v);
	}
	public void Write(Dictionary<string, string> fields) {
		//fields.Add("value", value.ToString());
	}
}

[Component("rotation3D")]
public record struct Rotation3DComponent(float3 value) : IComponent {
	public float3 value = value;
	
	public void Parse(Dictionary<string, string> fields) {
		string? v;
		if (fields.TryGetValue("x", out v)) value.x = float.Parse(v);
		if (fields.TryGetValue("y", out v)) value.y = float.Parse(v);
		if (fields.TryGetValue("y", out v)) value.z = float.Parse(v);
	}
	public void Write(Dictionary<string, string> fields) {
		//fields.Add("value", value.ToString());
	}
}