using System.Collections;
using Quartz.CoreCs.collections;
using Quartz.objects.ecs.entities;
// ReSharper disable InconsistentlySynchronizedField

namespace Quartz.objects.ecs.components;

public abstract class ComponentCollection {
	protected const int componentArrayInitialSize = 1 << 16;

	public DualIntMap entitiesComponentsMap = new();
	
	public abstract Type type { get; }
	public abstract int totalCount { get; }
	public abstract int componentCount { get; }
	public abstract int elementSize { get; }

	public abstract void Remove(EntityId entityId);
	
	public ComponentId GetComponentFromEntity(EntityId entityId) => entitiesComponentsMap.GetVal(entityId.id);
	public EntityId GetEntityFromComponent(ComponentId component) => entitiesComponentsMap.GetKey(component);

	public bool ContainsEntity(EntityId entityId) => entitiesComponentsMap.ContainsKey(entityId.id);
	public bool ContainsComponent(ComponentId component) => entitiesComponentsMap.ContainsVal(component);
}

public class ComponentCollection<T> : ComponentCollection, IEnumerable<T> where T : unmanaged, IComponent {
#region fields

	public NativeListPool<T> components = new(componentArrayInitialSize);

	public override Type type => typeof(T);
	public override int totalCount => components.count;
	public override int componentCount => components.count - components.emptyIndices.count;
	public override unsafe int elementSize => sizeof(T);
	
	private readonly object _lock = new();

#endregion fields

#region elements

	public unsafe void DisposeElement(ComponentId id) {
		if (components.ptr[id] is IDisposable d) d.Dispose();
	}

	public ComponentId Set(EntityId entityId, T component) {
		ComponentId componentId = GetComponentFromEntity(entityId);
		if (componentId.isValid) {
			components[(int)componentId.id] = component;
			return componentId;
		}
		lock (_lock) {
			componentId = (uint) components.Add(component);
			entitiesComponentsMap.Set(entityId.id, componentId);
		
			return componentId;
		}
	}

	public override void Remove(EntityId entityId) {
		ComponentId componentId = GetComponentFromEntity(entityId);
		if (!componentId.isValid) return;
		lock (_lock) {
			components.RemoveAt((int)componentId.id);
			DisposeElement(componentId);
			entitiesComponentsMap.Remove(entityId.id, componentId);
		}
	}
	
	
	public unsafe T* IndexToPtr(ComponentId componentId) => components.ptr + componentId;

	public unsafe T* TryGet(EntityId entityId) {
		ComponentId componentId = GetComponentFromEntity(entityId);
		return componentId.isValid ? IndexToPtr(componentId) : null;
	}

	public unsafe T* GetOrAdd(EntityId entityId) {
		ComponentId componentId = GetComponentFromEntity(entityId);
		if (componentId.isValid) return IndexToPtr(componentId);

		lock (_lock) {
			componentId = (uint) components.Add(new());
			entitiesComponentsMap.Set(entityId.id, componentId);
			return IndexToPtr(componentId);
		}
	}

	public void Clear() {
		lock (_lock) {
			components.Clear();
			entitiesComponentsMap.Clear();
		}
	}

	public unsafe T this[EntityId id] {
		get => *GetOrAdd(id);
		set => Set(id, value);
	}
	
	public unsafe T* this[ComponentId id] => components.ptr + id;
	public unsafe T* this[int id] => components.ptr + id;

	public int GetNextComponentIndex(int index) {
		index++;
		while (index < components.count && components.emptyIndices.Contains(index)) index++;
		return index;
	}

#endregion elements

#region enumerator

	public IEnumerator<T> GetEnumerator() => new Enumerator(this);
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
	public class Enumerator : IEnumerator<T> {
		public int pos = -1;
		public ComponentCollection<T> owner;

		public Enumerator(ComponentCollection<T> owner) => this.owner = owner;

		public bool MoveNext() {
			pos = owner.GetNextComponentIndex(pos);
			return pos < owner.components.count;
		}
		
		public void Reset() => pos = -1;
		public T Current => owner.components[pos];

		object IEnumerator.Current => Current;

		public void Dispose() { }
	}

#endregion enumerator
}