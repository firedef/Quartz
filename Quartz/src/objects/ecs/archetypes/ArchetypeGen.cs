using Quartz.objects.ecs.components;
using Quartz.objects.ecs.delegates;
using Quartz.objects.ecs.filters;
// ReSharper disable CognitiveComplexity

namespace Quartz.objects.ecs.archetypes;

public partial class ArchetypeRoot {
#region generics1

#region normalForeach

	public void Foreach<T1>(EcsDelegates.ComponentDelegate<T1> a)
		where T1 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();

		int archCount = _archetypes.Count;

		// create stack for locked archetypes
		Stack<(Archetype arch, int i1)> lockedArchetypes = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;

			// if archetype is locked, execute later
			if (!arch.TryLock()) {
				lockedArchetypes.Push((arch, i1));
				continue;
			}
			arch.components.Foreach(a, i1);
			arch.Unlock();
		}

		// wait for locked archetypes unlock and execute
		foreach ((Archetype arch, int i1) in lockedArchetypes) {
			arch.Lock();
			arch.components.Foreach(a, i1);
			arch.Unlock();
		}
	}

	public void Foreach<TFilter, T1>(EcsDelegates.ComponentDelegate<T1> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();

		int archCount = _archetypes.Count;

		// create stack for locked archetypes
		Stack<(Archetype arch, int i1)> lockedArchetypes = new();
		TFilter filter = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;

			if (!filter.Filter(arch)) continue;

			// if archetype is locked, execute later
			if (!arch.TryLock()) {
				lockedArchetypes.Push((arch, i1));
				continue;
			}
			arch.components.Foreach(a, i1);
			arch.Unlock();
		}

		// wait for locked archetypes unlock and execute
		foreach ((Archetype arch, int i1) in lockedArchetypes) {
			arch.Lock();
			arch.components.Foreach(a, i1);
			arch.Unlock();
		}
	}

	public Task ForeachAsync<T1>(EcsDelegates.ComponentDelegate<T1> a)
		where T1 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();

		int archCount = _archetypes.Count;

		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.Foreach(a, i1);
				arch.Unlock();
			}));
		}

		return Task.WhenAll(tasks.ToArray());
	}

	public Task ForeachAsync<TFilter, T1>(EcsDelegates.ComponentDelegate<T1> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();
		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;

			if (!filter.Filter(arch)) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.Foreach(a, i1);
				arch.Unlock();
			}));
		}

		return Task.WhenAll(tasks.ToArray());
	}

#endregion normalForeach

#region batchedForeach

	public void ForeachBatched<T1>(EcsDelegates.ComponentDelegate<T1> batched, EcsDelegates.ComponentDelegate<T1> basic, int batchSize)
		where T1 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();

		int archCount = _archetypes.Count;


		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;

			arch.Lock();
			arch.components.ForeachBatched(batched, basic, batchSize, i1);
			arch.Unlock();
		}
	}

	public void ForeachBatched<TFilter, T1>(EcsDelegates.ComponentDelegate<T1> batched, EcsDelegates.ComponentDelegate<T1> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;

			if (!filter.Filter(arch)) continue;

			arch.Lock();
			arch.components.ForeachBatched(batched, basic, batchSize, i1);
			arch.Unlock();
		}
	}

	public Task ForeachAsyncBatched<T1>(EcsDelegates.ComponentDelegate<T1> batched, EcsDelegates.ComponentDelegate<T1> basic, int batchSize)
		where T1 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();

		int archCount = _archetypes.Count;

		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.ForeachBatched(batched, basic, batchSize, i1);
				arch.Unlock();
			}));
		}
		return Task.WhenAll(tasks.ToArray());
	}

	public Task ForeachAsyncBatched<TFilter, T1>(EcsDelegates.ComponentDelegate<T1> batched, EcsDelegates.ComponentDelegate<T1> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();
		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;

			if (!filter.Filter(arch)) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.ForeachBatched(batched, basic, batchSize, i1);
				arch.Unlock();
			}));
		}
		return Task.WhenAll(tasks.ToArray());
	}

