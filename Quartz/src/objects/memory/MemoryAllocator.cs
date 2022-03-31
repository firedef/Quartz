using Quartz.core;
using Quartz.debug.log;
using Quartz.other;

namespace Quartz.objects.memory; 

public static class MemoryAllocator {
	public static ulong currentAllocated => QuartzNative.GetCurrentAllocatedBytes();
	public static ulong totalAllocated => QuartzNative.GetTotalAllocatedBytes();
	public static ulong allocatedSinceLastCleanup => QuartzNative.GetAllocatedBytesSinceLastCleanup();
	public static int allocatedPerRareUpdate;
	
	public static unsafe void* Allocate(int bytes) {
		allocatedPerRareUpdate += bytes;
		return QuartzNative.Allocate((uint) bytes);
	}
	
	public static unsafe void Free(void* ptr) => QuartzNative.Free(ptr);

	public static unsafe void* Resize(void* ptr, int newSizeBytes) {
		if (ptr == null) return Allocate(newSizeBytes);
		allocatedPerRareUpdate += newSizeBytes;

		return QuartzNative.Resize(ptr, (uint)newSizeBytes);
	}

	public static void Cleanup() {
		QuartzNative.CleanupMemoryAllocator();
		Log.Note($"memoryAllocator cleanup");
	}

	public static void OnRareUpdate(float deltatime) {
		Log.Print(ToStringMarkup());
		allocatedPerRareUpdate = 0;
		if (Rand.Bool(.1f)) Cleanup();
	}

	public static string ToStringMarkup() => $"\n[b]" +
	                                         $"current: {ToStringDataVal(currentAllocated)}\n" +
	                                         $"total: {ToStringDataVal(totalAllocated)}\n" +
	                                         $"per rare update: {ToStringDataVal((ulong)allocatedPerRareUpdate)}\n" +
	                                         $"[/]";

	private static string ToStringDataVal(ulong v) => $"[yellow]{v.ToStringData()}[/]";
}