namespace Quartz.CoreCs.collections; 

public class SortedIntList : NativeList<int> {
	public int minValue => count == 0 ? 0 : first;
	public int maxValue => count == 0 ? 0 : last;

	public SortedIntList() : base(16) { }
	
	public override int Add(int v) {
		int place = BinarySearch(v);
		Insert(place, v);
		return place;
	}

	public unsafe void AddRange(int start, int c) {
		int place = BinarySearch(start);
		int* range = InsertSpace(place, c);
		for (int i = 0; i < c; i++) range[i] = start + i;
	}

	public override int IndexOf(int v) => BinarySearchExact(v);

	public unsafe int BinarySearch(int v) {
		int low = 0;
		int high = count - 1;
		int mid = 0;
		while (low <= high) {
			mid = low + ((high - low) >> 1);
			if (ptr[mid] == v) return mid;
			if (ptr[mid] < v) {
				low = mid + 1;
				continue;
			}
			high = mid - 1;
		}

		return low;
	}
	
	public unsafe int BinarySearchExact(int v) {
		if (count == 0) return -1;
		int ind = BinarySearch(v);
		if (ind >= 0 && ind < count && ptr[ind] == v) return ind;
		return -1;
	}
}