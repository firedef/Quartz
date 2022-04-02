namespace Quartz.objects.ecs.world; 

public struct WorldId {
	public readonly uint id;

	public bool isValid => id != uint.MaxValue;
	
	public WorldId(uint id) => this.id = id;

	public static implicit operator uint(WorldId v) => v.id;
	public static implicit operator WorldId(uint v) => new(v);
	
	public override string ToString() => id.ToString();
	
}