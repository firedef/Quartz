using System.Buffers;
using MathStuff;

namespace Quartz.objects.memory; 

public class MemoryAllocator : MemoryAllocatorBase {
	public int allocatedBytes;

	public override int GetAllocatedBytes() => allocatedBytes;
	public override unsafe void* Allocate(int byteCount) {
		return null;
	}
	public override unsafe void Free(void* ptr) { throw new NotImplementedException(); }
}