#endregion batchedForeach

#endregion generics1

#region generics2

#region normalForeach

	public void Foreach<T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();

		int archCount = _archetypes.Count;

		// create stack for locked archetypes
		Stack<(Archetype arch, int i1, int i2)> lockedArchetypes = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;

			// if archetype is locked, execute later
			if (!arch.TryLock()) {
				lockedArchetypes.Push((arch, i1, i2));
				continue;
			}
			arch.components.Foreach(a, i1, i2);
			arch.Unlock();
		}

		// wait for locked archetypes unlock and execute
		foreach ((Archetype arch, int i1, int i2) in lockedArchetypes) {
			arch.Lock();
			arch.components.Foreach(a, i1, i2);
			arch.Unlock();
		}
	}

	public void Foreach<TFilter, T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();

		int archCount = _archetypes.Count;

		// create stack for locked archetypes
		Stack<(Archetype arch, int i1, int i2)> lockedArchetypes = new();
		TFilter filter = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;

			if (!filter.Filter(arch)) continue;

			// if archetype is locked, execute later
			if (!arch.TryLock()) {
				lockedArchetypes.Push((arch, i1, i2));
				continue;
			}
			arch.components.Foreach(a, i1, i2);
			arch.Unlock();
		}

		// wait for locked archetypes unlock and execute
		foreach ((Archetype arch, int i1, int i2) in lockedArchetypes) {
			arch.Lock();
			arch.components.Foreach(a, i1, i2);
			arch.Unlock();
		}
	}

	public Task ForeachAsync<T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();

		int archCount = _archetypes.Count;

		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.Foreach(a, i1, i2);
				arch.Unlock();
			}));
		}

		return Task.WhenAll(tasks.ToArray());
	}

	public Task ForeachAsync<TFilter, T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();
		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;

			if (!filter.Filter(arch)) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.Foreach(a, i1, i2);
				arch.Unlock();
			}));
		}

		return Task.WhenAll(tasks.ToArray());
	}

#endregion normalForeach

#region batchedForeach

	public void ForeachBatched<T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> batched, EcsDelegates.ComponentDelegate<T1, T2> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();

		int archCount = _archetypes.Count;


		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;

			arch.Lock();
			arch.components.ForeachBatched(batched, basic, batchSize, i1, i2);
			arch.Unlock();
		}
	}

	public void ForeachBatched<TFilter, T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> batched, EcsDelegates.ComponentDelegate<T1, T2> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;

			if (!filter.Filter(arch)) continue;

			arch.Lock();
			arch.components.ForeachBatched(batched, basic, batchSize, i1, i2);
			arch.Unlock();
		}
	}

	public Task ForeachAsyncBatched<T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> batched, EcsDelegates.ComponentDelegate<T1, T2> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();

		int archCount = _archetypes.Count;

		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.ForeachBatched(batched, basic, batchSize, i1, i2);
				arch.Unlock();
			}));
		}
		return Task.WhenAll(tasks.ToArray());
	}

	public Task ForeachAsyncBatched<TFilter, T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> batched, EcsDelegates.ComponentDelegate<T1, T2> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();
		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;

			if (!filter.Filter(arch)) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.ForeachBatched(batched, basic, batchSize, i1, i2);
				arch.Unlock();
			}));
		}
		return Task.WhenAll(tasks.ToArray());
	}

#endregion batchedForeach

#endregion generics2

#region generics3

