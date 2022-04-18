using System.Diagnostics;
using MathStuff;
using Quartz.CoreCs.collections;

namespace Quartz.CoreCs.other.events;

public static partial class FixedUpdatePipeline {
	public const int bufferLength = 1024;
	
	public static RingBuffer<Events> GetRingBuffer() => _upcomingEvents;
	public static int GetNextTickUpdateCount() => _upcomingEvents[-1].events.Count;
	
	public static FixedUpdateTask Enqueue(Action action, string? name, int invokeTick, bool executeByMainThread = false, float weight = 25, bool waitForComplete = true) => Enqueue(new(action, name ?? GetMethodName(action), invokeTick, weight, executeByMainThread, waitForComplete));
	public static FixedUpdateTask Enqueue(Action action, string? name, int invokeTick, int maxTickDelay, bool executeByMainThread = false, float weight = 25, bool waitForComplete = true) {
		if (invokeTick >= tick + bufferLength || invokeTick <= tick)
			return Enqueue(action, name, invokeTick, executeByMainThread, weight, waitForComplete);

		maxTickDelay = math.min(maxTickDelay, bufferLength - invokeTick - tick);
		int bestTick = invokeTick;
		float bestWeight = GetUpcomingEvents(invokeTick).weight;

		float wMul = 2f / maxTickDelay;
		for (int i = 1; i < maxTickDelay; i++) {
			float w = GetUpcomingEvents(invokeTick + i).weight * (1 + wMul * i);
			if (w >= bestWeight) continue;
			bestWeight = w;
			bestTick = invokeTick + i;
		}

		return Enqueue(action, name, bestTick, executeByMainThread, weight);
	}
	public static FixedUpdateTask Enqueue(Action action, string? name, bool executeByMainThread = false, bool waitForComplete = true) => AddPendingEvent(new(action, name ?? GetMethodName(action), tick, 25, executeByMainThread, waitForComplete));
	public static FixedUpdateTask EnqueueWithDelay(Action action, string? name, int tickDelay, int maxTickDelay = 5, bool executeByMainThread = false, float weight = 25, bool waitForComplete = false) => Enqueue(action, name, tick + tickDelay, maxTickDelay, executeByMainThread, weight, waitForComplete);
	public static void Pause() => _paused = true;
	public static void Resume() => _paused = false;

	public static void WaitForEmptyAndExecute(Action a) {
		lock (_lock) {
			_waitForEmpty = true;
			currentTask?.Wait();
			a();
			_waitForEmpty = false;
		}
	}
}