using System.Runtime.InteropServices;
using Quartz.CoreCs.memory;

namespace Quartz.Ecs.memory; 

public static class EcsMemoryAllocator {
	public static unsafe void* Alloc(int bytes) => MemoryAllocator.Allocate(bytes);
	public static unsafe void Free(void* ptr) => MemoryAllocator.Free(ptr);
	public static unsafe void* Realloc(void* old, int bytes) => MemoryAllocator.Resize(old, bytes);

	public static unsafe T* Alloc<T>(int elements) where T : unmanaged => (T*)Alloc(elements * sizeof(T));
	public static unsafe void Free<T>(T* ptr) where T : unmanaged => Free((void*) ptr);
	public static unsafe T* Realloc<T>(T* old, int elements) where T : unmanaged => (T*) Realloc((void*) old, elements * sizeof(T));
}