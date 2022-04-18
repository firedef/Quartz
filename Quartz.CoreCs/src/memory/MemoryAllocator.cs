using Quartz.CoreCs.native;
using Quartz.CoreCs.other;

namespace Quartz.CoreCs.memory; 

public static class MemoryAllocator {
	public static ulong currentAllocated => QuartzNative.GetCurrentAllocatedBytes();
	public static ulong totalAllocated => QuartzNative.GetTotalAllocatedBytes();
	public static ulong allocatedSinceLastCleanup => QuartzNative.GetAllocatedBytesSinceLastCleanup();
	public static int allocatedPerRareUpdate;

	public static unsafe void* Allocate(int bytes) {
		allocatedPerRareUpdate += bytes;
		void* ptr = QuartzNative.Allocate((uint) bytes);
		return ptr;
	}

	public static unsafe void Free(void* ptr) {
		QuartzNative.Free(ptr);
	}

	public static unsafe void* Resize(void* ptr, int newSizeBytes) {
		if (ptr == null) return Allocate(newSizeBytes);
		allocatedPerRareUpdate += newSizeBytes;

		void* ptrNew = QuartzNative.Resize(ptr, (uint)newSizeBytes);
		return ptrNew;
	}

	public static unsafe void MemCpy<T>(T* dest, T* src, int count) where T : unmanaged => QuartzNative.MemCpy(dest, src, (uint)(count * sizeof(T))); 

	public static string ToStringMarkup() => $"\n[b]" +
	                                         $"current: {ToStringDataVal(currentAllocated)}\n" +
	                                         $"total: {ToStringDataVal(totalAllocated)}\n" +
	                                         $"per rare update: {ToStringDataVal((ulong)allocatedPerRareUpdate)}\n" +
	                                         $"[/]";

	private static string ToStringDataVal(ulong v) => $"[yellow]{v.ToStringData()}[/]";
}