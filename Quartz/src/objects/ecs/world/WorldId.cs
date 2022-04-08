namespace Quartz.objects.ecs.world; 

public readonly struct WorldId {
	public static readonly WorldId @null = new(ushort.MaxValue);
	public readonly ushort id;

	public World world => World.GetWorld(id);

	public bool isValid => id != ushort.MaxValue;
	
	public WorldId(ushort id) => this.id = id;

	public static implicit operator ushort(WorldId v) => v.id;
	public static implicit operator WorldId(ushort v) => new(v);
	
	public override string ToString() => id.ToString();
}