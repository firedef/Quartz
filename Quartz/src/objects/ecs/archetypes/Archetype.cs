namespace Quartz.objects.ecs.archetypes; 

public class Archetype {
#region fields

	public readonly Type[] componentTypes;
	public readonly EcsChunk components;
	public readonly ArchetypeRoot root;
	public readonly uint id;

#endregion fields

#region init

	public Archetype(Type[] componentTypes, ArchetypeRoot root, uint id) {
		this.componentTypes = componentTypes;
		EcsList[] c = new EcsList[componentTypes.Length];
		for (int i = 0; i < componentTypes.Length; i++)
			c[i] = EcsListTypes.CreateList(componentTypes[i]);

		components = new(c);
		this.root = root;
		this.id = id;
	}

#endregion init

#region components

	public bool ContainsComponent(Type t) => IndexOfComponent(t) != -1;

	public int IndexOfComponent(Type t) {
		int c = componentTypes.Length;
		for (int i = 0; i < c; i++)
			if (componentTypes[i] == t)
				return i;
		return -1;
	}
	
	public bool ContainsArchetype(Type[] types) {
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

	public unsafe void* GetComponent(Type t, uint component) {
		int compIndex = IndexOfComponent(t);
		if (compIndex == -1) return null;
		long offset = component * components.components[compIndex].elementSize;
		return (byte*) components.components[compIndex].rawData + offset;
	} 
	
#endregion components
}