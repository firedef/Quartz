using System.Runtime.InteropServices;

namespace Quartz.core; 

public static class QuartzNative {
	public const string libName = "libs/libQuartz_Core";

	[DllImport(libName)]
	public static extern unsafe void* Allocate(uint bytes);
	
	[DllImport(libName)]
	public static extern unsafe void Free(void* ptr);
	
	[DllImport(libName)]
	public static extern unsafe void CleanupMemoryAllocator();
}