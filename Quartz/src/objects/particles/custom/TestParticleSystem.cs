using MathStuff;
using MathStuff.vectors;
using Quartz.other.time;

namespace Quartz.objects.particles.custom; 

public class TestParticleSystem : ParticleSystem {
	protected override ParticleData UpdateParticle(ParticleData particle, float deltaTime) {
		float2 pos = particle.position;
		float2 vel = particle.velocity;
		
		if (pos.x is < -1 or > 1) vel.FlipX();
		if (pos.y is < -1 or > 1) vel.FlipY();

		pos += vel * deltaTime;
		pos = float2.Clamp(pos, -float2.one, float2.one);
		//vel.y -= .981f * deltaTime;
		
		particle.velocity = vel;
		particle.position = pos;
		particle.lifetime -= deltaTime;
		particle.color.aF = math.clamp(particle.lifetime, 0, 1);

		return particle;
	}
}