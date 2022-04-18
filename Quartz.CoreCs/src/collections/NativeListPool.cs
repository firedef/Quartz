using MathStuff;

namespace Quartz.CoreCs.collections;

public interface INativeListPool {}

public class NativeListPool<T> : NativeList<T>, INativeListPool where T : unmanaged {
	public SortedIntList emptyIndices = new();
	public int elementCount => count - emptyIndices.count;

	public NativeListPool() : base(16) { }
	
	public NativeListPool(int capacity) : base(capacity) { }

	public override unsafe int Add(T v) {
		if (emptyIndices.count == 0) return base.Add(v);

		int ind = emptyIndices.Pop();
		ptr[ind] = v;
		return ind;
	}
	
	public int Claim() {
		if (emptyIndices.count == 0) {
			EnsureFreeSpace(1);
			count++;
			return count - 1;
		}

		int ind = emptyIndices.Pop();
		return ind;
	}
	
	public void ClaimMultiple(int c, Action<int> onClaim) {
		int emptyIndicesCount = math.min(emptyIndices.count, c);
		for (int i = 0; i < emptyIndicesCount; i++) {
			int ind = emptyIndices.Pop();
			onClaim(ind);
		}

		int left = c - emptyIndicesCount;
		if (left == 0) return;
		EnsureFreeSpace(left);

		for (int i = 0; i < left; i++) onClaim(count + i);

		count += left;
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

	public override void Clear() {
		base.Clear();
		emptyIndices.Clear();
	}
}