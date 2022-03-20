using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL.Compatibility;
using Quartz.debug.log;
using Quartz.geometry.mesh;
using Quartz.utils;

namespace Quartz.collections.buffers; 

public struct VAO : IDisposable {
	public static readonly VAO empty = new();

	public static VAO boundBuffer = empty;
	
	public uint handle;
	public bool isGenerated => handle != 0;
	
	public VAO() => handle = 0;
	public VAO(uint handle) => this.handle = handle;
	public VAO(int handle) => this.handle = (uint)handle;

	public static implicit operator VAO(int v) => new(v);
	public static implicit operator VAO(uint v) => new(v);
	public static implicit operator int(VAO v) => (int) v.handle;
	public static implicit operator uint(VAO v) => v.handle;
	
	public static implicit operator VAO(VertexArrayHandle v) => v.Handle;
	public static implicit operator VertexArrayHandle(VAO v) => new(v);

	public void Bind() {
		if (boundBuffer.handle == handle) return;
		GL.BindVertexArray(this);
		boundBuffer = handle;
		Log.Minimal($"bound VAO {handle}");
	}

	public static VAO Generate() {
		VAO buffer = GL.GenVertexArray();
		Log.Note($"generate VAO {buffer.handle}");
		return buffer;
	}

	public static void Unbind() {
		if (boundBuffer.handle == 0) return;
		Log.Minimal($"unbound VAO {boundBuffer.handle}");
		boundBuffer = empty;
		GL.BindVertexArray(VertexArrayHandle.Zero);
	}

	public void Dispose() {
		if (!isGenerated) return;
		if (boundBuffer.handle == handle) Unbind();
		Log.Note($"delete VAO {handle}");
		GL.DeleteVertexArray(this);
		handle = 0;
	}
}