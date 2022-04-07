using Quartz.other.events;

namespace Quartz.other.time; 

public static class Time {
	public static double elapsedSecondsRealTime;
	public static double elapsedSecondsGame;
	public static double secondsRealTime => DateTime.Now.TimeOfDay.TotalSeconds;

	public static int currentTick;

	public static float frameDeltaTime;
	public static float fixedDeltaTime;
	
	public static float gameSpeed = 1f;

	public static void OnFrameUpdate(float dt) {
		frameDeltaTime = dt;
		elapsedSecondsRealTime += dt;
		elapsedSecondsGame += dt * gameSpeed;
	}
	
	public static void OnFixedUpdate(float dt) {
		fixedDeltaTime = dt;
		currentTick++;
	}
}