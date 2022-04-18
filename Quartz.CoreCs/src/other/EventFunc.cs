namespace Quartz.CoreCs.other; 

public class EventFunc<T0, TReturn> {
	private readonly List<Func<T0, TReturn, TReturn>> _delegates = new();

	public void Subscribe(Func<T0, TReturn, TReturn> d) => _delegates.Add(d);
	public void Unsubscribe(Func<T0, TReturn, TReturn> d) => _delegates.Remove(d);

	public static EventFunc<T0, TReturn> operator +(EventFunc<T0, TReturn> a, Func<T0, TReturn, TReturn?> b) {
		a.Subscribe(b);
		return a;
	}
	
	public static EventFunc<T0, TReturn> operator -(EventFunc<T0, TReturn> a, Func<T0, TReturn, TReturn?> b) {
		a.Unsubscribe(b);
		return a;
	}

	public TReturn Invoke(TReturn @default, T0 a0) {
		TReturn val = @default;
		int c = _delegates.Count;
		for (int i = 0; i < c; i++) val = _delegates[i](a0, val);
		return val;
	}
}