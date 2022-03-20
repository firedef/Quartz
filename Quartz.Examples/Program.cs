using OpenTK.Windowing.Desktop;
using Quartz.debug.log;
using Quartz.ui.windows;

namespace Quartz.Examples; 

public class Program {
	public static void Main(string[] args) {
		Log.level = LogLevel.minimal;
		QuartzWindow win = new(GameWindowSettings.Default, NativeWindowSettings.Default);
		win.Run();
	}
}