namespace Quartz.debug.log; 

public enum LogLevel {
	minimal,
	note,
	message,
	important,
	warning,
	error,
	fatal,
}

[Flags]
public enum LogForm {
	unspecified = 1 << 0,
	rendererCore = 1 << 1,
	renderer = 1 << 2,
	events = 1 << 3,
	all = int.MaxValue,
}