using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL.Compatibility;
using Quartz.debug.log;

namespace Quartz.collections.buffers; 

public struct VBO : IDisposable {
	public static readonly VBO empty = new();

	public static VBO boundBuffer = empty;
	public const BufferTargetARB target = BufferTargetARB.ArrayBuffer;
	
	public uint handle;
	public bool isGenerated => handle != 0;
	
	public VBO() => handle = 0;
	public VBO(uint handle) => this.handle = handle;
	public VBO(int handle) => this.handle = (uint)handle;

	public static implicit operator VBO(int v) => new(v);
	public static implicit operator VBO(uint v) => new(v);
	public static implicit operator int(VBO v) => (int) v.handle;
	public static implicit operator uint(VBO v) => v.handle;
	
	public static implicit operator VBO(BufferHandle v) => v.Handle;
	public static implicit operator BufferHandle(VBO v) => new(v);

	public void Bind() {
		if (boundBuffer.handle == handle) return;
		boundBuffer = handle;
		GL.BindBuffer(target, this);
		//Log.Minimal($"bound VBO {handle}");
	}

	public static VBO Generate() {
		VBO buffer = GL.GenBuffer();
		Log.Note($"generate VBO {buffer.handle}");
		return buffer;
	}

	public static void Unbind() {
		if (boundBuffer.handle == 0) return;
		//Log.Minimal($"unbound VBO {boundBuffer.handle}");
		boundBuffer = empty;
		GL.BindBuffer(target, BufferHandle.Zero);
	}

	public void Dispose() {
		if (!isGenerated) return;
		if (boundBuffer.handle == handle) Unbind();
		Log.Note($"delete VBO {handle}");
		GL.DeleteBuffer(this);
		handle = 0;
	}
}

