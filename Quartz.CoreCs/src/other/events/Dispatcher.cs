using MathStuff;
using Quartz.CoreCs.other.time;

namespace Quartz.CoreCs.other.events; 

public class Dispatcher {
	public static readonly Dispatcher global = new();

	protected readonly object _lock = new();
	protected List<RepeatingEvent>[] repeatingEvents;
	protected Stack<Event>[] pendingEvents;

	public Dispatcher() {
		int eventTypeCount = 32;
		pendingEvents = new Stack<Event>[eventTypeCount];
		repeatingEvents = new List<RepeatingEvent>[eventTypeCount];
		for (int i = 0; i < eventTypeCount; i++) {
			pendingEvents[i] = new();
			repeatingEvents[i] = new();
		}
	}

	public void Call(EventTypes type) {
		lock (_lock) {
			if (type == EventTypes.none) return;
			int id = (int)math.log2((double) type);

			Stack<Event> stack = pendingEvents[id];

			while (stack.Count > 0) {
				if (!stack.TryPop(out Event? e)) continue;
				if (e.addedTime + e.lifetime < Time.elapsedSecondsRealTime) continue;
				e.action();
			}

			foreach (RepeatingEvent e in repeatingEvents[id].Where(e => e.continueInvoke() && e.addedTime + e.lifetime >= Time.elapsedSecondsRealTime)) e.action();
		}
	}

	public void Cleanup() {
		lock (_lock)
			for (int i = 0; i < 32; i++) repeatingEvents[i].RemoveAll(e => !e.continueInvoke() || e.addedTime + e.lifetime < Time.elapsedSecondsRealTime);
	}

	public void Push(Event e, EventTypes type) {
		lock (_lock) {
			if (type == EventTypes.none) return;
			int id = (int)math.log2((double) type);
			pendingEvents[id].Push(e);
		}
	}

	public void Push(Action a, EventTypes type, float lifetime = float.MaxValue) => Push(new(a, lifetime), type);

	public void PushRepeating(RepeatingEvent e, EventTypes type) {
		lock (_lock) {
			if (type == EventTypes.none) return;
			int id = (int)math.log2((double) type);
			repeatingEvents[id].Add(e);
		}
	}
	
	public void PushRepeating(Action a, EventTypes type, float lifetime = float.MaxValue) => PushRepeating(new(a, () => true, lifetime), type);
	public void PushRepeating(Action a, Func<bool> continueInvoke, EventTypes type, float lifetime = float.MaxValue) => PushRepeating(new(a, continueInvoke, lifetime), type);
	
	public void PushMultiple(Event e, EventTypes types) {
		lock (_lock) {
			if (types == EventTypes.none) return;
			uint ids = (uint)types;
			for (int i = 0; i < 32; i++) {
				if ((ids & (1 << i)) != 1 << i) continue;
				pendingEvents[i].Push(e);
			}
		}
	}
	
	public void PushMultiple(Action a, EventTypes type, float lifetime = float.MaxValue) => PushMultiple(new(a, lifetime), type);
	
	public void PushMultipleRepeating(RepeatingEvent e, EventTypes types) {
		lock (_lock) {
			if (types == EventTypes.none) return;
			uint ids = (uint)types;
			for (int i = 0; i < 32; i++) {
				if ((ids & (1 << i)) != 1 << i) continue;
				repeatingEvents[i].Add(e);
			}
		}
	}
	
	public void PushMultipleRepeating(Action a, EventTypes type, float lifetime = float.MaxValue) => PushMultipleRepeating(new(a, () => true, lifetime), type);
	public void PushMultipleRepeating(Action a, Func<bool> continueInvoke, EventTypes type, float lifetime = float.MaxValue) => PushMultipleRepeating(new(a, continueInvoke, lifetime), type);
}