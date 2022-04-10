using OpenTK.Windowing.Desktop;
using Quartz.debug.log;
using Quartz.ui.windows;

namespace Quartz.Examples; 

public class Program {
	public static void Main(string[] args) {
		Log.level = LogLevel.minimal;
		
		NativeWindowSettings settings = NativeWindowSettings.Default;
		settings.NumberOfSamples = 4;
		
		GameWindowSettings winSettings = GameWindowSettings.Default;
		winSettings.IsMultiThreaded = true;
		//winSettings.UpdateFrequency = 20;
		
		QuartzWindow win = new(winSettings, settings);
		win.Run();
	}
}