#region normalForeach

	public void Foreach<T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();

		int archCount = _archetypes.Count;

		// create stack for locked archetypes
		Stack<(Archetype arch, int i1, int i2, int i3)> lockedArchetypes = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;

			// if archetype is locked, execute later
			if (!arch.TryLock()) {
				lockedArchetypes.Push((arch, i1, i2, i3));
				continue;
			}
			arch.components.Foreach(a, i1, i2, i3);
			arch.Unlock();
		}

		// wait for locked archetypes unlock and execute
		foreach ((Archetype arch, int i1, int i2, int i3) in lockedArchetypes) {
			arch.Lock();
			arch.components.Foreach(a, i1, i2, i3);
			arch.Unlock();
		}
	}

	public void Foreach<TFilter, T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();

		int archCount = _archetypes.Count;

		// create stack for locked archetypes
		Stack<(Archetype arch, int i1, int i2, int i3)> lockedArchetypes = new();
		TFilter filter = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;

			if (!filter.Filter(arch)) continue;

			// if archetype is locked, execute later
			if (!arch.TryLock()) {
				lockedArchetypes.Push((arch, i1, i2, i3));
				continue;
			}
			arch.components.Foreach(a, i1, i2, i3);
			arch.Unlock();
		}

		// wait for locked archetypes unlock and execute
		foreach ((Archetype arch, int i1, int i2, int i3) in lockedArchetypes) {
			arch.Lock();
			arch.components.Foreach(a, i1, i2, i3);
			arch.Unlock();
		}
	}

	public Task ForeachAsync<T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();

		int archCount = _archetypes.Count;

		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.Foreach(a, i1, i2, i3);
				arch.Unlock();
			}));
		}

		return Task.WhenAll(tasks.ToArray());
	}

	public Task ForeachAsync<TFilter, T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();
		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;

			if (!filter.Filter(arch)) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.Foreach(a, i1, i2, i3);
				arch.Unlock();
			}));
		}

		return Task.WhenAll(tasks.ToArray());
	}

#endregion normalForeach

#region batchedForeach

	public void ForeachBatched<T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> batched, EcsDelegates.ComponentDelegate<T1, T2, T3> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();

		int archCount = _archetypes.Count;


		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;

			arch.Lock();
			arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3);
			arch.Unlock();
		}
	}

	public void ForeachBatched<TFilter, T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> batched, EcsDelegates.ComponentDelegate<T1, T2, T3> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;

			if (!filter.Filter(arch)) continue;

			arch.Lock();
			arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3);
			arch.Unlock();
		}
	}

	public Task ForeachAsyncBatched<T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> batched, EcsDelegates.ComponentDelegate<T1, T2, T3> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();

		int archCount = _archetypes.Count;

		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3);
				arch.Unlock();
			}));
		}
		return Task.WhenAll(tasks.ToArray());
	}

	public Task ForeachAsyncBatched<TFilter, T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> batched, EcsDelegates.ComponentDelegate<T1, T2, T3> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();
		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;

			if (!filter.Filter(arch)) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3);
				arch.Unlock();
			}));
		}
		return Task.WhenAll(tasks.ToArray());
	}

#endregion batchedForeach

#endregion generics3

#region generics4

#region normalForeach

	public void Foreach<T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();

		int archCount = _archetypes.Count;

		// create stack for locked archetypes
		Stack<(Archetype arch, int i1, int i2, int i3, int i4)> lockedArchetypes = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;

			// if archetype is locked, execute later
			if (!arch.TryLock()) {
				lockedArchetypes.Push((arch, i1, i2, i3, i4));
				continue;
			}
			arch.components.Foreach(a, i1, i2, i3, i4);
			arch.Unlock();
		}

		// wait for locked archetypes unlock and execute
		foreach ((Archetype arch, int i1, int i2, int i3, int i4) in lockedArchetypes) {
			arch.Lock();
			arch.components.Foreach(a, i1, i2, i3, i4);
			arch.Unlock();
		}
	}

	public void Foreach<TFilter, T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();

		int archCount = _archetypes.Count;

		// create stack for locked archetypes
		Stack<(Archetype arch, int i1, int i2, int i3, int i4)> lockedArchetypes = new();
		TFilter filter = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;

			if (!filter.Filter(arch)) continue;

			// if archetype is locked, execute later
			if (!arch.TryLock()) {
				lockedArchetypes.Push((arch, i1, i2, i3, i4));
				continue;
			}
			arch.components.Foreach(a, i1, i2, i3, i4);
			arch.Unlock();
		}

		// wait for locked archetypes unlock and execute
		foreach ((Archetype arch, int i1, int i2, int i3, int i4) in lockedArchetypes) {
			arch.Lock();
			arch.components.Foreach(a, i1, i2, i3, i4);
			arch.Unlock();
		}
	}

	public Task ForeachAsync<T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();

		int archCount = _archetypes.Count;

		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.Foreach(a, i1, i2, i3, i4);
				arch.Unlock();
			}));
		}

		return Task.WhenAll(tasks.ToArray());
	}

	public Task ForeachAsync<TFilter, T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();
		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;

			if (!filter.Filter(arch)) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.Foreach(a, i1, i2, i3, i4);
				arch.Unlock();
			}));
		}

		return Task.WhenAll(tasks.ToArray());
	}

