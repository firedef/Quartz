using OpenTK.Graphics.OpenGL.Compatibility;
using Quartz.collections.buffers;
using Quartz.CoreCs.collections;

namespace Quartz.objects.mesh; 

public class TinyPointMesh : IDisposable, IMesh {
	public NativeList<Vertex> vertices;
	public PrimitiveType getTopology => PrimitiveType.Points;
	
	public VBO vbo;
	public VAO vao;

	public int oldVertexCount = -1;

	public TinyPointMesh(NativeList<Vertex> vertices) {
		this.vertices = vertices;
	}
	
	public TinyPointMesh(int count = 16) {
		Bind();
		vertices = new(count);
		// vertices = new(BufferTargetARB.ArrayBuffer, MapBufferAccessMask.MapWriteBit | MapBufferAccessMask.MapCoherentBit | MapBufferAccessMask.MapPersistentBit, count);
	}

	public unsafe TinyPointMesh(Vertex[] vertices) : this(vertices.Length) {
		fixed(Vertex* vPtr = vertices)
			this.vertices = new(vPtr, vertices.Length);

		this.vertices.count = vertices.Length;
	}
	

	public void Bind() {
		if (!vbo.isGenerated) {
			GenBuffers();
			return;
		}
		vao.Bind();
		vbo.Bind();
	}
	
	public bool PrepareForRender() {
		UpdateBuffers(false);
		Bind();
		return true;
	}

	private void UpdateBuffers(bool forceFullUpdate) {
		MeshUtils.UpdateBuffer(vertices, BufferTargetARB.ArrayBuffer, BufferUsageARB.DynamicDraw, vertices.capacity, oldVertexCount, forceFullUpdate);
		oldVertexCount = vertices.capacity;
	}
	
	private void GenBuffers() {
		vbo = VBO.Generate();
		vao = VAO.Generate();
		Bind();
		MeshUtils.ProcessVertexAttributes<Vertex>();
	}
	
	public void Dispose() {
		vertices.Dispose();
		vbo.Dispose();
		vao.Dispose();
	}
}