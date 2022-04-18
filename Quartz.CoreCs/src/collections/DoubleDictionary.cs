using Quartz.CoreCs.native.collections;

namespace Quartz.CoreCs.collections; 

public class DoubleDictionary {
	public readonly Dictionary<uint, uint> kv = new();
	public readonly Dictionary<uint, uint> vk = new();

	public int count => kv.Count;
	public int keyCount => kv.Count;
	public int valCount => vk.Count;

	public void Set(IntInt v) => Set(v.key, v.val);

	public void Set(uint k, uint v) {
		uint oldK = GetKey(v);
		uint oldV = GetVal(k);
		if (oldK != uint.MaxValue) kv.Remove(oldK);
		if (oldV != uint.MaxValue) vk.Remove(oldV);
		if (!kv.TryAdd(k, v)) kv[k] = v;
		if (!vk.TryAdd(v, k)) kv[v] = k;
	}

	public void Remove(uint k, uint v) {
		if (k == uint.MaxValue || v == uint.MaxValue) return;
		kv.Remove(k);
		vk.Remove(v);
	}

	public void RemoveByKey(uint k) => Remove(k, GetVal(k));
	public void RemoveByVal(uint v) => Remove(GetKey(v), v);

	public uint GetVal(uint k) => kv.TryGetValue(k, out uint v) ? v : uint.MaxValue;
	public uint GetKey(uint v) => vk.TryGetValue(v, out uint k) ? k : uint.MaxValue;

	public bool ContainsKey(uint k) => GetVal(k) != uint.MaxValue;
	public bool ContainsVal(uint v) => GetKey(v) != uint.MaxValue;

	public void Clear() {
		kv.Clear();
		vk.Clear();
	}

	public uint this[uint k] {
		get => GetVal(k);
		set => Set(k, value);
	}
}

public class DualIntMap {
	protected readonly IntMap kv = new();
	protected readonly IntMap vk = new();

	public int count => kv.count;
	public int keyCount => kv.count;
	public int valCount => vk.count;

	public void Set(IntInt v) => Set(v.key, v.val);

	public void Set(uint k, uint v) {
		uint oldK = GetKey(v);
		uint oldV = GetVal(k);
		if (oldK != uint.MaxValue) kv.Remove(oldK);
		if (oldV != uint.MaxValue) vk.Remove(oldV);
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

	public void Clear() {
		kv.Clear();
		vk.Clear();
	}

	public uint this[uint k] {
		get => GetVal(k);
		set => Set(k, value);
	}
}