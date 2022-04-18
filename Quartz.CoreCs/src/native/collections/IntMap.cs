namespace Quartz.CoreCs.native.collections; 

public class IntMap : IDisposable {
	public unsafe void* ptr;
	public unsafe int count => QuartzNative.IntMap_Count(ptr);

	public unsafe IntMap() => ptr = QuartzNative.IntMap_Create();

	public unsafe void Set(IntInt element) => QuartzNative.IntMap_Set(ptr, element);
	public unsafe void Set(uint key, uint val) => QuartzNative.IntMap_Set(ptr, new(key, val));
	public unsafe void Remove(uint key) => QuartzNative.IntMap_Remove(ptr, key);
	public unsafe uint TryGet(uint key) => QuartzNative.IntMap_TryGetValue(ptr, key);
	public bool Contains(uint key) => TryGet(key) != uint.MaxValue;

	public unsafe void Clear() => QuartzNative.IntMap_Clear(ptr);

	public uint this[uint key] {
		get => TryGet(key);
		set => Set(key, value);
	}

	private unsafe void ReleaseUnmanagedResources() {
		if (ptr != null) QuartzNative.IntMap_Destroy(ptr);
		ptr = null;
	}
	
	public void Dispose() {
		ReleaseUnmanagedResources();
		GC.SuppressFinalize(this);
	}
	~IntMap() => ReleaseUnmanagedResources();
}