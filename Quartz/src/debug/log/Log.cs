using Spectre.Console;

namespace Quartz.debug.log; 

public static class Log {
	public static LogLevel level = LogLevel.message;
	public static LogForm filter = LogForm.all;

	public static void PrintRaw(string msg) => Console.WriteLine(msg);
	public static void Print(string msg) => AnsiConsole.MarkupLine(msg);
	public static void Print(string msg, LogForm form, LogLevel lvl) {
		if (level <= lvl && (filter & form) != 0) Print(msg);
	}
	
	public static void Print(string msg, string style, LogForm form, LogLevel lvl) {
		if (level <= lvl && (filter & form) != 0) Print($"[{style}]{msg}[/]");
	}

	public static void Minimal(string msg, LogForm form = LogForm.unspecified) => Print(msg, "gray i s", form, LogLevel.minimal);
	public static void Note(string msg, LogForm form = LogForm.unspecified) => Print(msg, "gray", form, LogLevel.note);
	public static void Message(string msg, LogForm form = LogForm.unspecified) => Print(msg, form, LogLevel.message);
	public static void Important(string msg, LogForm form = LogForm.unspecified) => Print(msg, "blue", form, LogLevel.important);
	public static void Warning(string msg, LogForm form = LogForm.unspecified) => Print(msg, "yellow", form, LogLevel.warning);
	public static void Error(string msg, LogForm form = LogForm.unspecified) => Print(msg, "red", form, LogLevel.error);
	public static void Fatal(string msg, LogForm form = LogForm.unspecified) => Print(msg, "red bold", form, LogLevel.fatal);
}

