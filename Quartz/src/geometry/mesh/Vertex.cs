using MathStuff;
using MathStuff.vectors;
using OpenTK.Graphics.OpenGL.Compatibility;

namespace Quartz.geometry.mesh; 

public record struct Vertex(float3 position, float3 normal, color color, float2 uv0, float2 uv1) : IMeshVertex {
	public const int size = (3 + 3 + 2 + 2) * 4 + 1 * 4;
	
	public float3 position = position;
	public float3 normal = normal;
	public color color = color;
	public float2 uv0 = uv0;
	public float2 uv1 = uv1;

	public Vertex(float3 position, float3 normal, color color) : this(position, normal, color, float2.zero, float2.zero) { }
	public Vertex(float3 position, color color, float2 uv) : this(position, float3.front, color, uv, float2.zero) { }
	public Vertex(float3 position, color color) : this(position, float3.front, color, float2.zero, float2.zero) { }
	public Vertex(float3 position) : this(position, float3.front, color.white, float2.zero, float2.zero) { }

	public void ProcessVertexAttributes() {
		const int stride = size;

		const int offsetPosition = 0;
		const int offsetNormal = offsetPosition + 3 * sizeof(float);
		const int offsetColor = offsetNormal + 3 * sizeof(float);
		const int offsetUv0 = offsetColor + sizeof(int);
		const int offsetUv1 = offsetUv0 + 2 * sizeof(float);

		// position
		ProcessVertexAttribute(0, 3, VertexAttribPointerType.Float, false, stride, offsetPosition);
		
		// normal
		ProcessVertexAttribute(1, 3, VertexAttribPointerType.Float, false, stride, offsetNormal);
		
		// color
		ProcessVertexAttribute(2, 4, VertexAttribPointerType.UnsignedByte, true, stride, offsetColor);
		
		// uv0
		ProcessVertexAttribute(3, 2, VertexAttribPointerType.Float, false, stride, offsetUv0);
		
		// uv`
		ProcessVertexAttribute(4, 2, VertexAttribPointerType.Float, false, stride, offsetUv1);
	}

	private static void ProcessVertexAttribute(uint loc, int size_, VertexAttribPointerType type, bool normalized, int stride, int offset) {
		GL.VertexAttribPointer(loc, size_, type, normalized, stride, offset);
		GL.EnableVertexAttribArray(loc);
	}
}