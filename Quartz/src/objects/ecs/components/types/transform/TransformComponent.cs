using MathStuff.vectors;
using OpenTK.Mathematics;
using Quartz.utils;

namespace Quartz.objects.ecs.components.types.transform; 

public record struct TransformComponent(Matrix4 matrix, float _rotationZ) : IComponent {
	public Matrix4 matrix = matrix;
	public float _rotationZ = _rotationZ;

	public TransformComponent(float3 pos, float3 scale, float rot) : this() {
		position = pos;
		this.scale = scale;
		rotationZ = rot;
	}
	
	public TransformComponent(float3 pos, float3 scale) : this() {
		matrix = Matrix4.CreateScale(scale.x, scale.y, scale.z) * Matrix4.CreateTranslation(pos.x, pos.y, pos.z);
		//position = pos;
		//this.scale = scale;
	}

	public float rotationZ {
		get => _rotationZ;
		set => matrix = matrix.SetRotationZ(value);
	}

	public float3 position {
		get => matrix.Position();
		set => matrix = matrix.SetPosition(value);
	}
	public float3 scale {
		get => matrix.Scale();
		set => matrix = matrix.SetScale(value);
	}
}