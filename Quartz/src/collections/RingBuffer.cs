using System.Collections;
using MathStuff;

namespace Quartz.collections; 

public class RingBuffer<T> : ICollection<T> {
#region fields

	public T[] buffer;
	public int pos;
	public int count;
	public int capacity => buffer.Length;
	public bool isOverflowed => capacity <= count;

#endregion fields

#region buffer

	public RingBuffer(int size) => this.buffer = new T[size];

	public void Add(T v) {
		buffer[pos] = v;
		count++;
		pos = count % capacity;
	}
	public void Clear() {
		count = 0;
		pos = 0;
	}

	public T Pop() {
		if (count == 0) throw new IndexOutOfRangeException();
		count--;
		pos = count % capacity;
		return buffer[pos];
	}

	public int GetIndex(int offset) {
		int v = (offset + pos) % capacity;
		if (v < 0) v += capacity;
		return v;
	}

	public void OffsetBuffer(int offset) => pos = (offset + pos) % capacity;
	public void Next() => OffsetBuffer(+1);

	/// <summary>
	/// use 0 offset for current element, positive for previous elements and negative for next elements
	/// </summary>
	public T this[int offset] {
		get => buffer[GetIndex(-offset - 1)];
		set => buffer[GetIndex(-offset - 1)] = value;
	}

#endregion buffer

#region icollection

	bool ICollection<T>.Contains(T item) => throw new NotImplementedException();
	void ICollection<T>.CopyTo(T[] array, int arrayIndex) => throw new NotImplementedException();
	bool ICollection<T>.Remove(T item) => throw new NotImplementedException();
	int ICollection<T>.Count => count;
	bool ICollection<T>.IsReadOnly => false;

#endregion icollection

#region ienumerable

	public IEnumerator<T> GetEnumerator() => new Enumerator(this);
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public class Enumerator : IEnumerator<T> {
		private int pos = -1;
		private RingBuffer<T> owner;

		public Enumerator(RingBuffer<T> owner) {
			this.owner = owner;
		}
		
		public bool MoveNext() => ++pos < math.min(owner.capacity, owner.count);
		public void Reset() => pos = -1;
		public T Current => owner[pos];

		object IEnumerator.Current => owner[pos]!;

		public void Dispose() { }
	}

#endregion ienumerable


}