#endregion normalForeach

#region batchedForeach

	public void ForeachBatched<T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();

		int archCount = _archetypes.Count;


		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;

			arch.Lock();
			arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4);
			arch.Unlock();
		}
	}

	public void ForeachBatched<TFilter, T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;

			if (!filter.Filter(arch)) continue;

			arch.Lock();
			arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4);
			arch.Unlock();
		}
	}

	public Task ForeachAsyncBatched<T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();

		int archCount = _archetypes.Count;

		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4);
				arch.Unlock();
			}));
		}
		return Task.WhenAll(tasks.ToArray());
	}

	public Task ForeachAsyncBatched<TFilter, T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();
		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;

			if (!filter.Filter(arch)) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4);
				arch.Unlock();
			}));
		}
		return Task.WhenAll(tasks.ToArray());
	}

#endregion batchedForeach

#endregion generics4

#region generics5

#region normalForeach

	public void Foreach<T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();

		int archCount = _archetypes.Count;

		// create stack for locked archetypes
		Stack<(Archetype arch, int i1, int i2, int i3, int i4, int i5)> lockedArchetypes = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;

			// if archetype is locked, execute later
			if (!arch.TryLock()) {
				lockedArchetypes.Push((arch, i1, i2, i3, i4, i5));
				continue;
			}
			arch.components.Foreach(a, i1, i2, i3, i4, i5);
			arch.Unlock();
		}

		// wait for locked archetypes unlock and execute
		foreach ((Archetype arch, int i1, int i2, int i3, int i4, int i5) in lockedArchetypes) {
			arch.Lock();
			arch.components.Foreach(a, i1, i2, i3, i4, i5);
			arch.Unlock();
		}
	}

	public void Foreach<TFilter, T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();

		int archCount = _archetypes.Count;

		// create stack for locked archetypes
		Stack<(Archetype arch, int i1, int i2, int i3, int i4, int i5)> lockedArchetypes = new();
		TFilter filter = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;

			if (!filter.Filter(arch)) continue;

			// if archetype is locked, execute later
			if (!arch.TryLock()) {
				lockedArchetypes.Push((arch, i1, i2, i3, i4, i5));
				continue;
			}
			arch.components.Foreach(a, i1, i2, i3, i4, i5);
			arch.Unlock();
		}

		// wait for locked archetypes unlock and execute
		foreach ((Archetype arch, int i1, int i2, int i3, int i4, int i5) in lockedArchetypes) {
			arch.Lock();
			arch.components.Foreach(a, i1, i2, i3, i4, i5);
			arch.Unlock();
		}
	}

	public Task ForeachAsync<T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();

		int archCount = _archetypes.Count;

		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.Foreach(a, i1, i2, i3, i4, i5);
				arch.Unlock();
			}));
		}

		return Task.WhenAll(tasks.ToArray());
	}

	public Task ForeachAsync<TFilter, T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();
		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;

			if (!filter.Filter(arch)) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.Foreach(a, i1, i2, i3, i4, i5);
				arch.Unlock();
			}));
		}

		return Task.WhenAll(tasks.ToArray());
	}

