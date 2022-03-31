namespace Quartz.other; 

public static class Rand {
	public static Random instance = new();

	public static float val => instance.NextSingle();

	public static bool Bool(float chance) => val < chance;
}