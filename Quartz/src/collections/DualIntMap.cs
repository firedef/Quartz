using Quartz.core.collections;

namespace Quartz.collections; 

public class DualIntMap {
	protected IntMap kv = new();
	protected IntMap vk = new();

	public int count => kv.count;

	public void Set(IntInt v) => Set(v.key, v.val);

	public void Set(uint k, uint v) {
		kv.Set(k, v);
		vk.Set(v, k);
	}

	public void Remove(uint k, uint v) {
		if (k == uint.MaxValue || v == uint.MaxValue) return;
		kv.Remove(k);
		vk.Remove(v);
	}

	public void RemoveByKey(uint k) => Remove(k, GetVal(k));
	public void RemoveByVal(uint v) => Remove(GetKey(v), v);

	public uint GetVal(uint k) => kv[k];
	public uint GetKey(uint v) => vk[v];

	public bool ContainsKey(uint k) => GetVal(k) != uint.MaxValue;
	public bool ContainsVal(uint v) => GetKey(v) != uint.MaxValue;

	public uint this[uint k] {
		get => GetVal(k);
		set => Set(k, value);
	}
}