using MathStuff;
using MathStuff.vectors;
using Quartz.other.time;

namespace Quartz.objects.particles.custom; 

public class TestParticleSystem : ParticleSystem {
	public float time;
	public float2 gravity0;
	public float2 gravity1;
	public float2 gravity2;

	public float spd = 1;

	public color col0 = "#22080502";
	public color col1 = "#ee993309";
	
	protected override ParticleData UpdateParticle(ParticleData particle, float deltaTime) {
		float temperature = particle.reserved0;
		float2 pos = particle.position;
		float2 vel = particle.velocity;
		
		if (pos.x is < -1 or > 1) vel.FlipX();
		if (pos.y is < -1 or > 1) vel.FlipY();
		
		pos = float2.Clamp(pos, -float2.one, float2.one);
		
		float2 vec = (pos - gravity0) * 500;
		float len = vec.length;
		float g = 9.81f / (len * len * len);
		vel -= vec * g * .05f * deltaTime;
		temperature += 1000f / len * deltaTime;
			
		vec = (pos - gravity1) * 500;
		len = vec.length;
		g = 9.81f / (len * len * len);
		vel -= vec * g * .05f * deltaTime;
		temperature += 1000f / len * deltaTime;
			
		vec = (pos - gravity2) * 500;
		len = vec.length;
		g = 9.81f / (len * len * len);
		vel -= vec * g * .05f * deltaTime;
		temperature += 1000f / len * deltaTime;
		
		temperature -= deltaTime * .0025f * temperature;
		float tempV = math.min(temperature * .0001f, 1f);

		particle.reserved0 = temperature;
		particle.velocity = vel;
		particle.position = pos + vel * deltaTime;
		particle.color = color.Lerp(col0, col1, tempV);

		return particle;
	}

	public override unsafe void Update(float deltaTime) {
		deltaTime *= spd;
		
		time += deltaTime;
		gravity0 = new(-((float) Time.elapsedSecondsGame * spd * .002f + .1f),.1f);
		gravity1 = new float2(MathF.Sin(time * .0012f), MathF.Cos(time * .00008f)) * .6f;
		gravity2 = new float2(MathF.Cos(time * .0004f), MathF.Sin(time * .00003f)) * .8f;
		base.Update(deltaTime);
	}
}