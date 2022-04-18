using Quartz.CoreCs.other.events;
using Quartz.objects.scenes;

namespace Quartz.debug.log; 

public static class EventLogger {
	[CallRepeating(EventTypes.start)] private static void OnStart() => Log.Message($"application start", LogForm.events);
	[CallRepeating(EventTypes.quit)] private static void OnQuit() => Log.Message($"application quit", LogForm.events);
	[CallRepeating(EventTypes.lowMemory)] private static void OnLowMemory() => Log.Message($"low memory", LogForm.events);
	[CallRepeating(EventTypes.sceneEnter)] private static void OnSceneEnter() => Log.Message($"scene open: {SceneManager.last}", LogForm.events);
	[CallRepeating(EventTypes.sceneExit)] private static void OnSceneExit() => Log.Message($"scene close: {SceneManager.last}", LogForm.events);
	[CallRepeating(EventTypes.gamePauseToggle)] private static void OnGamePause() => Log.Message($"game pause", LogForm.events);
}