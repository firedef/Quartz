using Quartz.collections;

namespace Quartz.objects.ecs.managed; 

public class ManagedListPool<T> {
	public List<T> storage = new();
	public SortedIntList emptyIndices = new();

	private int _Add(T v) {
		storage.Add(v);
		return storage.Count - 1;
	}

	public void Pop(int c = 1) => storage.RemoveRange(storage.Count - c, c);

	public int Add(T v) {
		if (emptyIndices.count == 0) return _Add(v);

		int ind = emptyIndices.Pop();
		storage[ind] = v;
		return ind;
	}

	public void RemoveAt(int index, int c = 1) {
		if (index + c == storage.Count) {
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

	public bool Contains(int i) => i >= 0 && i < storage.Count && !emptyIndices.Contains(i);
}