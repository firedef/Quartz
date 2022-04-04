using System.Reflection;
using MathStuff;
using MathStuff.vectors;
using OpenTK.Graphics.OpenGL.Compatibility;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Quartz.collections;
using Quartz.core;
using Quartz.core.collections;
using Quartz.graphics.camera;
using Quartz.graphics.render;
using Quartz.graphics.render.renderers;
using Quartz.graphics.render.targets;
using Quartz.graphics.shaders;
using Quartz.graphics.shaders.materials;
using Quartz.objects.ecs.archetypes;
using Quartz.objects.ecs.components;
using Quartz.objects.ecs.components.types.graphics;
using Quartz.objects.ecs.components.types.transform;
using Quartz.objects.ecs.entities;
using Quartz.objects.ecs.managed;
using Quartz.objects.ecs.systems;
using Quartz.objects.ecs.world;
using Quartz.objects.memory;
using Quartz.objects.mesh;
using Quartz.objects.particles;
using Quartz.objects.particles.custom;
using Quartz.objects.particles.emitters;
using Quartz.objects.scenes;
using Quartz.other;
using Quartz.other.events;
using Quartz.other.time;
using Quartz.utils;

namespace Quartz.ui.windows; 

public class QuartzWindow : GameWindow {
	public static QuartzWindow? mainWindow;
	public ShaderProgram shaderProgram;

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
	
	
	
	
	
	
	
	private const string _vertexShader1Src = @"
#version 330
in layout(location=0) vec3 position;
in layout(location=1) vec3 normal;
in layout(location=2) vec4 color;
in layout(location=3) vec2 uv0;
in layout(location=4) vec2 uv1;

uniform mat4 mvp;
out vec4 v_col;

void main(void) {
	gl_Position = vec4(position, 1.0) * mvp;
	v_col = color.bgra;
}";

	private const string _fragmentShader1Src = @"
#version 330
in vec4 v_col;
out vec4 outputColor;
void main(void) { 
	outputColor = v_col; 
}";

	private readonly Vertex[] _points = {
		new(new(-0.4f, -0.5f), color.white),
		new(new(-0.5f,  0.5f), color.softRed),
		new(new( 0.5f,  0.5f), color.softPurple),
		new(new( 0.2f, -0.5f), color.softCyan),
	};

	private readonly ushort[] _indices = { 0, 1, 2, 0, 2, 3 };

	private readonly List<Vertex> _points2 = new();
	
