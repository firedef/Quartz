using System.Collections;
using Quartz.collections;
using Quartz.objects.ecs.entities;
using Quartz.objects.memory;

namespace Quartz.objects.ecs.components;

public abstract class ComponentCollection {
	protected const int componentArrayInitialSize = 1 << 20;

	public DualIntMap entitiesComponentsMap = new();
	
	public abstract Type type { get; }
	public abstract int totalCount { get; }
	public abstract int componentCount { get; }
	public abstract int elementSize { get; }
}

public class ComponentCollection<T> : ComponentCollection, IEnumerable<T> where T : unmanaged, IComponent {
#region fields

	public NativeListPool<T> components = new(componentArrayInitialSize);

	public override Type type => typeof(T);
	public override int totalCount => components.count;
	public override int componentCount => components.count - components.emptyIndices.count;
	public override unsafe int elementSize => sizeof(T);

#endregion fields

#region elements

	public uint Set(EntityId entity, T component) {
		uint componentId = TryGetId(entity);
		if (componentId != uint.MaxValue) {
			components[(int)componentId] = component;
			return componentId;
		}
		componentId = (uint) components.Add(component);
		entitiesComponentsMap.Set(entity.id, componentId);
		
		return componentId;
	}

	public void Remove(EntityId entity) {
		uint componentId = TryGetId(entity);
		if (componentId == uint.MaxValue) return;
		components.RemoveAt((int)componentId);
		entitiesComponentsMap.Remove(entity.id, componentId);
	}
	
	public uint TryGetId(EntityId entity) => entitiesComponentsMap[entity.id];
	
	public unsafe T* IndexToPtr(uint componentId) => components.ptr + componentId;

	public unsafe T* TryGet(EntityId entity) {
		uint componentId = TryGetId(entity);
		return componentId == uint.MaxValue ? null : IndexToPtr(componentId);
	}

	public unsafe T* GetOrAdd(EntityId entity) {
		uint componentId = TryGetId(entity);
		if (componentId != uint.MaxValue) return IndexToPtr(componentId);
		
		componentId = (uint) components.Add(new());
		entitiesComponentsMap.Set(entity.id, componentId);
		return IndexToPtr(componentId);
	}

	public void Clear() {
		components.Clear();
		entitiesComponentsMap.Clear();
	}

	public unsafe T this[EntityId id] {
		get => *GetOrAdd(id);
		set => Set(id, value);
	}
	
	public unsafe T* this[uint id] => GetOrAdd(id);
	public unsafe T* this[int id] => GetOrAdd((uint) id);

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