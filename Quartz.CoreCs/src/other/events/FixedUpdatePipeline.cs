using System.Runtime.CompilerServices;
using Quartz.CoreCs.collections;
using Quartz.CoreCs.other.time;

namespace Quartz.CoreCs.other.events; 

public static partial class FixedUpdatePipeline {
	private static readonly List<Event> _deferredEvents = new();
	private static readonly Queue<Event> _pendingEvents = new();
	private static readonly Queue<Event> _pendingEventsMainThread = new();
	private static readonly RingBuffer<Events> _upcomingEvents = new(bufferLength);
	private static bool _paused;
	private static bool _waitForEmpty;
	private static readonly object _lock = new();
	private static int tick => Time.currentTick;

	private static Task? currentTask = null;

	static FixedUpdatePipeline() {
		for (int i = 0; i < bufferLength; i++) {
			_upcomingEvents[i] = new();
		}
	}

	private static FixedUpdateTask Enqueue(Event ev) {
		if (ev.tick >= tick + bufferLength) {
			_deferredEvents.Add(ev);
			return ev.SetTask(new(ev.tick, true));
		}

		if (ev.tick <= tick) {
			AddPendingEvent(ev);
			return ev.SetTask(new(tick));
		}
		
		GetUpcomingEvents(ev.tick).Add(ev);
		return ev.SetTask(new(ev.tick));
	}

	private static Events GetUpcomingEvents(int t) => _upcomingEvents[-(t - tick)];

	private static FixedUpdateTask AddPendingEvent(Event ev) {
		if (ev.executeInMainThread) _pendingEventsMainThread.Enqueue(ev);
		else _pendingEvents.Enqueue(ev);
		return ev.SetTask(new(tick));
	}

	private static string GetMethodName(Action a) => $"{a.Method.DeclaringType!.FullName}.{a.Method.Name}()";

	private static void Run(Event ev, List<Task> tasks, List<Action> mainThreadActions) {
		if (ev.executeInMainThread) {
			mainThreadActions.Add(() => {
				ev.action();
				ev.task?.SetComplete();
			});
			return;
		}
		
		if (ev.waitForComplete) tasks.Add(Task.Run(ev.action).ContinueWith(_ => ev.task?.SetComplete()));
		else Task.Run(ev.action).ContinueWith(_ => ev.task?.SetComplete());
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

		currentTask = Task.WhenAll(tasks.ToArray());
		currentTask.Wait();
		currentTask = null;
	}

	internal static void OnFixedUpdate() {
		Step(!_paused && !_waitForEmpty);
	}

	public static void Step(bool next = true) {
		Run();
		if (next) _upcomingEvents.Next();
	}
	
	[CallRepeating(EventTypes.rareUpdate)]
	private static void OnRareUpdate() {
		_deferredEvents.RemoveAll(ev => {
			if (ev.tick >= tick + bufferLength) return false;
			GetUpcomingEvents(ev.tick).Add(ev);
			return true;
		});
	}

	public class Events {
		public List<Event> events = new();
		public float weight = 0;

		public void Add(Event ev) {
			weight += ev.weight;
			events.Add(ev);
		}
	}

	public struct Event {
		public Action action;
		public string name;
		public int tick;
		public float weight;
		public bool executeInMainThread;
		public bool waitForComplete;
		public FixedUpdateTask? task = null;

		public Event(Action action, string name, int tick, float weight, bool executeInMainThread, bool waitForComplete) {
			this.action = action;
			this.name = name;
			this.tick = tick;
			this.weight = weight;
			this.executeInMainThread = executeInMainThread;
			this.waitForComplete = waitForComplete;
		}

		internal FixedUpdateTask SetTask(FixedUpdateTask t) {
			task = t;
			return t;
		}
	}
}