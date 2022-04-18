using MathStuff.vectors;

namespace Quartz.CoreCs.other; 

public static class Rand {
	public static Random instance = new();

	public static float val => instance.NextSingle();

	public static float ValueWithDistribution(float strength = 2f) {
		float v = (val - .5f) * 2;
		return MathF.CopySign(MathF.Pow(MathF.Abs(v), strength), v) * .5f + .5f;
	}

	public static float Range(float min, float max) => val * (max - min) + min;
	public static int Int() => instance.Next();
	public static int Int(int min, int max) => instance.Next(min, max);

	public static float2 Float2(float2 min, float2 max) => new(Range(min.x, max.x), Range(min.y, max.y));

	public static bool Bool(float chance) => val < chance;
}