#endregion normalForeach

#region batchedForeach

	public void ForeachBatched<T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();

		int archCount = _archetypes.Count;


		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;

			arch.Lock();
			arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5);
			arch.Unlock();
		}
	}

	public void ForeachBatched<TFilter, T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;

			if (!filter.Filter(arch)) continue;

			arch.Lock();
			arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5);
			arch.Unlock();
		}
	}

	public Task ForeachAsyncBatched<T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();

		int archCount = _archetypes.Count;

		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5);
				arch.Unlock();
			}));
		}
		return Task.WhenAll(tasks.ToArray());
	}

	public Task ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();
		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;

			if (!filter.Filter(arch)) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5);
				arch.Unlock();
			}));
		}
		return Task.WhenAll(tasks.ToArray());
	}

#endregion batchedForeach

#endregion generics5

#region generics6

#region normalForeach

	public void Foreach<T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();

		int archCount = _archetypes.Count;

		// create stack for locked archetypes
		Stack<(Archetype arch, int i1, int i2, int i3, int i4, int i5, int i6)> lockedArchetypes = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;

			// if archetype is locked, execute later
			if (!arch.TryLock()) {
				lockedArchetypes.Push((arch, i1, i2, i3, i4, i5, i6));
				continue;
			}
			arch.components.Foreach(a, i1, i2, i3, i4, i5, i6);
			arch.Unlock();
		}

		// wait for locked archetypes unlock and execute
		foreach ((Archetype arch, int i1, int i2, int i3, int i4, int i5, int i6) in lockedArchetypes) {
			arch.Lock();
			arch.components.Foreach(a, i1, i2, i3, i4, i5, i6);
			arch.Unlock();
		}
	}

	public void Foreach<TFilter, T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();

		int archCount = _archetypes.Count;

		// create stack for locked archetypes
		Stack<(Archetype arch, int i1, int i2, int i3, int i4, int i5, int i6)> lockedArchetypes = new();
		TFilter filter = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;

			if (!filter.Filter(arch)) continue;

			// if archetype is locked, execute later
			if (!arch.TryLock()) {
				lockedArchetypes.Push((arch, i1, i2, i3, i4, i5, i6));
				continue;
			}
			arch.components.Foreach(a, i1, i2, i3, i4, i5, i6);
			arch.Unlock();
		}

		// wait for locked archetypes unlock and execute
		foreach ((Archetype arch, int i1, int i2, int i3, int i4, int i5, int i6) in lockedArchetypes) {
			arch.Lock();
			arch.components.Foreach(a, i1, i2, i3, i4, i5, i6);
			arch.Unlock();
		}
	}

	public Task ForeachAsync<T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();

		int archCount = _archetypes.Count;

		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.Foreach(a, i1, i2, i3, i4, i5, i6);
				arch.Unlock();
			}));
		}

		return Task.WhenAll(tasks.ToArray());
	}

	public Task ForeachAsync<TFilter, T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();
		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;

			if (!filter.Filter(arch)) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.Foreach(a, i1, i2, i3, i4, i5, i6);
				arch.Unlock();
			}));
		}

		return Task.WhenAll(tasks.ToArray());
	}

#endregion normalForeach

#region batchedForeach

	public void ForeachBatched<T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();

		int archCount = _archetypes.Count;


		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;

			arch.Lock();
			arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6);
			arch.Unlock();
		}
	}

	public void ForeachBatched<TFilter, T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;

			if (!filter.Filter(arch)) continue;

			arch.Lock();
			arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6);
			arch.Unlock();
		}
	}

	public Task ForeachAsyncBatched<T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();

		int archCount = _archetypes.Count;

		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6);
				arch.Unlock();
			}));
		}
		return Task.WhenAll(tasks.ToArray());
	}

	public Task ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();
		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;

			if (!filter.Filter(arch)) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6);
				arch.Unlock();
			}));
		}
		return Task.WhenAll(tasks.ToArray());
	}

#endregion batchedForeach

