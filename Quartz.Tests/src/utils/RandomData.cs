using System;
using System.Collections.Generic;
using Quartz.other;

namespace Quartz.Tests.utils; 

public static class RandomData {
	public static int[] RndInts(int count, int step, bool shuffle = true) {
		int[] arr = new int[count];
		for (int i = 1; i < count; i++)
			arr[i] = arr[i - 1] + Rand.Int(1, step);

		if (shuffle) arr.Shuffle();
		return arr;
	}
	
	public static uint[] RndUInts(int count, int step, bool shuffle = true) {
		uint[] arr = new uint[count];
		for (int i = 1; i < count; i++)
			arr[i] = arr[i - 1] + (uint) Rand.Int(1, step);

		if (shuffle) arr.Shuffle();
		return arr;
	}

	public static void Shuffle<T>(this T[] arr) {
		int n = arr.Length;
		while (n > 1) {
			int k = Rand.Int(0, n--);
			(arr[k], arr[n]) = (arr[n], arr[k]);
		}
	}
}