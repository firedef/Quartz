using System.Runtime.InteropServices;
using Quartz.CoreCs.native.collections;

namespace Quartz.CoreCs.native; 

public static class QuartzNative {
	public const string libName = "libs/libQuartz_Core";

	[DllImport(libName)] public static extern unsafe void* Allocate(uint bytes);
	
	[DllImport(libName)] public static extern unsafe void Free(void* ptr);
	
	[DllImport(libName)] public static extern unsafe void* Resize(void* ptr, uint newSize);
	
	[DllImport(libName)] public static extern void CleanupMemoryAllocator();
	
	[DllImport(libName)] public static extern ulong GetCurrentAllocatedBytes();
	
	[DllImport(libName)] public static extern ulong GetTotalAllocatedBytes();
	
	[DllImport(libName)] public static extern ulong GetAllocatedBytesSinceLastCleanup();
	
	[DllImport(libName)] public static extern unsafe void MemCpy(void* dest, void* src, uint bytes);
	
	[DllImport(libName)] public static extern unsafe void MemSet(void* dest, int val, uint bytes);
	
	[DllImport(libName)] public static extern unsafe int MemCmp(void* dest, void* src, uint bytes);
	
	[DllImport(libName)] public static extern unsafe void* IntMap_Create();
	
	[DllImport(libName)] public static extern unsafe void IntMap_Destroy(void* ptr);
	
	[DllImport(libName)] public static extern unsafe void IntMap_Set(void* ptr, IntInt element);
	
	[DllImport(libName)] public static extern unsafe void IntMap_Remove(void* ptr, uint key);
	
	[DllImport(libName)] public static extern unsafe uint IntMap_TryGetValue(void* ptr, uint key);
	
	[DllImport(libName)] public static extern unsafe int IntMap_Count(void* ptr);
	
	[DllImport(libName)] public static extern unsafe void IntMap_Clear(void* ptr);
}
