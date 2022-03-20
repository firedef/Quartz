using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL.Compatibility;
using Quartz.debug.log;

namespace Quartz.collections.buffers; 

public struct IBO : IDisposable {
	public static readonly IBO empty = new();

	public static IBO boundBuffer = empty;
	public const BufferTargetARB target = BufferTargetARB.ElementArrayBuffer;
	
	public uint handle;
	public bool isGenerated => handle != 0;
	
	public IBO() => handle = 0;
	public IBO(uint handle) => this.handle = handle;
	public IBO(int handle) => this.handle = (uint)handle;

	public static implicit operator IBO(int v) => new(v);
	public static implicit operator IBO(uint v) => new(v);
	public static implicit operator int(IBO v) => (int) v.handle;
	public static implicit operator uint(IBO v) => v.handle;
	
	public static implicit operator IBO(BufferHandle v) => v.Handle;
	public static implicit operator BufferHandle(IBO v) => new(v);

	public void Bind() {
		if (boundBuffer.handle == handle) return;
		boundBuffer = handle;
		GL.BindBuffer(target, this);
		Log.Minimal($"bound IBO {handle}");
	}

	public static IBO Generate() {
		IBO buffer = GL.GenBuffer();
		Log.Note($"generate IBO {buffer.handle}");
		return buffer;
	}

	public static void Unbind() {
		if (boundBuffer.handle == 0) return;
		Log.Minimal($"unbound IBO {boundBuffer.handle}");
		boundBuffer = empty;
		GL.BindBuffer(target, BufferHandle.Zero);
	}

	public void Dispose() {
		if (!isGenerated) return;
		if (boundBuffer.handle == handle) Unbind();
		Log.Note($"delete IBO {handle}");
		GL.DeleteBuffer(this);
		handle = 0;
	}
}