#endregion generics6

#region generics7

#region normalForeach

	public void Foreach<T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();
		ComponentType t7 = typeof(T7).ToEcsComponent();

		int archCount = _archetypes.Count;

		// create stack for locked archetypes
		Stack<(Archetype arch, int i1, int i2, int i3, int i4, int i5, int i6, int i7)> lockedArchetypes = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;
			int i7 = arch.IndexOfComponent(t7);
			if (i7 == -1) continue;

			// if archetype is locked, execute later
			if (!arch.TryLock()) {
				lockedArchetypes.Push((arch, i1, i2, i3, i4, i5, i6, i7));
				continue;
			}
			arch.components.Foreach(a, i1, i2, i3, i4, i5, i6, i7);
			arch.Unlock();
		}

		// wait for locked archetypes unlock and execute
		foreach ((Archetype arch, int i1, int i2, int i3, int i4, int i5, int i6, int i7) in lockedArchetypes) {
			arch.Lock();
			arch.components.Foreach(a, i1, i2, i3, i4, i5, i6, i7);
			arch.Unlock();
		}
	}

	public void Foreach<TFilter, T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();
		ComponentType t7 = typeof(T7).ToEcsComponent();

		int archCount = _archetypes.Count;

		// create stack for locked archetypes
		Stack<(Archetype arch, int i1, int i2, int i3, int i4, int i5, int i6, int i7)> lockedArchetypes = new();
		TFilter filter = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;
			int i7 = arch.IndexOfComponent(t7);
			if (i7 == -1) continue;

			if (!filter.Filter(arch)) continue;

			// if archetype is locked, execute later
			if (!arch.TryLock()) {
				lockedArchetypes.Push((arch, i1, i2, i3, i4, i5, i6, i7));
				continue;
			}
			arch.components.Foreach(a, i1, i2, i3, i4, i5, i6, i7);
			arch.Unlock();
		}

		// wait for locked archetypes unlock and execute
		foreach ((Archetype arch, int i1, int i2, int i3, int i4, int i5, int i6, int i7) in lockedArchetypes) {
			arch.Lock();
			arch.components.Foreach(a, i1, i2, i3, i4, i5, i6, i7);
			arch.Unlock();
		}
	}

	public Task ForeachAsync<T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();
		ComponentType t7 = typeof(T7).ToEcsComponent();

		int archCount = _archetypes.Count;

		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;
			int i7 = arch.IndexOfComponent(t7);
			if (i7 == -1) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.Foreach(a, i1, i2, i3, i4, i5, i6, i7);
				arch.Unlock();
			}));
		}

		return Task.WhenAll(tasks.ToArray());
	}

	public Task ForeachAsync<TFilter, T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();
		ComponentType t7 = typeof(T7).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();
		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;
			int i7 = arch.IndexOfComponent(t7);
			if (i7 == -1) continue;

			if (!filter.Filter(arch)) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.Foreach(a, i1, i2, i3, i4, i5, i6, i7);
				arch.Unlock();
			}));
		}

		return Task.WhenAll(tasks.ToArray());
	}

#endregion normalForeach

#region batchedForeach

	public void ForeachBatched<T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();
		ComponentType t7 = typeof(T7).ToEcsComponent();

		int archCount = _archetypes.Count;


		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;
			int i7 = arch.IndexOfComponent(t7);
			if (i7 == -1) continue;

			arch.Lock();
			arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6, i7);
			arch.Unlock();
		}
	}

	public void ForeachBatched<TFilter, T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();
		ComponentType t7 = typeof(T7).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;
			int i7 = arch.IndexOfComponent(t7);
			if (i7 == -1) continue;

			if (!filter.Filter(arch)) continue;

			arch.Lock();
			arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6, i7);
			arch.Unlock();
		}
	}

	public Task ForeachAsyncBatched<T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();
		ComponentType t7 = typeof(T7).ToEcsComponent();

		int archCount = _archetypes.Count;

		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;
			int i7 = arch.IndexOfComponent(t7);
			if (i7 == -1) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6, i7);
				arch.Unlock();
			}));
		}
		return Task.WhenAll(tasks.ToArray());
	}

	public Task ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();
		ComponentType t7 = typeof(T7).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();
		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;
			int i7 = arch.IndexOfComponent(t7);
			if (i7 == -1) continue;

			if (!filter.Filter(arch)) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6, i7);
				arch.Unlock();
			}));
		}
		return Task.WhenAll(tasks.ToArray());
	}

