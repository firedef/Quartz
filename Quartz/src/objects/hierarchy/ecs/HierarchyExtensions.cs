using Quartz.debug.log;
using Quartz.objects.ecs.delegates;
using Quartz.objects.ecs.entities;
using Quartz.objects.ecs.world;

namespace Quartz.objects.hierarchy.ecs; 

public static class HierarchyExtensions {
	public static unsafe int AddChild(this World world, EntityId parent, EntityId child) {
		HierarchyComponent* parentHierarchy = world.Comp<HierarchyComponent>(parent);
		HierarchyComponent* childHierarchy = world.Comp<HierarchyComponent>(child);

		if (parentHierarchy->hierarchyLevel == 255) {
			Log.Error($"can't set {child} as child of {parent}: reached max hierarchy level (255)");
			return -1;
		}
		
		childHierarchy->parent = parent;
		childHierarchy->hierarchyLevel = (byte)(parentHierarchy->hierarchyLevel + 1);
		if (!parentHierarchy->firstChild.isValid) {
			parentHierarchy->firstChild = child;
			return 0;
		}

		int index = 1;
		HierarchyComponent* siblingComponent = world.Comp<HierarchyComponent>(parentHierarchy->firstChild);
		while (siblingComponent->nextSibling.isValid) {
			index++;
			siblingComponent = world.Comp<HierarchyComponent>(siblingComponent->nextSibling);
		}

		siblingComponent->nextSibling = child;
		return index;
	}

	public static void AddChild(this EntityId parent, EntityId child) => parent.world.AddChild(parent, child);

	public static EntityId AddChild(this EntityId parent) {
		EntityId child = parent.world.CreateEntity();
		parent.AddChild(child);
		return child;
	}

	public static unsafe bool RemoveChild(this World world, EntityId parent, EntityId child) {
		HierarchyComponent* parentHierarchy = world.Comp<HierarchyComponent>(parent);
		HierarchyComponent* childHierarchy = world.Comp<HierarchyComponent>(child);
		
		if (!parentHierarchy->firstChild.isValid) return false;
		
		if (parentHierarchy->firstChild == child) {
			parentHierarchy->firstChild = EntityId.@null;
			childHierarchy->parent = EntityId.@null;
			childHierarchy->hierarchyLevel = 0;
			return true;
		}
		
		HierarchyComponent* siblingComponent = world.Comp<HierarchyComponent>(parentHierarchy->firstChild);
		while (siblingComponent->nextSibling != child && siblingComponent->nextSibling.isValid)
			siblingComponent = world.Comp<HierarchyComponent>(siblingComponent->nextSibling);

		if (!siblingComponent->nextSibling.isValid) return false;
		siblingComponent->nextSibling = childHierarchy->nextSibling;
		childHierarchy->parent = EntityId.@null;
		childHierarchy->hierarchyLevel = 0;
		return true;
	}

	public static unsafe EntityId GetParent(this World world, EntityId entity) {
		HierarchyComponent* ptr = world.TryComp<HierarchyComponent>(entity);
		return ptr == null ? EntityId.@null : ptr->parent;
	}
	
	public static unsafe int CountChilds(this World world, EntityId entity) {
		HierarchyComponent* ptr = world.TryComp<HierarchyComponent>(entity);
		if (ptr == null) return 0;
		if (!ptr->firstChild.isValid) return 0;
		return world.CountSiblings(ptr->nextSibling) + 1;
	}
	
	public static unsafe int CountSiblings(this World world, EntityId entity) {
		HierarchyComponent* ptr = world.TryComp<HierarchyComponent>(entity);
		if (ptr == null) return 0;
		int c = 0;
		while (ptr->nextSibling.isValid) {
			ptr = world.Comp<HierarchyComponent>(ptr->nextSibling);
			c++;
		}

		return c;
	}
	
	public static unsafe byte GetHierarchyLevel(this World world, EntityId entity) {
		HierarchyComponent* ptr = world.TryComp<HierarchyComponent>(entity);
		return ptr == null ? (byte)0 : ptr->hierarchyLevel;
	}

	public static unsafe void ForeachChild(this World world, EntityId entity, EcsDelegates.ComponentEntityDelegate<HierarchyComponent> a) {
		HierarchyComponent* ptr = world.TryComp<HierarchyComponent>(entity);
		if (ptr == null) return;
		if (!ptr->firstChild.isValid) return;
		entity = ptr->firstChild;
		ptr = world.Comp<HierarchyComponent>(entity);
		a(ptr, entity);
		ForeachSibling(world, entity, a);
	}
	
	public static unsafe void ForeachChild(this EntityId entity, EcsDelegates.ComponentEntityDelegate<HierarchyComponent> a) => entity.world.ForeachChild(entity, a);
	
	public static unsafe void ForeachSibling(this World world, EntityId entity, EcsDelegates.ComponentEntityDelegate<HierarchyComponent> a) {
		HierarchyComponent* ptr = world.TryComp<HierarchyComponent>(entity);
		if (ptr == null) return;

		while (ptr->nextSibling.isValid) {
			entity = ptr->nextSibling;
			ptr = world.Comp<HierarchyComponent>(entity);
			a(ptr, entity);
		}
	}
	
	public static unsafe void ForeachParent(this World world, EntityId entity, EcsDelegates.ComponentEntityDelegate<HierarchyComponent> a) {
		HierarchyComponent* ptr = world.TryComp<HierarchyComponent>(entity);
		if (ptr == null) return;

		while (ptr->parent.isValid) {
			entity = ptr->parent;
			ptr = world.Comp<HierarchyComponent>(entity);
			a(ptr, entity);
		}
	}

}