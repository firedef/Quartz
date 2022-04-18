using Quartz.CoreCs.collections;
using Quartz.Ecs.collections;
using Quartz.Ecs.ecs.archetypes;
using Quartz.Ecs.ecs.identifiers;

namespace Quartz.Ecs.ecs.components.data;

public partial class ArchetypeComponents {
	private readonly Archetype _owner;
	public readonly NormalComponentArray _normal;
	public readonly SharedComponentArray _shared;

	private readonly object _lock = new();

	public int normalComponentCount => _normal.types.componentCount;
	public int sharedComponentCount => _shared.types.componentCount;
	
	public int elementCount { get; private set; }
	public int allocatedElementCount => normalComponentCount > 0 ? _normal.data[0].capacity : _shared.data[0].capacity;

	private readonly DoubleDictionary _entityComponentMap = new();
	// private readonly DualIntMap _entityComponentMap = new();

	public int entityComponentMapKeys => _entityComponentMap.keyCount;
	public int entityComponentMapValues => _entityComponentMap.valCount;

	public ArchetypeComponents(Archetype owner, ComponentTypeArray normal, ComponentTypeArray shared) {
		_owner = owner;
		_normal = new(normal);
		_shared = new(shared);
	}

	public ComponentId GetComponent(EntityId entity) { lock (_lock) return _entityComponentMap[entity]; }
	public EntityId GetEntity(ComponentId component) { lock (_lock) return _entityComponentMap.GetKey(component); }

	private void SetEntityComponent(EntityId entity, ComponentId component) => _entityComponentMap[entity] = component;
	private void RemoveEntityComponent(EntityId entity, ComponentId component) => _entityComponentMap.Remove(entity, component);

	public bool ContainsArchetype(ArchetypeComponents other) { lock (_lock) return _normal.types.Contains(other._normal.types) && _shared.types.Contains(other._shared.types); }
	
	public bool ContainsNormalComponents(ComponentTypeArray other) { lock (_lock) return _normal.types.Contains(other); }
	public bool ContainsSharedComponents(ComponentTypeArray other) { lock (_lock) return _shared.types.Contains(other); }
	
	public bool ContainsNormalComponent(ComponentType other) { lock (_lock) return _normal.types.Contains(other); }
	public bool ContainsSharedComponent(ComponentType other) { lock (_lock) return _shared.types.Contains(other); }
	
	public bool ContainsEntityId(EntityId entity) { lock (_lock) return _entityComponentMap.ContainsKey(entity); }
	public bool ContainsComponentId(ComponentId comp) => comp.position < elementCount;

	public int IndexOfNormalComponent(ComponentType t) { lock (_lock) return _normal.types.IndexOf(t); }
	public int IndexOfSharedComponent(ComponentType t) { lock (_lock) return _shared.types.IndexOf(t); }

	public ComponentId Add(EntityId entity) {
		lock (_lock) {
			if (ContainsEntityId(entity)) return ComponentId.@null;
			
			int normalCount = normalComponentCount;   
			for (int i = 0; i < normalCount; i++) _normal.data[i].Push();

			int sharedCount = sharedComponentCount;
			for (int i = 0; i < sharedCount; i++) _shared.data[i].Push();

			ComponentId component = elementCount++;
			SetEntityComponent(entity, component);
			return component;
		}
	}
	
	public void AddMultiple(EntityId[] entities) {
		lock (_lock) {
			int c = entities.Length;

			int normalCount = normalComponentCount;   
			for (int i = 0; i < normalCount; i++) _normal.data[i].PushMultiple(c);

			int sharedCount = sharedComponentCount;
			for (int i = 0; i < sharedCount; i++) _shared.data[i].PushMultiple(c);

			for (int i = 0; i < c; i++) _entityComponentMap[entities[i]] = (uint)(elementCount + i);
			elementCount += c;
		}
	}

	public bool Remove(EntityId entity, bool dispose = true) {
		lock (_lock) {
			uint comp = _entityComponentMap[entity];
			if (comp == uint.MaxValue || comp >= elementCount) return false;
			_entityComponentMap.Remove(entity, comp);
			elementCount--;
			
			int normalCount = normalComponentCount;
			int sharedCount = sharedComponentCount;
			
			if (comp == elementCount) {
				for (int i = 0; i < normalCount; i++) _normal.data[i].Pop(dispose);
				for (int i = 0; i < sharedCount; i++) _shared.data[i].Pop(dispose);
				return true;
			}

			int compI = (int)comp;
			for (int i = 0; i < normalCount; i++) _normal.data[i].RemoveByReplaceLast(compI, dispose);
			for (int i = 0; i < sharedCount; i++) _shared.data[i].RemoveByReplaceLast(compI, dispose);

			uint old = _entityComponentMap.GetKey((uint)elementCount);
			_entityComponentMap[old] = comp;

			return true;
		}
	}
	