#endregion batchedForeach

#endregion generics7

#region generics8

#region normalForeach

	public void Foreach<T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		where T8 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();
		ComponentType t7 = typeof(T7).ToEcsComponent();
		ComponentType t8 = typeof(T8).ToEcsComponent();

		int archCount = _archetypes.Count;

		// create stack for locked archetypes
		Stack<(Archetype arch, int i1, int i2, int i3, int i4, int i5, int i6, int i7, int i8)> lockedArchetypes = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;
			int i7 = arch.IndexOfComponent(t7);
			if (i7 == -1) continue;
			int i8 = arch.IndexOfComponent(t8);
			if (i8 == -1) continue;

			// if archetype is locked, execute later
			if (!arch.TryLock()) {
				lockedArchetypes.Push((arch, i1, i2, i3, i4, i5, i6, i7, i8));
				continue;
			}
			arch.components.Foreach(a, i1, i2, i3, i4, i5, i6, i7, i8);
			arch.Unlock();
		}

		// wait for locked archetypes unlock and execute
		foreach ((Archetype arch, int i1, int i2, int i3, int i4, int i5, int i6, int i7, int i8) in lockedArchetypes) {
			arch.Lock();
			arch.components.Foreach(a, i1, i2, i3, i4, i5, i6, i7, i8);
			arch.Unlock();
		}
	}

	public void Foreach<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		where T8 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();
		ComponentType t7 = typeof(T7).ToEcsComponent();
		ComponentType t8 = typeof(T8).ToEcsComponent();

		int archCount = _archetypes.Count;

		// create stack for locked archetypes
		Stack<(Archetype arch, int i1, int i2, int i3, int i4, int i5, int i6, int i7, int i8)> lockedArchetypes = new();
		TFilter filter = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;
			int i7 = arch.IndexOfComponent(t7);
			if (i7 == -1) continue;
			int i8 = arch.IndexOfComponent(t8);
			if (i8 == -1) continue;

			if (!filter.Filter(arch)) continue;

			// if archetype is locked, execute later
			if (!arch.TryLock()) {
				lockedArchetypes.Push((arch, i1, i2, i3, i4, i5, i6, i7, i8));
				continue;
			}
			arch.components.Foreach(a, i1, i2, i3, i4, i5, i6, i7, i8);
			arch.Unlock();
		}

		// wait for locked archetypes unlock and execute
		foreach ((Archetype arch, int i1, int i2, int i3, int i4, int i5, int i6, int i7, int i8) in lockedArchetypes) {
			arch.Lock();
			arch.components.Foreach(a, i1, i2, i3, i4, i5, i6, i7, i8);
			arch.Unlock();
		}
	}

	public Task ForeachAsync<T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		where T8 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();
		ComponentType t7 = typeof(T7).ToEcsComponent();
		ComponentType t8 = typeof(T8).ToEcsComponent();

		int archCount = _archetypes.Count;

		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;
			int i7 = arch.IndexOfComponent(t7);
			if (i7 == -1) continue;
			int i8 = arch.IndexOfComponent(t8);
			if (i8 == -1) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.Foreach(a, i1, i2, i3, i4, i5, i6, i7, i8);
				arch.Unlock();
			}));
		}

		return Task.WhenAll(tasks.ToArray());
	}

	public Task ForeachAsync<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		where T8 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();
		ComponentType t7 = typeof(T7).ToEcsComponent();
		ComponentType t8 = typeof(T8).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();
		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;
			int i7 = arch.IndexOfComponent(t7);
			if (i7 == -1) continue;
			int i8 = arch.IndexOfComponent(t8);
			if (i8 == -1) continue;

			if (!filter.Filter(arch)) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.Foreach(a, i1, i2, i3, i4, i5, i6, i7, i8);
				arch.Unlock();
			}));
		}

		return Task.WhenAll(tasks.ToArray());
	}

