namespace Quartz.CoreCs.other; 

[Flags]
public enum State : byte {
	on = 0b01,
	off = 0b10,
	any = on | off,
	none = 0,
}

public static class StateExtensions {
	public static bool Check(this State state, bool currentState) => currentState ? (state & State.on) != 0 : (state & State.off) != 0;
}