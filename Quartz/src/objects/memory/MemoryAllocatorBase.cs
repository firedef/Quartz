namespace Quartz.objects.memory; 

public abstract class MemoryAllocatorBase {
	public abstract int GetAllocatedBytes();
	
	public abstract unsafe void* Allocate(int byteCount);
	public abstract unsafe void Free(void* ptr);
}