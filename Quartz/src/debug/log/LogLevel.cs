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
	unspecified = 0b001,
	rendererCore = 0b010,
	renderer = 0b100,
	all = int.MaxValue,
}