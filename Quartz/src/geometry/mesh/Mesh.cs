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
		Vertex.ProcessVertexAttributes();
	}

	public void Bind() {
		vao.Bind();
		vbo.Bind();
		ibo.Bind();
	}

	private void UpdateBuffers(bool forceFullUpdate) {
		UpdateBuffer(vertices, BufferTargetARB.ArrayBuffer, BufferUsageARB.DynamicDraw, forceFullUpdate);
		UpdateBuffer(indices, BufferTargetARB.ElementArrayBuffer, BufferUsageARB.DynamicDraw, forceFullUpdate);
		updateRequired = false;
	}

	private static unsafe void UpdateBuffer<T>(HashedList<T> buffer, BufferTargetARB target, BufferUsageARB usage, bool forceFullUpdate) where T : unmanaged {
		int size = sizeof(T);
		bool sizeChanged = buffer.GetCountChange();
		if (sizeChanged || forceFullUpdate) {
			int c = buffer.count;
			GL.BufferData(target, c * size, buffer.dataPtr, usage);
			buffer.ResetChanges();
			
			Log.Note($"update mesh {target}", LogForm.renderer);
			return;
		}

		List<Range> changes = buffer.GetChanges();
		foreach (Range range in changes) {
			int start = range.Start.Value;
			int len = range.End.Value - start;
			GL.BufferSubData(target, (IntPtr)(start * size), len * size, buffer.dataPtr + start);
		}
		if (changes.Count > 0) 
			Log.Note($"update mesh {target}", LogForm.renderer);
	}

	public bool PrepareForRender() {
		if (isDisposed || vertices.count == 0 || indices.count == 0) return false;
		if (!vao.isGenerated) {
			GenBuffers();
			UpdateBuffers(true);
			return true;
		}
		
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