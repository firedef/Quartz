using Quartz.Ecs.ecs.components;
using Quartz.Ecs.ecs.identifiers;

namespace Quartz.Ecs.ecs.worlds;

public partial class World {
	public unsafe void* TryAdd(EntityId e, ComponentType t) {
		lock (_lock) return archetypes.TryAddComponent(e, t);
	}
	public unsafe void* TryAdd(EntityId e, Type t) {
		lock (_lock) return archetypes.TryAddComponent(e, t.Get());
	}
	public unsafe TNormal* TryAdd<TNormal>(EntityId e) where TNormal : unmanaged, IComponent => (TNormal*)TryAdd(e, typeof(TNormal));
	
	public unsafe void* Comp(EntityId e, ComponentType t) {
		lock (_lock) return archetypes.GetOrAddComponent(e, t);
	}
	public unsafe void* Comp(EntityId e, Type t) {
		lock (_lock) return archetypes.GetOrAddComponent(e, t.Get());
	}
	public unsafe TNormal* Comp<TNormal>(EntityId e) where TNormal : unmanaged, IComponent => (TNormal*)Comp(e, typeof(TNormal));

	public unsafe void SetComponent(EntityId e, ComponentType t, Dictionary<string, string> fields) {
		lock (_lock) {
			archetypes.SetComponent(e, t, fields);
		}
	}
	
	public unsafe void* TryComp(EntityId e, ComponentType t) {
		lock (_lock) return archetypes.GetComponent(e, t);
	}
	public unsafe void* TryComp(EntityId e, Type t) {
		lock (_lock) return archetypes.GetComponent(e, t.Get());
	}
	public unsafe TNormal* TryComp<TNormal>(EntityId e) where TNormal : unmanaged, IComponent => (TNormal*)TryComp(e, typeof(TNormal));
	
	public unsafe ushort* SharedIndex<TShared>(EntityId e) where TShared : unmanaged, ISharedComponent => (ushort*)Comp(e, typeof(TShared));
	public unsafe ushort* TrySharedIndex<TShared>(EntityId e) where TShared : unmanaged, ISharedComponent => (ushort*)TryComp(e, typeof(TShared));
	public unsafe TShared* SharedComponent<TShared>(EntityId e) where TShared : unmanaged, ISharedComponent {
		ushort index = *SharedIndex<TShared>(e);
		return index == 0 ? null : sharedComponents.Get<TShared>(index);
	}
	public static ushort AddSharedComponent<TShared>(TShared v) where TShared : unmanaged, ISharedComponent {
		lock (_lock) return sharedComponents.Add(v);
	}
	public static unsafe TShared* GetSharedComponent<TShared>(ushort index) where TShared : unmanaged, ISharedComponent {
		lock (_lock) return sharedComponents.Get<TShared>(index);
	}
	public static void RemoveSharedComponent<TShared>(ushort index) where TShared : unmanaged, ISharedComponent {
		lock (_lock) sharedComponents.Remove<TShared>(index);
	}
	public static void ClearSharedComponents<TShared>() where TShared : unmanaged, ISharedComponent {
		lock (_lock) sharedComponents.Clear<TShared>();
	}

	public void RemoveComp(EntityId e, ComponentType t) {
		lock (_lock) archetypes.RemoveComponent(e, t);
	}
	public void RemoveComp<T>(EntityId e) {
		lock (_lock) archetypes.RemoveComponent(e, typeof(T).Get());
	}

}