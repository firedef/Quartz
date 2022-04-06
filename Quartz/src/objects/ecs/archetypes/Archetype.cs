using Quartz.objects.ecs.components;
using Quartz.objects.ecs.entities;

namespace Quartz.objects.ecs.archetypes; 

public class Archetype {
#region fields

	private readonly object _currentLock = new();
	public readonly ComponentType[] componentTypes;
	public readonly EcsChunk components;
	public readonly ArchetypeRoot root;
	public readonly uint id;

#endregion fields

#region init

	public Archetype(ComponentType[] componentTypes, ArchetypeRoot root, uint id) {
		this.componentTypes = componentTypes;
		EcsList[] c = new EcsList[componentTypes.Length];
		for (int i = 0; i < componentTypes.Length; i++)
			c[i] = EcsListTypes.CreateList(componentTypes[i].type);

		components = new(c);
		this.root = root;
		this.id = id;
	}

#endregion init

#region components

	public void PreAllocate(int count) {
		lock (_currentLock) {
			int componentCount = componentTypes.Length;
			for (int i = 0; i < componentCount; i++)
				components.components[i].PreAllocate(count);
		}
	}

	public bool ContainsComponent(ComponentType t) => IndexOfComponent(t) != -1;
	public bool ContainsComponent(Type t) => IndexOfComponent(t.ToEcsComponent()) != -1;

	public int IndexOfComponent(ComponentType t) {
		lock (_currentLock) {
			int c = componentTypes.Length;
			for (int i = 0; i < c; i++)
				if (componentTypes[i].id == t.id)
					return i;
			return -1;
		}
	}
	
	public bool ContainsArchetype(ComponentType[] types) {
		lock (_currentLock) {
			int currentTypeCount = componentTypes.Length;
			int otherTypeCount = types.Length;

			if (currentTypeCount < otherTypeCount) return false;

			for (int other = 0; other < otherTypeCount; other++) {
				for (int cur = 0; cur < currentTypeCount; cur++)
					if (componentTypes[cur] == types[other]) goto FOUND;
				return false;
			FOUND: ;
			}
		
			return true;
		}
	}

	public unsafe void* GetComponent(ComponentType t, uint component) {
		lock (_currentLock) {
			int compIndex = IndexOfComponent(t);
			if (compIndex == -1) return null;
			long offset = component * components.components[compIndex].elementSize;
			return (byte*) components.components[compIndex].rawData + offset;
		}
	}
	
	public uint GetComponentIdFromEntity(EntityId entity) => components.entityComponentMap[entity.id];

	public unsafe void* GetComponent(int componentIndex, uint component) {
		lock (_currentLock) {
			long offset = component * components.components[componentIndex].elementSize;
			return (byte*) components.components[componentIndex].rawData + offset;
		}
	}
	
#endregion components
	
#region other

	public void Lock() => Monitor.Enter(_currentLock);
	public bool TryLock() => Monitor.TryEnter(_currentLock);
	public void Unlock() => Monitor.Exit(_currentLock);

#endregion other
}