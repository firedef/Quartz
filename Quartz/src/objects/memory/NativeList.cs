using System.Collections;
using MathStuff;

namespace Quartz.objects.memory; 

public class NativeList<T> : ICollection<T>, IDisposable where T : unmanaged {
#region fields

	public const int defaultSize = 1 << 20;
	public unsafe T* ptr;
	public int capacity;
	public int count;
	public int freeSpace => capacity - count;

#endregion fields

#region ctors

	public NativeList() : this(defaultSize) {}

	public unsafe NativeList(int capacity) {
		ptr = (T*) MemoryAllocator.Allocate(capacity * sizeof(T));
		this.capacity = capacity;
	}

#endregion ctors

#region memory

	protected unsafe void Resize(int newSize) {
		if (newSize == capacity) return;
		if (capacity == 0) ptr = (T*)MemoryAllocator.Allocate(newSize * sizeof(T));
		else ptr = (T*)MemoryAllocator.Resize(ptr, newSize * sizeof(T));
		capacity = newSize;
	}

	protected void Expand(int minSize) => Resize(capacity + math.max(minSize, defaultSize));
	public void EnsureFreeSpace(int space) { if (freeSpace < space) Expand(space); }
	public void EnsureCapacity(int c) { if (capacity < c) Expand(c - capacity); }

#endregion memory

#region elements

	public unsafe void Add(T v) {
		EnsureFreeSpace(1);
		ptr[count++] = v;
	}

	public void Pop(int c = 1) => count = math.max(count, count - c);

	public unsafe void MoveElements(int src, int dest, int c) {
		EnsureCapacity(dest + c);
		MemoryAllocator.MemCpy(ptr + dest, ptr + src, c);
	}
	public void ShiftElements(int index, int amount) {
		count += amount;
		switch (amount) {
			case 0:   return;
			case < 0: MoveElements(index - amount, index, count - index + amount); break;
			default:  MoveElements(index, index + amount, count - index - amount); break;
		}
	}

	protected void CascadeRemove(int index, int c) => ShiftElements(index, -c);
	protected void CascadeInsert(int index, int c) => ShiftElements(index,  c);

	public void Insert(int index, T v) {
		CascadeInsert(index, 1);
		this[index] = v;
	}

	public void RemoveAt(int index, int c = 1) => CascadeRemove(index, c);

	public unsafe T this[int index] { get => ptr[index]; set => ptr[index] = value; }

	public int IndexOf(T v) { 
		for (int i = 0; i < count; i++) if (this[i].Equals(v)) return i; 
		return -1; 
	}
	
	public int IndexOf(Predicate<T> predicate) { 
		for (int i = 0; i < count; i++) if (predicate(this[i])) return i; 
		return -1; 
	}

	public unsafe void CopyTo(T* dest, int c) => MemoryAllocator.MemCpy(dest, ptr, math.min(c, count));

#endregion elements

#region icollection

	public void Clear() => count = 0;
	public bool Contains(T item) => IndexOf(item) != -1;
	public unsafe void CopyTo(T[] array, int arrayIndex) { fixed(T* dest = array) CopyTo(dest, array.Length); }
	public bool Remove(T item) {
		int ind = IndexOf(item);
		if (ind == -1) return false;
		RemoveAt(ind);
		return true;
	}
	public int Count => count;
	public bool IsReadOnly => false;

	public IEnumerator<T> GetEnumerator() => new Enumerator(this);
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

#endregion icollection

#region enumerator
	
	public class Enumerator : IEnumerator<T> {
		public int pos;
		public NativeList<T> owner;

		public Enumerator(NativeList<T> owner) => this.owner = owner;

		public bool MoveNext() => pos++ < owner.count;
		public void Reset() => pos = 0;
		public T Current => owner[pos];

		object IEnumerator.Current => Current;

		public void Dispose() { }
	}  

#endregion enumerator

#region dispose

	private unsafe void ReleaseUnmanagedResources() {
		MemoryAllocator.Free(ptr);
		capacity = 0;
	}
	public void Dispose() {
		ReleaseUnmanagedResources();
		GC.SuppressFinalize(this);
	}
	~NativeList() => ReleaseUnmanagedResources();

#endregion dispose
}
