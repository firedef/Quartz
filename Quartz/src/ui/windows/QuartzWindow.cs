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
	public ShaderProgram shaderProgram;
	public Mesh mesh;
	public TinyPointMesh mesh2;

	private const string _vertexShaderSrc = @"
#version 330
in vec3 position;
in vec3 normal;
in vec4 color;
in vec2 uv0;
in vec2 uv1;

uniform mat4 mvp;

void main(void) {
	gl_Position = vec4(position, 1.0);
}";

	private const string _fragmentShaderSrc = @"
#version 330
out vec4 outputColor;
void main(void) { 
	outputColor = vec4(0.8, 0.8, 0.9, 1.0); 
}";
	
	private const string _geometryShaderSrc = @"
#version 330
layout (points) in;
layout (triangle_strip, max_vertices = 4) out;

void main() {
	const float s = .1f;

	vec4 pos = gl_in[0].gl_Position;

	gl_Position = pos + vec4(0., 1., 0., 0.) * s;
	EmitVertex();

	gl_Position = pos + vec4(1., 1., 0., 0.) * s;
	EmitVertex();

	gl_Position = pos + vec4(0., 0., 0., 0.) * s;
	EmitVertex();

	gl_Position = pos + vec4(1., 0., 0., 0.) * s;
	EmitVertex();
}
";

	private readonly Vertex[] _points = {
		new(new(-0.5f, -0.5f), color.softBlue),
		new(new(-0.5f,  0.5f), color.softRed),
		new(new( 0.5f,  0.5f), color.softPurple),
		new(new( 0.5f, -0.5f), color.softCyan),
	};

	private readonly ushort[] _indices = { 0, 1, 2, 0, 2, 3 };
	
	private readonly Vertex[] _points2 = {
		new(new(-0.5f, -0.5f), color.softBlue),
		new(new(-0.5f,  0.5f), color.softRed),
		new(new( 0.5f,  0.5f), color.softPurple),
		new(new( 0.5f, -0.5f), color.softCyan),
	};
	
	public QuartzWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) {
		GLFW.WindowHint(WindowHintInt.Samples, 4);
	}

	protected override unsafe void OnLoad() {
		shaderProgram = new(_vertexShaderSrc, _fragmentShaderSrc, _geometryShaderSrc);

		//mesh = new(_points, _indices);
		//mesh.topology = PrimitiveType.Triangles;

		mesh2 = new(_points2);
		mesh2.vertices.Add(new(new(0, .7f), color.softBlue));
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
		shaderProgram.Dispose();
		
		base.OnUnload();
	}

	protected override void OnResize(ResizeEventArgs e) {
		GL.Viewport(0,0, e.Width, e.Height);
		base.OnResize(e);
	}

	protected override void OnRenderFrame(FrameEventArgs args) {
		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		shaderProgram.Bind();

		//mesh.updateRequired = true;
		//Vertex vert = mesh.vertices[0];
		//vert.position.x = MathF.Sin((float)DateTime.Now.TimeOfDay.TotalMilliseconds * .01f) * .5f;
		//mesh.vertices[0] = vert;

		GL.Enable(EnableCap.Multisample);
		//if (mesh.PrepareForRender())
		//	GL.DrawElements(mesh.topology, mesh.indices.count, DrawElementsType.UnsignedShort, 0);


		if (mesh2.PrepareForRender()) {
			GL.DrawArrays(mesh2.getTopology, 0, mesh2.vertices.count);
		}

		//ImGui.GetIO().
		SwapBuffers();
		
		base.OnRenderFrame(args);
	}
}