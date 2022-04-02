using OpenTK.Graphics.OpenGL.Compatibility;
using Quartz.other.events;
using Quartz.ui.windows;

namespace Quartz.graphics.render.targets; 

public class CameraRenderTarget : IRenderTarget {
	public void BeforeRendering() {
		if (QuartzWindow.mainWindow == null) return;
		
		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		GL.Enable(EnableCap.Blend);
		GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
		GL.Enable(EnableCap.Multisample);
	}

	public void AfterRendering() {
		QuartzWindow.mainWindow?.SwapBuffers();
	}
}