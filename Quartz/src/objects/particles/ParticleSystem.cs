using MathStuff;
using MathStuff.vectors;
using OpenTK.Graphics.OpenGL.Compatibility;
using Quartz.objects.mesh;
using Quartz.objects.particles.emitters;
using Quartz.other;
using Quartz.other.time;

namespace Quartz.objects.particles; 

public abstract class ParticleSystem {
	public TinyPointMesh mesh = new();
	private object _lock = new();
	public int particleCount => mesh.vertices.count;

	protected abstract ParticleData UpdateParticle(ParticleData particle, float deltaTime);

	public virtual unsafe void Update(float deltaTime) {
		lock (_lock) {
			ParticleData* ptr = (ParticleData*)mesh.vertices.ptr;
			Parallel.For(0, particleCount, i => ptr[i] = UpdateParticle(ptr[i], deltaTime));
		
			if (Rand.Bool(.0025f)) SortData();
		}
	}
	
	public void Spawn(int count, ParticleData min, ParticleData max) {
		lock (_lock) for (int i = 0; i < count; i++) SpawnParticle(ParticleData.Random(min, max));
	}
	
	public void Spawn(int count, ParticleData min, ParticleData max, IParticleEmitter emitter) {
		lock (_lock) for (int i = 0; i < count; i++) SpawnParticle(emitter.Apply(ParticleData.Random(min, max)));
	}

	protected unsafe void SpawnParticle(ParticleData data) {
		int c = particleCount;
		ParticleData* ptr = (ParticleData*)mesh.vertices.ptr;
		for (int i = 0; i < c; i++) {
			if (ptr[i].lifetime > 0) continue;
			ptr[i] = data;
			return;
		}
		mesh.vertices.EnsureFreeSpace(1);
		ptr = (ParticleData*)mesh.vertices.ptr;
		ptr[mesh.vertices.count++] = data;
	}

	protected unsafe int Partition(ParticleData* particles, int low, int high) {
		float x = particles[high].lifetime;
		int i = low - 1;

		for (int j = low; j < high - 1; j++) {
			if (particles[j].lifetime < x) continue;
			i++;
			(particles[i], particles[j]) = (particles[j], particles[i]);
		}
		i++;
		(particles[i], particles[high]) = (particles[high], particles[i]);
		return i;
	}

	protected unsafe void SortData() {
		int pCount = particleCount;
		if (pCount == 0) return;
		
		ParticleData* arr = (ParticleData*) mesh.vertices.ptr;
		if (pCount == 1) {
			if (arr->lifetime <= 0) mesh.vertices.count = 0;
			return;
		}
		int low = 0;
		int high = pCount - 1;
		int[] stack = new int[high + 1];
		int top = -1;

		stack[++top] = low;
		stack[++top] = high;
		
		while (top >= 0) {
			high = stack[top--];
			low = stack[top--];

			int p = Partition(arr, low, high);

			if (p - 1 > low) {
				stack[++top] = low;
				stack[++top] = p - 1;
			}

			if (p + 1 >= high) continue;
			stack[++top] = p + 1;
			stack[++top] = high;
		}

		int c = pCount - 1;
		while (c > 0 && arr[c].lifetime <= 0) c--;

		mesh.vertices.count = c + 1;
		if (c == 1 && arr->lifetime <= 0) mesh.vertices.count = 0;
	}

	public void Render() {
		mesh.Bind();
		MeshUtils.UpdateBuffer(mesh.vertices, BufferTargetARB.ArrayBuffer, BufferUsageARB.DynamicDraw, mesh.vertices.capacity, mesh.oldVertexCount);
		mesh.oldVertexCount = mesh.vertices.capacity;
		GL.DrawArrays(mesh.getTopology, 0, mesh.vertices.count);
	}
}

public struct ParticleData {
	public float3 position;
	public float3 velocity;
	public color color;
	public float2 uv;
	public float lifetime;
	public float reserved0;

	public static ParticleData Lerp(ParticleData a, ParticleData b, float t) =>
		new() {
			position = Lerp(a.position, b.position, t), 
			velocity = Lerp(a.velocity, b.velocity, t), 
			color = Lerp((float4)a.color, b.color, t), 
			uv = Lerp(a.uv, b.uv, t),
			lifetime = math.lerp(a.lifetime, b.lifetime, t),
			reserved0 = math.lerp(a.reserved0, b.reserved0, t)
		};
	
	public static ParticleData Random(ParticleData a, ParticleData b) =>
		new() {
			position = Lerp(a.position, b.position, Rand.val), 
			velocity = Lerp(a.velocity, b.velocity, Rand.val), 
			color = Lerp((float4)a.color, b.color, Rand.val), 
			uv = Lerp(a.uv, b.uv, Rand.val),
			lifetime = math.lerp(a.lifetime, b.lifetime, Rand.val),
			reserved0 = math.lerp(a.reserved0, b.reserved0, Rand.val)
		};

	private static float2 Lerp(float2 a, float2 b, float t) => b * t + a * (1 - t);
	private static float3 Lerp(float3 a, float3 b, float t) => b * t + a * (1 - t);
	private static float4 Lerp(float4 a, float4 b, float t) => b * t + a * (1 - t);
}