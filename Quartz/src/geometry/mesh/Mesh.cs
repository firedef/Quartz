using OpenTK.Graphics.OpenGL.Compatibility;
using Quartz.collections;
using Quartz.collections.buffers;
using Quartz.debug.log;

namespace Quartz.geometry.mesh; 

public class Mesh : MeshBase {
	public VBO vbo;
	public IBO ibo;
	public VAO vao;

	private bool isDisposed;
	public bool updateRequired;

	public Mesh(HashedList<Vertex> vertices, HashedList<ushort> indices) : base(vertices, indices) { }
	public unsafe Mesh(Vertex* vertices, ushort* indices, int vCount, int iCount) : base(vertices, indices, vCount, iCount) { }
	public Mesh(Vertex[] vertices, ushort[] indices) : base(vertices, indices) { }

	private void GenBuffers() {
		vbo = VBO.Generate();
		vao = VAO.Generate();
		ibo = IBO.Generate();
		Bind();
		MeshUtils.ProcessVertexAttributes<Vertex>();
	}

	public override void Bind() {
		if (!vao.isGenerated) {
			GenBuffers();
			UpdateBuffers(true);
			return;
		}
		
		vao.Bind();
		vbo.Bind();
		ibo.Bind();
	}

	private void UpdateBuffers(bool forceFullUpdate) {
		MeshUtils.UpdateBuffer(vertices, BufferTargetARB.ArrayBuffer, BufferUsageARB.DynamicDraw, forceFullUpdate);
		MeshUtils.UpdateBuffer(indices, BufferTargetARB.ElementArrayBuffer, BufferUsageARB.DynamicDraw, forceFullUpdate);
		updateRequired = false;
	}
	
	public override bool PrepareForRender() {
		if (isDisposed || vertices.count == 0 || indices.count == 0) return false;
		Bind();
		if (updateRequired) UpdateBuffers(false);
		return true;
	}

	protected override void Dispose(bool disposing) {
		if (!disposing) return;
		isDisposed = true;
		base.Dispose(disposing);
		vbo.Dispose();
		vao.Dispose();
		ibo.Dispose();
	}
}