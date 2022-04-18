using System.Reflection;
using MathStuff;
using MathStuff.vectors;
using OpenTK.Graphics.OpenGL.Compatibility;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Quartz.CoreCs.other;
using Quartz.CoreCs.other.events;
using Quartz.CoreCs.other.time;
using Quartz.debug.log;
using Quartz.Ecs.ecs.archetypes;
using Quartz.Ecs.ecs.attributes;
using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.identifiers;
using Quartz.Ecs.ecs.jobs;
using Quartz.Ecs.ecs.prefabs;
using Quartz.Ecs.ecs.prefabs.load;
using Quartz.Ecs.ecs.views;
using Quartz.Ecs.ecs.worlds;
using Quartz.graphics.camera;
using Quartz.graphics.render;
using Quartz.graphics.render.renderers.ecs;
using Quartz.graphics.render.targets;
using Quartz.graphics.shaders;
using Quartz.graphics.shaders.materials;
using Quartz.objects.hierarchy.ecs;
using Quartz.objects.mesh;
using Quartz.objects.particles;
using Quartz.objects.particles.custom;
using Quartz.objects.particles.emitters;
using Quartz.objects.scenes;
using Quartz.utils;

namespace Quartz.ui.windows; 

public class QuartzWindow : ImGuiWindow {
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
		new(new(-0.5f, -0.5f), color.white),
		new(new(-0.5f,  0.5f), color.softRed),
		new(new( 0.5f,  0.5f), color.softPurple),
		new(new( 0.5f, -0.5f), color.softCyan),
	};

	private readonly ushort[] _indices = { 0, 1, 2, 0, 2, 3 };

	private readonly List<Vertex> _points2 = new();
	
	public QuartzWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) {
		
	}

	public record struct ParticleEmitterComponent(float2 pos, int count) : IComponent {
		public float2 pos = pos;
		public int count = count;
		
		public void Parse(Dictionary<string, string> fields) {
			string? v;
			if (fields.TryGetValue("x", out v)) pos.x = float.Parse(v);
			if (fields.TryGetValue("y", out v)) pos.y = float.Parse(v);
			if (fields.TryGetValue("count", out v)) count = int.Parse(v);
		}
		public void Write(Dictionary<string, string> fields) {
			//fields.Add("value", value.ToString());
		}
	}
	
	public record struct TinyTestComponent(byte a) : IComponent {
		public byte a = a;
		
		public void Parse(Dictionary<string, string> fields) {
			string? v;
			if (fields.TryGetValue("count", out v)) a = byte.Parse(v);
		}
		public void Write(Dictionary<string, string> fields) {
		}
	}

	[Require(typeof(TinyTestComponent))]
	public record struct SmallTestComponent(float2 pos) : IComponent {
		public float2 pos = pos;
		
		public void Parse(Dictionary<string, string> fields) {
			string? v;
			if (fields.TryGetValue("x", out v)) pos.x = float.Parse(v);
			if (fields.TryGetValue("y", out v)) pos.y = float.Parse(v);
		}
		public void Write(Dictionary<string, string> fields) {
		}
	}

	// public class SmallTestSystem : EntitySystem, IAutoEntitySystem {
	// 	public EventTypes eventTypes => EventTypes.fixedUpdate;
	// 	public bool repeating => true;
	//
	// 	protected override unsafe void Run() {
	// 		//Stopwatch sw = Stopwatch.StartNew();
	// 		World.ForeachWorld(w => w.Foreach<TinyTestComponent>(c1 => { c1->a++; }));
	// 		//Console.WriteLine($"{sw.ElapsedMilliseconds}ms");
	//
	// 		//Console.WriteLine($"{world.currentEntityCount} {world.archetypeCount} {world.GetTotalComponentCount()} {world.GetUniqueComponentCount()}");
	// 	}
	// }
	
	
	private static ParticleSystem ps;

	public class ParticleEmitTestSystem : EntitiesJobBase {
		public override void Run() {
			JobScheduler.Schedule<ParticleEmitterComponent>(EmitParticles, JobScheduleSettings.singleThreadedImmediate);
			this.Schedule(JobScheduleSettings.@default);
		}

		private static unsafe void EmitParticles(ComponentsView<ParticleEmitterComponent> view, JobState state) {
			for (int i = 0; i < view.count; i++) {
				const float s = .1f;
				(float2 pos, int count) = view.component0[i];
				
				ParticleData min = new() {color = "#f75", lifetime = .2f, position = pos, velocity = 0};
				ParticleData max = new() {color = "#fb9", lifetime = .5f, position = pos, velocity = 0};
				
				IParticleEmitter emitter = new ParticleEmitters.Cone(0, 0, .05f, .5f, 0, MathF.PI * .1f, 8);
				ps.Spawn(count, min, max, emitter);
			}
		}
		// public EventTypes eventTypes => EventTypes.render;
		// public bool repeating => true;
		// public bool continueInvoke => true;
		// public float lifetime => float.MaxValue;
		// public bool invokeWhileInactive => false;
		//
		//
		// protected override unsafe void Run() {
		// 	World.ForeachWorld(w => {
		// 		w.Foreach<ParticleEmitterComponent>((c1) => {
		// 			float s = .1f;
		// 			(float2 pos, int count) = *c1;
		// 			ParticleData min = new() {
		// 				color = "#f75", lifetime = .2f, position = pos, velocity = 0,
		// 			};
		// 			ParticleData max = new() {
		// 				color = "#fb9", lifetime = .5f, position = pos, velocity = 0,
		// 			};
		// 			IParticleEmitter emitter = new ParticleEmitters.Cone(0, 0, .05f, .5f, 0, MathF.PI * .1f, 8);
		// 			ps.Spawn(count, min, max, emitter);
		// 			//c1->pos.x -= .001f;
		// 		});
		// 	
		// 		// w.Foreach<PositionComponent>(c => {
		// 		// 	c->value += new float3(.01f, 0, 0);
		// 		// });
		// 	});
		// }
	}

	// private unsafe void TestF(World world, EntityId parent, float chance, ref int i) {
	// 	while (Rand.Bool(chance)) {
	// 		EntityId child = world.CreateEntity<HierarchyComponent>();
	// 		*world.Comp<HierarchyComponent>(child) = new();
	// 		world.AddChild(parent, child);
	// 		TestF(world, child, chance * .75f, ref i);
	// 		i++;
	// 	}
	// 	
	// }

	// public class Sys : EntitySystem {
	// 	protected override unsafe void Run() {
	// 		int a = 0;
	// 		World.ForeachWorld(w => {
	// 			w.Foreach<HierarchyComponent>(h => {
	// 				if (h->parent > 1000 && h->parent.isValid) {
	// 					//Debugger.Break();
	// 				}
	// 				a++;
	// 				Log.Message(new string('.', h->hierarchyLevel) + *h);
	// 			});
	// 		});
	// 		Console.WriteLine($"das {a}");
	// 	}
	// }

	protected override void OnLoad() {
		mainWindow ??= this;
		AppDomain.CurrentDomain.ProcessExit += (_,_) => OnUnload();

		Camera cam = new();
		cam.renderer.targets.Add(new CameraRenderTarget());

		EventManager.ProcessCurrentAssembly();
		EventManager.OnStart();
		
		//GL.Enable(EnableCap.Blend);
		//GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.SrcAlpha);
		World world = SceneManager.current!.world;

		//TODO: prefabs
		//Prefab p = Prefab.New("basic_entity_prefab")
		//                 .Set<HierarchyComponent>()
		//                 .OnSpawn((p,e) => Console.WriteLine($"spawned {e} from {p}"));

		// YamlStream yaml = new YamlStream();
		// yaml.Documents[0].RootNode.
		//Prefab particleEmitterPrefab = p.ClonePrefab("particle_emitter_entity_prefab")
		//                                .Set<ParticleEmitterComponent>(() => new(Rand.Float2(-1, 1), Rand.Int(2, 20)));
		
		//particleEmitterPrefab.Spawn(world, 6);

		const string yml = @"---
test_prefab:
    position:
        x: 5.87
        y: -89
    rotation:
        degrees: 90
    matrix:
";
		PrefabLoader.Load(yml);
		
		//YamlPrefabParser.ParseAndAddPrefabs(yml);
		
		// YamlStream yaml = new();
		// using StringReader sr = new(yml);
		// yaml.Load(sr);
		// YamlMappingNode map = (YamlMappingNode)yaml.Documents[0].RootNode;
		//
		// Stack<(YamlNode k, YamlNode v, int depth)> stack = new();
		// foreach (KeyValuePair<YamlNode, YamlNode> keyValuePair in map.Children) 
		// 	stack.Push((keyValuePair.Key, keyValuePair.Value, 0));
		//
		// while (stack.Count > 0) {
		// 	(YamlNode k, YamlNode v, int depth) v = stack.Pop();
		// 	
		// 	if (v.v.NodeType == YamlNodeType.Scalar)
		// 		Console.WriteLine($"{new(' ', v.depth * 4)}{v.k}: {v.v}");
		// 	if (v.v.NodeType == YamlNodeType.Mapping) {
		// 		Console.WriteLine($"{new(' ', v.depth * 4)}{v.k}:");
		// 		map = (YamlMappingNode)v.v;
		// 		foreach (KeyValuePair<YamlNode, YamlNode> keyValuePair in map.Children)
		// 			stack.Push((keyValuePair.Key, keyValuePair.Value, v.depth + 1));
		// 	}
		// }
		
		unsafe {

			Material mat = new(new(_vertexShader1Src, _fragmentShader1Src));
			Mesh mesh = new(_points, _indices);

			Prefab prefab0 = Prefab.New("prefab 0")
			                       .Set<PositionComponent>(() => new(new(Rand.Range(-40, 0), Rand.Range(-5, 5))))
			                       .Set(new ScaleComponent(.5f))
			                       .Set(new Rotation2DComponent(MathF.PI * .25f))
			                       .Set(new RendererComponent(mat, 0, RenderingPass.opaque, true))
			                       .Set(new MeshComponent(mesh))
			                       .Set<MatrixComponent>()
			                       .Set<AabbComponent>();
			
			Prefab prefab1 = Prefab.New("particle sys")
			                       .Set<ParticleEmitterComponent>(() => new(Rand.Float2(-1, 1), Rand.Int(2, 20)));
			
			prefab1.Spawn(16);
			prefab0.Spawn(1000);

			// Archetype archetype = world.GetArchetype<PositionComponent, ScaleComponent, Rotation2DComponent, MatrixComponent, RendererComponent, MeshComponent, AabbComponent>();
			// world.AddEntities(1000, archetype, e => {
			// 	world.Comp<PositionComponent>(e)->value = new(Rand.Range(-40, 0), Rand.Range(-5, 5));
			// 	world.Comp<ScaleComponent>(e)->value = .5f;
			// 	world.Comp<Rotation2DComponent>(e)->value = MathF.PI * .25f;
			// 	
			// 	*world.Comp<RendererComponent>(e) = new(mat, 0, RenderingPass.opaque, true);
			// 	world.Comp<MeshComponent>(e)->value = mesh;
			// 	if (Rand.Bool(.1f)) e.name = $"#{Guid.NewGuid()}";
			// });

			// world.AddEntities(10, archetype, e => {
			// 	world.Comp<PositionComponent>(e)->value = new(Rand.Range(-40, 0), Rand.Range(-5, 5));
			// 	//world.Comp<PositionComponent>(e)->value.z = p->value.y * .0001f;
			// 	
			// 	world.Comp<ScaleComponent>(e)->value = .5f;
			// 	world.Comp<Rotation2DComponent>(e)->value = MathF.PI * .25f;
			// 	*world.Comp<RendererComponent>(e) = new(mat, 0, RenderingPass.opaque, true);
			// 	*world.Comp<MeshComponent>(e) = new(mesh);
			// });

			// world.CreateEntitiesForeachComp<PositionComponent, ScaleComponent, Rotation2DComponent, MatrixComponent, RendererComponent, MeshComponent, AabbComponent>(1_000, (p, s, r, c2, c3, c4, c5) => {
			// 	p->value = new(Rand.Range(-40, 0), Rand.Range(-5, 5));
			// 	p->value.z = p->value.y * .0001f;
			// 	s->value = .5f;
			// 	r->value = MathF.PI * .25f;
			// 	*c3 = new(mat, 0, RenderingPass.opaque, true);
			// 	c4->value = mesh;
			// });

			//World.Select<PositionComponent>()
			//     .DestroyEntities(v => v->value.x is < -30 or > 30);

			// Console.WriteLine("--------------");
			// int a = 0;
			// TestF(world, e0, .9f, ref a);
			// TestF(world, e1, .8f, ref a);
			// TestF(world, e2, .7f, ref a);
			// Console.WriteLine($"jfvsdhf {a}");
			// Console.WriteLine("--------------");
			//
			// Sys s = new();
			// s.Execute(world);
		}

		// Task.Run(async () => {
		// 	await Task.Delay(200);
		// 	world.RegisterSystemsFromAssembly(Assembly.GetExecutingAssembly());
		// });
		// world.RegisterSystemsFromAssembly(Assembly.GetExecutingAssembly());
		ps = new TestParticleSystem();
		shaderProgram = new(_vertexShaderSrc, _fragmentShaderSrc, _geometryShaderSrc);
		
		Dispatcher.global.PushRepeating(() => ps.Update(Time.fixedDeltaTime), EventTypes.fixedUpdate);
		Dispatcher.global.PushRepeating(() => {
			shaderProgram.Bind();
			ps.Render();
		}, EventTypes.render);
		//world.RemoveEntity(e2);

		GL.Enable(EnableCap.DebugOutput);
		GL.Enable(EnableCap.DepthTest);
		GL.FrontFace(FrontFaceDirection.Cw);
		//GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

		color clearCol = color.steelBlue;
		GL.ClearColor(clearCol.rF, clearCol.gF, clearCol.bF, clearCol.aF);

		OpenGl.CheckErrors();
		base.OnLoad();
		
		new ParticleEmitTestSystem().Run();
		//Assembly.GetExecutingAssembly().RegisterSystemsFromAssembly();
		// EventsPipeline.Resume();
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
	
	

	protected override void OnUpdateFrame(FrameEventArgs args) {
		base.OnUpdateFrame(args);
		
		EventManager.Update();
	}

	protected override unsafe void OnRenderFrame(FrameEventArgs args) {
		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		GL.Enable(EnableCap.Blend);
		GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
		GL.Enable(EnableCap.DepthTest);

		Camera.main!.UpdateTransform();
		Camera.main!.targetSize = new(Size.X, Size.Y);
		GL.Viewport(0,0, Size.X, Size.Y);
		RenderManager.Render();
		
		base.OnRenderFrame(args);
		
		SwapBuffers();
		VSync = VSyncMode.Off;

		//SwapBuffers();
		//int b = 0;
		//GL.GetInteger(GetPName.RenderbufferBinding, ref b);
		//
		//Console.WriteLine(b);
		// if (!_renderTarget.Bind()) return;
		// shaderProgram.Bind();
		// _renderTarget.UnBind();
	}
}