#endregion normalForeach

#region batchedForeach

	public void ForeachBatched<T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		where T8 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();
		ComponentType t7 = typeof(T7).ToEcsComponent();
		ComponentType t8 = typeof(T8).ToEcsComponent();

		int archCount = _archetypes.Count;


		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;
			int i7 = arch.IndexOfComponent(t7);
			if (i7 == -1) continue;
			int i8 = arch.IndexOfComponent(t8);
			if (i8 == -1) continue;

			arch.Lock();
			arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6, i7, i8);
			arch.Unlock();
		}
	}

	public void ForeachBatched<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		where T8 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();
		ComponentType t7 = typeof(T7).ToEcsComponent();
		ComponentType t8 = typeof(T8).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;
			int i7 = arch.IndexOfComponent(t7);
			if (i7 == -1) continue;
			int i8 = arch.IndexOfComponent(t8);
			if (i8 == -1) continue;

			if (!filter.Filter(arch)) continue;

			arch.Lock();
			arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6, i7, i8);
			arch.Unlock();
		}
	}

	public Task ForeachAsyncBatched<T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		where T8 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();
		ComponentType t7 = typeof(T7).ToEcsComponent();
		ComponentType t8 = typeof(T8).ToEcsComponent();

		int archCount = _archetypes.Count;

		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;
			int i7 = arch.IndexOfComponent(t7);
			if (i7 == -1) continue;
			int i8 = arch.IndexOfComponent(t8);
			if (i8 == -1) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6, i7, i8);
				arch.Unlock();
			}));
		}
		return Task.WhenAll(tasks.ToArray());
	}

	public Task ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		where T8 : unmanaged, IComponent {

		// convert generics to component types
		ComponentType t1 = typeof(T1).ToEcsComponent();
		ComponentType t2 = typeof(T2).ToEcsComponent();
		ComponentType t3 = typeof(T3).ToEcsComponent();
		ComponentType t4 = typeof(T4).ToEcsComponent();
		ComponentType t5 = typeof(T5).ToEcsComponent();
		ComponentType t6 = typeof(T6).ToEcsComponent();
		ComponentType t7 = typeof(T7).ToEcsComponent();
		ComponentType t8 = typeof(T8).ToEcsComponent();

		int archCount = _archetypes.Count;

		TFilter filter = new();
		List<Task> tasks = new(archCount);

		// iterate over all archetypes
		for (int i = 0; i < archCount; i++) {
			Archetype arch = _archetypes[i];

			// get indices of required components, and exclude archetypes without them
			int i1 = arch.IndexOfComponent(t1);
			if (i1 == -1) continue;
			int i2 = arch.IndexOfComponent(t2);
			if (i2 == -1) continue;
			int i3 = arch.IndexOfComponent(t3);
			if (i3 == -1) continue;
			int i4 = arch.IndexOfComponent(t4);
			if (i4 == -1) continue;
			int i5 = arch.IndexOfComponent(t5);
			if (i5 == -1) continue;
			int i6 = arch.IndexOfComponent(t6);
			if (i6 == -1) continue;
			int i7 = arch.IndexOfComponent(t7);
			if (i7 == -1) continue;
			int i8 = arch.IndexOfComponent(t8);
			if (i8 == -1) continue;

			if (!filter.Filter(arch)) continue;

			tasks.Add(Task.Run(() => {
				arch.Lock();
				arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6, i7, i8);
				arch.Unlock();
			}));
		}
		return Task.WhenAll(tasks.ToArray());
	}

#endregion batchedForeach

#endregion generics8
}