using Quartz.graphics.render.renderers;

namespace Quartz.graphics.render; 

public static class RenderManager {
	public static RendererBase mainRenderer => renderers[0];
	public static List<RendererBase> renderers = new();

	public static void Render() {
		foreach (RendererBase renderer in renderers) {
			renderer.Render();
		}
	}
}