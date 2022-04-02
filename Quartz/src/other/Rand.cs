using MathStuff;

namespace Quartz.other; 

public static class Rand {
	public static Random instance = new();

	public static float val => instance.NextSingle();

	public static float ValueWithDistribution(float strength = 2f) {
		float v = (val - .5f) * 2;
		return MathF.CopySign(MathF.Pow(MathF.Abs(v), strength), v) * .5f + .5f;
	}

	public static float Range(float min, float max) => val * (max - min) + min;

	public static bool Bool(float chance) => val < chance;
}