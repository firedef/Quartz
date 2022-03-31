using MathStuff;
using MathStuff.vectors;
using Quartz.objects.mesh;

namespace Quartz.objects.particles; 

public abstract class ParticleSystem {
	public TinyPointMesh mesh = new();
	public int particleCount => mesh.vertices.count;

	protected abstract ParticleData UpdateParticle(ParticleData particle, float deltaTime);

	public virtual unsafe void Update(float deltaTime) {
		ParticleData* ptr = (ParticleData*)mesh.vertices.dataPtr;
		Parallel.For(0, particleCount, i => ptr[i] = UpdateParticle(ptr[i], deltaTime));
	}
	
	
}

public struct ParticleData {
	public float3 position;
	public float3 velocity;
	public color color;
	public float2 uv;
	public float lifetime;
	public float reserved0;
}