namespace Quartz.objects.ecs.entities; 

public readonly struct EntityId {
	public readonly uint id;

	public bool isValid => id != uint.MaxValue;
	
	public EntityId(uint id) => this.id = id;

	public static implicit operator uint(EntityId v) => v.id;
	public static implicit operator EntityId(uint v) => new(v);
	
	public static implicit operator int(EntityId v) => (int)v.id;
	public static implicit operator EntityId(int v) => new((uint)v);
	
	public override string ToString() => id.ToString();
}