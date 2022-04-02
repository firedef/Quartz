using MathStuff.vectors;
using OpenTK.Mathematics;
using Quartz.graphics.render;
using Quartz.graphics.render.renderers;

namespace Quartz.graphics.camera; 

public class Camera {
	public static Camera? main;
	
	public float3 position = new(0,0,1);
	public float3 rotation;
	public float2 scale = float2.one;
	public float orthographicSize = .01f;

	public float2 targetSize;
	
	public Matrix4 viewProjection { get; protected set; }
	
	public Renderer renderer { get; protected set; } = new();

	public Camera() {
		main ??= this;
		RenderManager.renderers.Add(renderer);
	}

	public virtual void Destroy() {
		RenderManager.renderers.Remove(renderer);
	}

	public void UpdateTransform() {
		float aspectRatio = targetSize.y / targetSize.x;
		Matrix4 proj = Matrix4.CreateOrthographic(20f / aspectRatio, -20f, .001f, 1000f);
		Matrix4 view = Matrix4.LookAt(new(position.x, position.y, -10), new(position.x, position.y, 0), new(0, -1, 0));
		viewProjection = view * proj;
	}
}