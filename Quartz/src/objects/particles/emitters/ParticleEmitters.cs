using MathStuff;
using MathStuff.vectors;
using Quartz.other;

namespace Quartz.objects.particles.emitters;

public static class ParticleEmitters {
	public struct Basic : IParticleEmitter {
		public static readonly Basic general = new();
		public ParticleData Apply(ParticleData pd) => pd;
	}

	public record struct Circle(float minRadius, float maxRadius, float minVelocity, float maxVelocity) : IParticleEmitter {
		public static readonly Circle general = new(0,1,0,1);
		public float minRadius = minRadius;
		public float maxRadius = maxRadius;
		public float minVelocity = minVelocity;
		public float maxVelocity = maxVelocity;
		
		public ParticleData Apply(ParticleData pd) {
			float angle = Rand.val * MathF.PI * 2;
			float radius = Rand.Range(minRadius, maxRadius);
			float velocityMul = Rand.Range(minVelocity, maxVelocity);
			float2 sincos = float2.SinCos(angle, 1f);
			
			pd.position += (float3) sincos * radius;
			pd.velocity += (float3)sincos * velocityMul;
			return pd;
		}
	}
	
	public record struct Cone(float minRadius, float maxRadius, float minVelocity, float maxVelocity, float rotation, float angle, float distribution = 1) : IParticleEmitter {
		public static readonly Cone general = new(0, 1, 0, 1, 0, MathF.PI * .5f);
		public float minRadius = minRadius;
		public float maxRadius = maxRadius;
		public float minVelocity = minVelocity;
		public float maxVelocity = maxVelocity;
		public float rotation = rotation;
		public float angle = angle;
		public float distribution = distribution;

		public ParticleData Apply(ParticleData pd) {
			float particleAngle = rotation + angle * (Rand.ValueWithDistribution(distribution) - .5f);
			float radius = Rand.Range(minRadius, maxRadius);
			float velocityMul = Rand.Range(minVelocity, maxVelocity);
			float2 sincos = float2.SinCos(particleAngle, 1f);
			
			pd.position += (float3) sincos * radius;
			pd.velocity += (float3)sincos * velocityMul;
			return pd;
		}
	}
}