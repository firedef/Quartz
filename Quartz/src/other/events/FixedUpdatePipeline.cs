using MathStuff;
using Quartz.collections;
using Quartz.other.time;

namespace Quartz.other.events; 

public static class FixedUpdatePipeline {
	private static readonly List<Event> _deferredEvents = new();
	private static readonly Queue<Event> _pendingEvents = new();
	private static readonly Queue<Event> _pendingEventsMainThread = new();
	private static readonly RingBuffer<Events> _upcomingEvents = new(1024);
	private static bool _paused;
	private static int tick => Time.currentTick;

	static FixedUpdatePipeline() {
		for (int i = 0; i < 1024; i++) {
			_upcomingEvents[i] = new();
		}
	}

	private static void Enqueue(Event ev) {
		if (ev.tick >= tick + 1024) {
			_deferredEvents.Add(ev);
			return;
		}

		if (ev.tick <= tick) {
			AddPendingEvent(ev);
			return;
		}
		
		GetUpcomingEvents(ev.tick).Add(ev);
	}

	private static Events GetUpcomingEvents(int t) => _upcomingEvents[-(t - tick)];

	private static void AddPendingEvent(Event ev) {
		if (ev.executeInMainThread) _pendingEventsMainThread.Enqueue(ev);
		else _pendingEvents.Enqueue(ev);
	}
	
	public static void Enqueue(Action action, int invokeTick, bool executeByMainThread = false, float weight = 25, bool waitForComplete = true) => Enqueue(new(action, invokeTick, weight, executeByMainThread, waitForComplete));

	public static void Enqueue(Action action, int invokeTick, int maxTickDelay, bool executeByMainThread = false, float weight = 25, bool waitForComplete = true) {
		if (invokeTick >= tick + 1024 || invokeTick <= tick) {
			Enqueue(action, invokeTick, executeByMainThread, weight, waitForComplete);
			return;
		}

		maxTickDelay = math.min(maxTickDelay, 1024 - invokeTick - tick);
		int bestTick = invokeTick;
		float bestWeight = GetUpcomingEvents(invokeTick).weight;

		float wMul = 2f / maxTickDelay; 
		for (int i = 1; i < maxTickDelay; i++) {
			float w = GetUpcomingEvents(invokeTick+ i).weight * (1 + wMul * i);
			if (w >= bestWeight) continue;
			bestWeight = w;
			bestTick = invokeTick + i;
		}

		Enqueue(action, bestTick, executeByMainThread, weight);
	}

	public static void Enqueue(Action action, bool executeByMainThread = false, bool waitForComplete = true) => AddPendingEvent(new(action, tick, 25, executeByMainThread, waitForComplete));

	public static void EnqueueWithDelay(Action action, int tickDelay, int maxTickDelay = 5, bool executeByMainThread = false, float weight = 25, bool waitForComplete = false) => Enqueue(action, tick + tickDelay, maxTickDelay, executeByMainThread, weight, waitForComplete);
	
	private static void Run(Event ev, List<Task> tasks, List<Action> mainThreadActions) {
		if (ev.executeInMainThread) {
			mainThreadActions.Add(ev.action);
			return;
		}
		if (ev.waitForComplete) tasks.Add(Task.Run(ev.action));
		else Task.Run(ev.action);
	}

	private static void Run() {
		Events events = _upcomingEvents[0];
		_upcomingEvents[0] = new();
		
		int c = events.events.Count;
		List<Task> tasks = new(c);
		List<Action> mainThread = new();

		for (int i = 0; i < c; i++) Run(events.events[i], tasks, mainThread);
		while (_pendingEvents.Count > 0) Run(_pendingEvents.Dequeue(), tasks, mainThread);

		c = _pendingEventsMainThread.Count;
		for (int i = 0; i < c; i++) _pendingEventsMainThread.Dequeue().action();
		foreach (Action a in mainThread) a();

		Task.WaitAll(tasks.ToArray());
	}

	internal static void OnFixedUpdate() {
		Run();
		if (!_paused) _upcomingEvents.Next();
	}
	
	[CallRepeating(EventTypes.rareUpdate)]
	private static void OnRareUpdate() {
		_deferredEvents.RemoveAll(ev => {
			if (ev.tick >= tick + 1024) return false;
			GetUpcomingEvents(ev.tick).Add(ev);
			return true;
		});
	}
	
	public static void Pause() => _paused = true;
	public static void Resume() => _paused = false;

	private class Events {
		public List<Event> events = new();
		public float weight = 0;

		public void Add(Event ev) {
			weight += ev.weight;
			events.Add(ev);
		}
	}

	private struct Event {
		public Action action;
		public int tick;
		public float weight;
		public bool executeInMainThread;
		public bool waitForComplete;

		public Event(Action action, int tick, float weight, bool executeInMainThread, bool waitForComplete) {
			this.action = action;
			this.tick = tick;
			this.weight = weight;
			this.executeInMainThread = executeInMainThread;
			this.waitForComplete = waitForComplete;
		}
	}
}