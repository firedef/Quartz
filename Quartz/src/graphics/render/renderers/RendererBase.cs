using Quartz.graphics.render.targets;

namespace Quartz.graphics.render.renderers; 

public abstract class RendererBase {
	public bool isActive = true;
	public List<IRenderTarget> targets = new();

	public void Render() {
		if (!isActive || targets.Count == 0) return;
		
		foreach (IRenderTarget target in targets) target.BeforeRendering();
		if (!Render_Abstract()) return;
		foreach (IRenderTarget target in targets) target.AfterRendering();
		
		Cleanup();
	}

	protected abstract bool Render_Abstract();
	protected abstract void Cleanup();
}