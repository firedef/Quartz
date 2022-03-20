using ImGuiNET;
using MathStuff;
using OpenTK.Graphics.OpenGL.Compatibility;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Quartz.collections.shaders;
using Quartz.geometry.mesh;
using Quartz.utils;

namespace Quartz.ui.windows; 

public class QuartzWindow : GameWindow {
	public Shader shader;
	public Mesh mesh;

	private const string _vertexShaderSrc = @"
#version 330
in vec3 position;
in vec3 normal;
in vec4 color;
in vec2 uv0;
in vec2 uv1;

void main(void) {
	gl_Position = vec4(position, 1.0);
}";

	private const string _fragmentShaderSrc = @"
#version 330
out vec4 outputColor;
void main(void) { 
	outputColor = vec4(0.8, 0.8, 0.9, 1.0); 
}";

	private readonly Vertex[] _points = {
		new(new(-0.5f, -0.5f), color.softBlue),
		new(new(-0.5f,  0.5f), color.softRed),
		new(new( 0.5f,  0.5f), color.softPurple),
		new(new( 0.5f, -0.5f), color.softCyan),
	};

	private readonly ushort[] _indices = { 0, 1, 2, 0, 2, 3 };
	
	public QuartzWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) {
		GLFW.WindowHint(WindowHintInt.Samples, 4);
	}

	protected override unsafe void OnLoad() {
		shader = new(_vertexShaderSrc, _fragmentShaderSrc);

		mesh = new(_points, _indices);
		
		GL.Enable(EnableCap.DebugOutput);
		GL.FrontFace(FrontFaceDirection.Cw);
		//GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

		color clearCol = color.steelBlue;
		GL.ClearColor(clearCol.rF, clearCol.gF, clearCol.bF, clearCol.aF);

		OpenGl.CheckErrors();
		base.OnLoad();
	}

	protected override void OnUnload() {
		mesh.Dispose();
		shader.Dispose();
		
		base.OnUnload();
	}

	protected override void OnResize(ResizeEventArgs e) {
		GL.Viewport(0,0, e.Width, e.Height);
		base.OnResize(e);
	}

	protected override void OnRenderFrame(FrameEventArgs args) {
		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		shader.Bind();

		mesh.updateRequired = true;
		Vertex vert = mesh.vertices[0];
		vert.position.x = MathF.Sin((float)DateTime.Now.TimeOfDay.TotalMilliseconds * .01f) * .5f;
		mesh.vertices[0] = vert;

		GL.Enable(EnableCap.Multisample);
		if (mesh.PrepareForRender())
			GL.DrawElements(PrimitiveType.Triangles, mesh.indices.count, DrawElementsType.UnsignedShort, 0);

		//ImGui.GetIO().
		SwapBuffers();
		
		base.OnRenderFrame(args);
	}
}