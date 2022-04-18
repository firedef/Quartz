using Quartz.CoreCs.other.events;

namespace Quartz.graphics.render.renderers; 

public class Renderer : RendererBase {
	protected override bool Render_Abstract() {
		EventManager.OnRender();
		return true;
	}
	protected override void Cleanup() {
		
	}
}