	public QuartzWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) {
		GLFW.WindowHint(WindowHintInt.Samples, 4);
	}

	public record struct ParticleEmitterComponent(float2 pos, int count) : IComponent {
		public float2 pos = pos;
		public int count = count;
	} 
	
	
	private static ParticleSystem ps;

	public class ParticleEmitTestSystem : EntitySystem, IAutoEntitySystem {
		public EventTypes types => EventTypes.render;
		public bool repeating => true;
		public bool continueInvoke => true;
		public float lifetime => float.MaxValue;
		public bool invokeWhileInactive => false;

		public override unsafe void Run(World world) => world.Foreach<ParticleEmitterComponent>((c1) => {
			float s = .1f;
			(float2 pos, int count) = *c1;
			ParticleData min = new() {color = color.softRed, lifetime = .2f, position = pos, velocity = 0,};
			ParticleData max = new() {color = color.softYellow, lifetime = .5f, position = pos, velocity = 0,};
			IParticleEmitter emitter = new ParticleEmitters.Cone(0, 0, .05f, .5f, MathF.PI * .5f, MathF.PI * .4f, 8);
			ps.Spawn(count, min, max, emitter);
			c1->pos.x -= .001f;
			//Camera.main.position.x -= .0025f;
			//Camera.main.position.y += .001f;
		});
		
		// public override unsafe void Run(World world) => world.ForeachComponentPtr<ParticleEmitterComponent>((c1, _) => {
		// 	float s = .1f;
		// 	(float2 pos, int count) = *c1;
		// 	ParticleData min = new() {color = color.softRed, lifetime = .2f, position = pos, velocity = 0,};
		// 	ParticleData max = new() {color = color.softYellow, lifetime = .5f, position = pos, velocity = 0,};
		// 	IParticleEmitter emitter = new ParticleEmitters.Cone(0, 0, .05f, .5f, MathF.PI * .5f, MathF.PI * .4f, 8);
		// 	ps.Spawn(count, min, max, emitter);
		// 	c1->pos.x -= .001f;
		// 	Camera.main.position.x -= .0025f;
		// 	Camera.main.position.y += .001f;
		// });
	}

	protected override unsafe void OnLoad() {
		mainWindow ??= this;
		AppDomain.CurrentDomain.ProcessExit += (_,_) => OnUnload();

		Camera cam = new();
		cam.renderer.targets.Add(new CameraRenderTarget());

		EventManager.ProcessCurrentAssembly();
		EventManager.OnStart();

		// ArchetypeRoot root = new();
		// Archetype archetype = root.FindOrCreateArchetype<ParticleEmitterComponent, RenderableComponent, TransformComponent>();
		// root.AddEntity(0, typeof(ParticleEmitterComponent));
		// root.AddEntity(1, typeof(ParticleEmitterComponent));
		// root.AddEntity(2, typeof(ParticleEmitterComponent));
		//
		// Material mat = new(new(_vertexShader1Src, _fragmentShader1Src));
		// Mesh mesh = new(_points, _indices);
		//
		// *root.GetComponent<RenderableComponent>(0) = new(mat, mesh, 0, RenderingPass.opaque);
		// *root.GetComponent<RenderableComponent>(1) = new(mat, mesh, 0, RenderingPass.opaque);
		// *root.GetComponent<RenderableComponent>(2) = new(mat, mesh, 0, RenderingPass.transparent);
		//
		// *root.GetComponent<TransformComponent>(0) = new(new float2(-15,0), 4);
		// *root.GetComponent<TransformComponent>(1) = new(new float2(-5,9), 6);
		// *root.GetComponent<TransformComponent>(2) = new(new float2(11, 2), 2);
		//
		// //Console.WriteLine(root.archetypes.Count);
		// Console.WriteLine(root.RemoveComponent<TransformComponent>(1));
		// //root.RemoveEntity(0);
		// //root.RemoveEntity(1);
		//
		// root.Foreach<TransformComponent, RenderableComponent>((c0, c1) => Console.WriteLine("\n\n\n---\n"+*c0+"\n"+*c1));

		World world = SceneManager.current!.ecsWorld;
		
		EntityId e0 = world.CreateEntity<ParticleEmitterComponent>();
		EntityId e1 = world.CreateEntity<ParticleEmitterComponent>();
		EntityId e2 = world.CreateEntity<ParticleEmitterComponent>();
		
		*world.Comp<ParticleEmitterComponent>(e0) = new(new(.4f, -.2f), 5);
		*world.Comp<ParticleEmitterComponent>(e1) = new(new(.8f, .3f), 100);
		*world.Comp<ParticleEmitterComponent>(e2) = new(new(-.2f, -.2f), 2);
		
		Material mat = new(new(_vertexShader1Src, _fragmentShader1Src));
		Mesh mesh = new(_points, _indices);
		
		for (int i = 0; i < 100_000; i++) {
			EntityId e3 = world.CreateEntity<RenderableComponent, TransformComponent, OcclusionComponent>();
			*world.Comp<RenderableComponent>(e3) = new(mat, mesh, 0, RenderingPass.opaque);
			*world.Comp<TransformComponent>(e3) = new(new float2(Rand.Range(-100,100),Rand.Range(-10,10)), Rand.Range(.005f,.05f));
		}

		//Console.WriteLine(EcsManagedData<Material>.items.storage.Count);

		world.RegisterSystemsFromAssembly(Assembly.GetExecutingAssembly());
		ps = new TestParticleSystem();
		shaderProgram = new(_vertexShaderSrc, _fragmentShaderSrc, _geometryShaderSrc);
		
		Dispatcher.global.PushRepeating(() => ps.Update(Time.fixedDeltaTime), EventTypes.fixedUpdate);
		Dispatcher.global.PushRepeating(() => {
			shaderProgram.Bind();
			ps.Render();
		}, EventTypes.render);
		//world.RemoveEntity(e2);

		GL.Enable(EnableCap.DebugOutput);
		GL.FrontFace(FrontFaceDirection.Cw);
		//GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

		color clearCol = color.steelBlue;
		GL.ClearColor(clearCol.rF, clearCol.gF, clearCol.bF, clearCol.aF);

		OpenGl.CheckErrors();
		base.OnLoad();
		
	}

	protected override void OnUnload() {
		EventManager.OnQuit();
		// mesh.Dispose();
		// shaderProgram.Dispose();
		//
		// base.OnUnload();
	}

	protected override void OnResize(ResizeEventArgs e) {
		Camera.main!.targetSize = new(e.Width, e.Height);
		GL.Viewport(0,0, e.Width, e.Height);
		base.OnResize(e);
	}

	protected override unsafe void OnRenderFrame(FrameEventArgs args) {
		Camera.main!.UpdateTransform();
		
		EventManager.Update();
		RenderManager.Render();
		//int b = 0;
		//GL.GetInteger(GetPName.RenderbufferBinding, ref b);
		//
		//Console.WriteLine(b);
		// if (!_renderTarget.Bind()) return;
		// shaderProgram.Bind();
		// _renderTarget.UnBind();
	}
}