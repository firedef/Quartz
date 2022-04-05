using System.Diagnostics;
using System.Runtime.InteropServices;
using MathStuff;
using Quartz.core;
using Quartz.debug.log;
using Quartz.other;
using Quartz.other.events;

namespace Quartz.objects.memory; 

public static class MemoryAllocator {
	public static ulong currentAllocated => QuartzNative.GetCurrentAllocatedBytes();
	public static ulong totalAllocated => QuartzNative.GetTotalAllocatedBytes();
	public static ulong allocatedSinceLastCleanup => QuartzNative.GetAllocatedBytesSinceLastCleanup();
	public static int allocatedPerRareUpdate;

	private static Dictionary<IntPtr, int> _sizes = new();

	public static unsafe void* Allocate(int bytes) {
		allocatedPerRareUpdate += bytes;
		//Log.Warning(Environment.StackTrace.Replace("[","[[").Replace("]","]]"));
		void* ptr = QuartzNative.Allocate((uint) bytes);

		_sizes.Add((IntPtr) ptr, bytes);
		//Console.WriteLine($"alloc {(long) ptr} {((ulong)bytes).ToStringData()}");
		return ptr;
	}

	public static unsafe void Free(void* ptr) {
		Console.WriteLine($"free {(long) ptr} {((ulong)_sizes[(IntPtr) ptr]).ToStringData()}");
		_sizes.Remove((IntPtr)ptr);
		QuartzNative.Free(ptr);
	}

	public static unsafe void* Resize(void* ptr, int newSizeBytes) {
		if (ptr == null) return Allocate(newSizeBytes);
		allocatedPerRareUpdate += newSizeBytes;

		//Log.Warning(Environment.StackTrace.Replace("[","[[").Replace("]","]]"));
		void* ptrNew = QuartzNative.Resize(ptr, (uint)newSizeBytes);
		
		//Console.WriteLine($"realloc {(long) ptr} -> {(long) ptrNew} {((ulong)_sizes[(IntPtr) ptr]).ToStringData()} -> {((ulong) newSizeBytes).ToStringData()}");
		_sizes.Remove((IntPtr)ptr);
		_sizes.Add((IntPtr) ptrNew, newSizeBytes);
		return ptrNew;
	}

	[CallRepeating(EventTypes.lowMemory)]
	public static void Cleanup() {
		QuartzNative.CleanupMemoryAllocator();
		Log.Note($"memoryAllocator cleanup");
	}

	public static unsafe void MemCpy<T>(T* dest, T* src, int count) where T : unmanaged => QuartzNative.MemCpy(dest, src, (uint)(count * sizeof(T))); 

	[CallRepeating(EventTypes.rareUpdate)]
	public static void OnRareUpdate() {
		//Log.Print(ToStringMarkup());
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