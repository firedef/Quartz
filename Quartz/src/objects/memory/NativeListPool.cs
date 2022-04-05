using Quartz.collections;

namespace Quartz.objects.memory; 

public class NativeListPool<T> : NativeList<T> where T : unmanaged {
	public SortedIntList emptyIndices = new();
	public int elementCount => count - emptyIndices.count;

	public NativeListPool(int capacity) : base(capacity) {
	}

	public override unsafe int Add(T v) {
		if (emptyIndices.count == 0) return base.Add(v);

		int ind = emptyIndices.Pop();
		ptr[ind] = v;
		return ind;
	}

	public void AddMultiple(int c) {
		while (emptyIndices.count > 0 && c > 0) emptyIndices.Pop();
		for (int i = 0; i < c; i++) base.Add(default);
	}
	
	public void AddMultiple(int c, Action<int> onAdd) {
		while (emptyIndices.count > 0 && c > 0) onAdd(emptyIndices.Pop());
		EnsureFreeSpace(c);
		for (int i = 0; i < c; i++) onAdd(base.Add(default));
	}

	public override void RemoveAt(int index, int c = 1) {
		if (index + c == count) {
			Pop(c);
			int i = index - 1;
			while (emptyIndices.count > 0 && emptyIndices.maxValue == i) {
				Pop();
				emptyIndices.Pop();
				i--;
			}
			return;
		}
		emptyIndices.AddRange(index, c);
	}
}