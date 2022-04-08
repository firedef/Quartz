using Quartz.other.events;

namespace Quartz.objects.scenes; 

public static class SceneManager {
	public static readonly List<Scene> openedScenes = new();
	public static Scene? current => openedScenes.Count == 0 ? null : openedScenes[0];
	public static Scene? last => openedScenes.Count == 0 ? null : openedScenes[^1];
	
	[Call(EventTypes.start)]
	private static void OnStart() {
		LoadEmptyScene();
	}

	public static Scene LoadEmptyScene(bool open = true) {
		Scene scene = new("empty scene");

		if (open) OpenScene(scene);
		return scene;
	}

	public static void OpenScene(Scene scene) {
		openedScenes.Add(scene);
		Dispatcher.global.Call(EventTypes.sceneEnter);
	}
	
	public static void CloseScene(Scene? scene) {
		if (scene == null) return;
		Dispatcher.global.Call(EventTypes.sceneExit);
		scene.Destroy();
		openedScenes.Remove(scene);
	}
}