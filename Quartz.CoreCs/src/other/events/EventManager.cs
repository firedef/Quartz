using System.Diagnostics;
using Quartz.CoreCs.other.time;

namespace Quartz.CoreCs.other.events; 

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

	public static bool isPaused = false;

	private static double _lastFrameUpdateTime;
	private static double _lastFixedUpdateTime;
	private static double _lastRareUpdateTime;
	private static bool _isFirstUpdate = true;
	private static float _lastFixedDeltatime;

	public static void Update() {
		double currentTime = Time.secondsRealTime;
		
		float fixedDeltaTime = (float)(currentTime - _lastFixedUpdateTime);
		if (fixedDeltaTime >= fixedUpdateRate) {
			Stopwatch sw = Stopwatch.StartNew();
			_lastFixedUpdateTime = currentTime;
			_lastFixedDeltatime = fixedDeltaTime;
			if (!_isFirstUpdate && !isPaused) OnFixedUpdate(fixedDeltaTime);
			
			Time.fixedTime = (float) (sw.ElapsedTicks / (double)Stopwatch.Frequency);
		}
		
		float rareDeltaTime = (float)(currentTime - _lastRareUpdateTime);
		if (rareDeltaTime >= rareUpdateRate) {
			_lastRareUpdateTime = currentTime;
			if (!_isFirstUpdate) OnRareUpdate(rareDeltaTime);
		}

		_isFirstUpdate = false;
	}
	
	public static void OnRender() {
		Stopwatch sw = Stopwatch.StartNew();
		if (!_isFirstUpdate) {
			double currentTime = Time.secondsRealTime;
			float frameDeltaTime = (float)(currentTime - _lastFrameUpdateTime);
			_lastFrameUpdateTime = currentTime;
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
	
	internal static void OnFixedUpdate(float deltatime) {
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

	public static void Step(float deltatime) => OnFixedUpdate(deltatime);
	public static void Step() => OnFixedUpdate(_lastFixedDeltatime);
}