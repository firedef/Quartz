using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL.Compatibility;

namespace Quartz.collections; 

public class GlMappedList<T> : NativeList<T> where T : unmanaged {
	public BufferTargetARB target;
	public BufferUsageARB usage = BufferUsageARB.DynamicDraw;
	public MapBufferAccessMask access;
	public BufferStorageMask storage;

	public unsafe GlMappedList(BufferTargetARB target, MapBufferAccessMask access, int capacity = 16) : base(0) {
		this.target = target;
		this.access = access;
		//this.storage = storage;
		dataPtr = (T*)Alloc(capacity);
	}
	
	protected sealed override unsafe void* Alloc(int bytes) {
		//GL.BufferData(target, bytes, null, usage);
		//return GL.MapBuffer(target, BufferAccessARB.ReadWrite);
		
		GL.BufferStorage((BufferStorageTarget) target, bytes, null, ((BufferStorageMask)access) | BufferStorageMask.ClientStorageBit);
		return GL.MapBufferRange(target, IntPtr.Zero, bytes, access);
	}
	
	protected unsafe void* AllocBase(int bytes) => NativeMemory.Alloc((nuint)bytes);
	
	protected override unsafe void* Realloc(void* p, int bytes) {
		int oldBytes = sizeof(T) * count;
		void* temp = AllocBase(oldBytes);
		MemCpy(dataPtr, temp, oldBytes);
		
		Free(dataPtr);
		dataPtr = (T*) Alloc(bytes);
		
		MemCpy(temp, dataPtr, oldBytes);
		FreeBase(temp);

		return dataPtr;
	}
	
	protected override unsafe void Free(void* p) {
		GL.UnmapBuffer(target);
	}
	protected unsafe void FreeBase(void* p) => NativeMemory.Free(p);
}