	public bool RemoveComponents(ComponentId component) {
		lock (_lock) {
			if (!component.isValid || component.position >= elementCount) return false;
			RemoveEntityComponent(GetEntity(component), component);
			elementCount--;
			
			int normalCount = normalComponentCount;
			int sharedCount = sharedComponentCount;
			
			if (component.position == elementCount) {
				for (int i = 0; i < normalCount; i++) _normal.data[i].Pop(true);
				for (int i = 0; i < sharedCount; i++) _shared.data[i].Pop(true);
				return true;
			}
			
			for (int i = 0; i < normalCount; i++) _normal.data[i].RemoveByReplaceLast(component, true);
			for (int i = 0; i < sharedCount; i++) _shared.data[i].RemoveByReplaceLast(component, true);
			
			uint old = _entityComponentMap.GetKey((uint)elementCount);
			_entityComponentMap[old] = component;

			return true;
		}
	}

	public void Clear() {
		lock (_lock) {
			_entityComponentMap.Clear();
			
			int normalCount = normalComponentCount;
			int sharedCount = sharedComponentCount;
			for (int i = 0; i < normalCount; i++) _normal.data[i].Clear();
			for (int i = 0; i < sharedCount; i++) _shared.data[i].Clear();
			elementCount = 0;
		}
	}

	public bool Trim() {
		lock (_lock) {
			if (elementCount + 8 >= allocatedElementCount) return false;
			
			int normalCount = normalComponentCount;
			int sharedCount = sharedComponentCount;
			for (int i = 0; i < normalCount; i++) _normal.data[i].Trim();
			for (int i = 0; i < sharedCount; i++) _shared.data[i].Trim();
			
			return true;
		}
	}

	public unsafe void* GetNormalComponents(int typeIndex) { lock (_lock) return _normal.data[typeIndex].rawData; }
	public unsafe ushort* GetSharedComponentIndices(int typeIndex) { lock (_lock) return _shared.data[typeIndex].data; }
	public unsafe void* GetComponents(ComponentType t) {
		lock (_lock) {
			return t.data.kind == ComponentKind.normal ? GetNormalComponents(IndexOfNormalComponent(t)) : GetSharedComponentIndices(IndexOfSharedComponent(t));
		} 
	}

	public int GetElementSize(ComponentType t) { lock (_lock) return t.data.kind == ComponentKind.normal ? _normal.data[IndexOfNormalComponent(t)].sizeofElement : _shared.data[IndexOfSharedComponent(t)].sizeofElement; }
	
	public unsafe void* GetElement(ComponentType t, ComponentId index) {
		lock (_lock) {
			if (t.data.kind == ComponentKind.shared) return _shared.data[IndexOfSharedComponent(t)].data + index;
			EcsList n = _normal.data[IndexOfNormalComponent(t)];
			return (byte*)n.rawData + index * n.sizeofElement;
		}
	}

	public unsafe void CopyFrom(ComponentId current, ComponentId source, ArchetypeComponents srcArchetype) {
		lock (_lock) {
			int cur = current;
			int src = source;
			
			int normalCount = normalComponentCount;
			for (int i = 0; i < normalCount; i++) {
				int srcArrayId = srcArchetype.IndexOfNormalComponent(_normal.types[i]);
				if (srcArrayId != -1) _normal.data[i].CopyElementFrom(cur, srcArchetype._normal.data[srcArrayId].GetElementPtr(src));
				else _normal.data[i].Initialize(cur);
			}
			
			int sharedCount = sharedComponentCount;
			for (int i = 0; i < sharedCount; i++) {
				int srcArrayId = srcArchetype.IndexOfNormalComponent(_shared.types[i]);
				if (srcArrayId != -1) _shared.data[i].data[cur] = srcArchetype._shared.data[srcArrayId].data[src];
				else _shared.data[i].data[cur] = 0;
			}
		}
	}
	
	public unsafe void CopyFromAndDisposeOld(ComponentId current, ComponentId source, ArchetypeComponents srcArchetype) {
		lock (_lock) {
			int cur = current;
			int src = source;
			
			int normalCount = normalComponentCount;
			for (int i = 0; i < normalCount; i++) {
				int srcArrayId = srcArchetype.IndexOfNormalComponent(_normal.types[i]);
				if (srcArrayId != -1) _normal.data[i].CopyElementFrom(cur, srcArchetype._normal.data[srcArrayId].GetElementPtr(src));
				else _normal.data[i].Initialize(cur);
			}

			normalCount = srcArchetype.normalComponentCount;
			for (int i = 0; i < normalCount; i++) {
				int arrayId = IndexOfNormalComponent(srcArchetype._normal.types[i]);
				if (arrayId == -1) srcArchetype._normal.data[i].DisposeAt(src);
			}
			
			int sharedCount = sharedComponentCount;
			for (int i = 0; i < sharedCount; i++) {
				int srcArrayId = srcArchetype.IndexOfNormalComponent(_shared.types[i]);
				if (srcArrayId != -1) _shared.data[i].data[cur] = srcArchetype._shared.data[srcArrayId].data[src];
				else _shared.data[i].data[cur] = 0;
			}
		}
	}
}