using System.Collections.Concurrent;
using Quartz.debug.log;

namespace Quartz.other.events; 

public static class EventsPipeline {
	public const int threadCount = 8;

	private static Thread[] _threads = new Thread[threadCount];
	private static readonly ConcurrentQueue<Event> _pendingEvents = new();
	private static bool _isClosing;

	static EventsPipeline() {
		for (int i = 0; i < threadCount; i++) {
			int threadId = i;
			_threads[i] = new(() => ThreadJob(threadId));
			_threads[i].IsBackground = true;
			_threads[i].Start();
		}
	}

	private static void ThreadJob(int threadId) {
		Log.Message($"thread #{threadId} is started");
		
		while (!_isClosing) {
			if (_pendingEvents.IsEmpty) {
				Thread.Sleep(10);
				continue;
			}

			if (!_pendingEvents.TryDequeue(out Event a)) {
				Thread.Sleep(1);
				continue;
			}
			a.action();
			if (a.repeatDelay < 0 || (a.repeatWhile != null && a.repeatWhile())) continue;
			if (a.repeatDelay == 0) Execute(a);
			else Execute(a, a.repeatDelay);
		}
		
		Log.Message($"thread #{threadId} is closed");
	}

	[Call(EventTypes.quit)]
	private static void CloseThreads() {
		_isClosing = true;
	}
	
	private static void Execute(Event action) => _pendingEvents.Enqueue(action);
	private static void Execute(Event action, int delay) => Task.Run(async () => { await Task.Delay(delay); Execute(action); });
	public static void Execute(Action action) => _pendingEvents.Enqueue(new(-1, action, null));
	public static void Execute(Action action, int repeatDelay) => _pendingEvents.Enqueue(new(repeatDelay, action, null));
	public static void Execute(Action action, int repeatDelay, Func<bool> repeatWhile) => _pendingEvents.Enqueue(new(repeatDelay, action, repeatWhile));
	
	private record struct Event(int repeatDelay, Action action, Func<bool>? repeatWhile) {
		public int repeatDelay = repeatDelay;
		public Action action = action;
		public Func<bool>? repeatWhile = repeatWhile;
	}
}
