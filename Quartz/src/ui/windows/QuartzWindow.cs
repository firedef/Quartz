using MathStuff;
using MathStuff.vectors;
using OpenTK.Graphics.OpenGL.Compatibility;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Quartz.collections.shaders;
using Quartz.core;
using Quartz.objects.mesh;
using Quartz.utils;

namespace Quartz.ui.windows; 

public class QuartzWindow : GameWindow {
	public ShaderProgram shaderProgram;
	public Mesh mesh;
	public TinyPointMesh mesh2;

	private const string _vertexShaderSrc = @"
#version 330
in layout(location=0) vec3 position;
in layout(location=1) vec3 normal;
in layout(location=2) vec4 color;
in layout(location=3) vec2 uv0;
in layout(location=4) vec2 uv1;

uniform mat4 mvp;
out vec4 v_col;

void main(void) {
	gl_Position = vec4(position, 1.0);
	v_col = color.bgra;
}";

	private const string _fragmentShaderSrc = @"
#version 330
in vec4 col;
out vec4 outputColor;
void main(void) { 
	outputColor = col; 
}";
	
	private const string _geometryShaderSrc = @"
#version 330
layout (points) in;
layout (triangle_strip, max_vertices = 4) out;
in vec4 v_col[];
out vec4 col;

void main() {
	const float s = .005f;

	vec4 pos = gl_in[0].gl_Position;
	col = v_col[0];

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
		new(new(-0.5f, -0.5f), color.white),
		new(new(-0.5f,  0.5f), color.softRed),
		new(new( 0.5f,  0.5f), color.softPurple),
		new(new( 0.5f, -0.5f), color.softCyan),
	};

	private readonly ushort[] _indices = { 0, 1, 2, 0, 2, 3 };

	private readonly List<Vertex> _points2 = new();
	
	public QuartzWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) {
		GLFW.WindowHint(WindowHintInt.Samples, 4);
	}

	protected override unsafe void OnLoad() {
		void* ptr = QuartzNative.Allocate(55);
		Console.WriteLine((long) ptr);
		QuartzNative.Free(ptr);
		QuartzNative.CleanupMemoryAllocator();
		
		shaderProgram = new(_vertexShaderSrc, _fragmentShaderSrc, _geometryShaderSrc);

		const float r = .05f;
		const float vel = .0005f;
		const float temperature = 1500f;
		const int c = 1_200_000;
		Random rnd = new();

		color col = "#ffaa3303";
		color col1 = "#ffaa3388";

		for (int i = 0; i < c; i++) {
			float x = (rnd.NextSingle() * 2 - 1) * r;
			float y = (rnd.NextSingle() * 2 - 1) * r;

			float vx = (rnd.NextSingle() * 2 - 1) * vel;
			float vy = (rnd.NextSingle() * 2 - 1) * vel;
			
			float t = rnd.NextSingle() * temperature;
			
			_points2.Add(new(new(x,y), new(vx, vy), col, new(0,0), new(t,0)));
		}

		//mesh = new(_points, _indices);
		//mesh.topology = PrimitiveType.Triangles;

		mesh2 = new(_points2.ToArray());
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

	protected override unsafe void OnRenderFrame(FrameEventArgs args) {
		const float spd = 1f;
		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		shaderProgram.Bind();
		
		GL.Enable(EnableCap.Blend);
		GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.One);
		// GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

		int c = mesh2.vertices.count;
		Vertex* ptr = mesh2.vertices.dataPtr;
		float time = (float)DateTime.Now.TimeOfDay.TotalMilliseconds * spd;

		Random rnd = new();

		float2 gravity0 = new float2(MathF.Sin(time * .00012f), MathF.Cos(time * .000008f)) * .7f;
		float2 gravity1 = new float2(MathF.Sin(time * .0012f), MathF.Cos(time * .00008f)) * .6f;
		float2 gravity2 = new float2(MathF.Cos(time * .0004f), MathF.Sin(time * .00003f)) * .8f;

		color col0 = "#22080501";
		color col1 = "#ee993309";
		
		Parallel.For(0, c, i => {
			float2 vel = ptr[i].normal;
			float2 pos = ptr[i].position.xy + vel * spd;
			float temperature = ptr[i].uv1.x;

			if (pos.x is < -1 or > 1) vel.FlipX();
			if (pos.y is < -1 or > 1) vel.FlipY();

			pos = float2.Clamp(pos, -float2.one, float2.one);
			
			float2 vec = (pos - gravity0) * 500;
			float len = vec.length;
			float g = 9.81f / (len * len * len);
			vel -= vec * g * .05f * spd;
			temperature += 1000f / len;
			
			vec = (pos - gravity1) * 500;
			len = vec.length;
			g = 9.81f / (len * len * len);
			vel -= vec * g * .05f * spd;
			temperature += 1000f / len;
			
			vec = (pos - gravity2) * 500;
			len = vec.length;
			g = 9.81f / (len * len * len);
			vel -= vec * g * .05f * spd;
			temperature += 1000f / len;

			// for (int j = 0; j < gC; j++) {
			// 	float2 vec = (pos - gravity[j].xy) * 1000000;
			// 	float len = vec.length;
			// 	if (len < .1f) continue;
			// 	len -= .01f;
			// 	float g = 9.81f / (len * len * len);
			// 	vel -= vec * g * .001f * gravity[j].z;
			// }

			temperature *= .996f / spd;
			ptr[i].position = pos;
			ptr[i].normal = vel;
			ptr[i].uv1 = new(temperature, 0);
			float tempV = math.min(temperature * .0001f, 1f);
			ptr[i].color = color.Lerp(col0, col1, tempV);
		});

		//mesh.updateRequired = true;
		// Vertex vert = mesh2.vertices[^1];
		// vert.position.x = MathF.Sin((float)DateTime.Now.TimeOfDay.TotalMilliseconds * .001f) * .5f;
		// //vert.position.z = -.1f;
		// vert.color = color.yellow;
		// mesh2.vertices[^1] = vert;

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