using System.Diagnostics;
using Quartz.other.time;

namespace Quartz.other.events; 

public static partial class EventManager {
	public static Action<float> onFrameUpdate = _ => { };
	public static Action<float> onFixedUpdate = _ => { };
	public static Action<float> onRareUpdate = _ => { };
	public static Func<Task> beforeClose = () => Task.CompletedTask;

	public static int fixedUpdatesPerSec { get => (int)(1f / fixedUpdateRate); set => fixedUpdateRate = 1f / value; }
	public static float fixedUpdateRate = 1f / 20;
	
	public static int rareUpdatesPerMin { get => (int)(60f / rareUpdateRate); set => rareUpdateRate = 60f / value; }
	public static float rareUpdateRate = 1f;
	
	public static int fps { get => (int)(1f / renderRate); set => renderRate = 1f / value; }
	public static float renderRate = 1f / 60;

	private static double lastFrameUpdateTime;
	private static double lastFixedUpdateTime;
	private static double lastRareUpdateTime;
	private static bool isFirstUpdate = true;

	public static void Update() {
		double currentTime = Time.secondsRealTime;
		
		float fixedDeltaTime = (float)(currentTime - lastFixedUpdateTime);
		if (fixedDeltaTime >= fixedUpdateRate) {
			Stopwatch sw = Stopwatch.StartNew();
			lastFixedUpdateTime = currentTime;
			if (!isFirstUpdate) OnFixedUpdate(fixedDeltaTime);
			
			Time.fixedTime = (float) (sw.ElapsedTicks / (double)Stopwatch.Frequency);
		}
		
		float rareDeltaTime = (float)(currentTime - lastRareUpdateTime);
		if (rareDeltaTime >= rareUpdateRate) {
			lastRareUpdateTime = currentTime;
			if (!isFirstUpdate) OnRareUpdate(rareDeltaTime);
		}

		isFirstUpdate = false;
	}
	
	public static void OnRender() {
		Stopwatch sw = Stopwatch.StartNew();
		if (!isFirstUpdate) {
			double currentTime = Time.secondsRealTime;
			float frameDeltaTime = (float)(currentTime - lastFrameUpdateTime);
			lastFrameUpdateTime = currentTime;
			Dispatcher.global.Call(EventTypes.render);
			OnFrameUpdate(frameDeltaTime);
		}
		Time.frameTime = (float) (sw.ElapsedTicks / (double)Stopwatch.Frequency);
	}
	
	public static void OnImGui() {
		Dispatcher.global.Call(EventTypes.imgui);
	}

	public static void OnStart() {
		Dispatcher.global.Call(EventTypes.start);
	}
	
	public static void OnQuit() {
		Dispatcher.global.Call(EventTypes.quit);
	}
	
	public static void OnLowMemory() {
		Dispatcher.global.Call(EventTypes.lowMemory);
	}

	private static void OnFrameUpdate(float deltatime) {
		Time.OnFrameUpdate(deltatime);
		onFrameUpdate(deltatime);
		Dispatcher.global.Call(EventTypes.frameUpdate);
	}
	
	public static void OnFixedUpdate(float deltatime) {
		FixedUpdatePipeline.OnFixedUpdate();
		Time.OnFixedUpdate(deltatime);
		onFixedUpdate(deltatime);
		Dispatcher.global.Call(EventTypes.fixedUpdate);
	}
	
	private static void OnRareUpdate(float deltatime) {
		onRareUpdate(deltatime);
		Dispatcher.global.Call(EventTypes.rareUpdate);
		Dispatcher.global.Cleanup();
	}
}