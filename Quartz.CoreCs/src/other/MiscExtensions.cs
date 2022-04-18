namespace Quartz.CoreCs.other; 

public static class MiscExtensions {
	public static void UnorderedRemoveAt<T>(this List<T> l, int index) {
		int c = l.Count;
		if (index != c - 1) l[index] = l[c - 1];
		l.RemoveAt(c - 1); 
	}
}