using OpenTK.Graphics.OpenGL.Compatibility;
using Quartz.ui.windows;

namespace Quartz.graphics.render.targets; 

public class CameraRenderTarget : IRenderTarget {
	public void BeforeRendering() {
		if (QuartzWindow.mainWindow == null) return;
		
	}

	public void AfterRendering() {
	}
}