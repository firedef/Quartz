using Quartz.core.collections;
using Quartz.objects.ecs.components;
using Quartz.objects.ecs.delegates;
using Quartz.objects.ecs.entities;
using Quartz.objects.ecs.filters;

namespace Quartz.objects.ecs.archetypes; 

public partial class ArchetypeRoot {
#region normal

    public void Foreach<T1>(EcsDelegates.ComponentDelegate<T1> a, bool useLock = false)
        where T1 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;

            if (useLock) arch.Lock();
            arch.components.Foreach(a, i1);
            if (useLock) arch.Unlock();
        }
    }

    public void Foreach<TFilter, T1>(EcsDelegates.ComponentDelegate<T1> a, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;

            if (!filter.Filter(arch)) continue;

            if (useLock) arch.Lock();
            arch.components.Foreach(a, i1);
            if (useLock) arch.Unlock();
        }
    }

    public Task ForeachAsync<T1>(EcsDelegates.ComponentDelegate<T1> a, bool useLock = false)
        where T1 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;

            tasks.Add(Task.Run(() => {
                if (useLock) arch.Lock();
                arch.components.Foreach(a, i1);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

    public Task ForeachAsync<TFilter, T1>(EcsDelegates.ComponentDelegate<T1> a, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;

            if (!filter.Filter(arch)) continue;

            tasks.Add(Task.Run(() => {
                if (useLock) arch.Lock();
                arch.components.Foreach(a, i1);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

#endregion normal

#region batched

    public void ForeachBatched<T1>(EcsDelegates.ComponentDelegate<T1> batched, EcsDelegates.ComponentDelegate<T1> basic, int batchSize, bool useLock = false)
        where T1 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;

            if (useLock) arch.Lock();
            arch.components.ForeachBatched(batched, basic, batchSize, i1);
            if (useLock) arch.Unlock();
        }
    }

    public void ForeachBatched<TFilter, T1>(EcsDelegates.ComponentDelegate<T1> batched, EcsDelegates.ComponentDelegate<T1> basic, int batchSize, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;

            if (!filter.Filter(arch)) continue;

            if (useLock) arch.Lock();
            arch.components.ForeachBatched(batched, basic, batchSize, i1);
            if (useLock) arch.Unlock();
        }
    }

    public Task ForeachAsyncBatched<T1>(EcsDelegates.ComponentDelegate<T1> batched, EcsDelegates.ComponentDelegate<T1> basic, int batchSize, bool useLock = false)
        where T1 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;

            tasks.Add(Task.Run(() => {
                if (useLock) arch.Lock();
                arch.components.ForeachBatched(batched, basic, batchSize, i1);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

    public Task ForeachAsyncBatched<TFilter, T1>(EcsDelegates.ComponentDelegate<T1> batched, EcsDelegates.ComponentDelegate<T1> basic, int batchSize, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;

            if (!filter.Filter(arch)) continue;

            tasks.Add(Task.Run(() => {
                if (useLock) arch.Lock();
                arch.components.ForeachBatched(batched, basic, batchSize, i1);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

#endregion batched

#region normal

    public void Foreach<T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> a, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;
            int i2 = arch.IndexOfComponent(t2);
            if (i2 == -1) continue;

            if (useLock) arch.Lock();
            arch.components.Foreach(a, i1, i2);
            if (useLock) arch.Unlock();
        }
    }

    public void Foreach<TFilter, T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> a, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;
            int i2 = arch.IndexOfComponent(t2);
            if (i2 == -1) continue;

            if (!filter.Filter(arch)) continue;

            if (useLock) arch.Lock();
            arch.components.Foreach(a, i1, i2);
            if (useLock) arch.Unlock();
        }
    }

    public Task ForeachAsync<T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> a, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;
            int i2 = arch.IndexOfComponent(t2);
            if (i2 == -1) continue;

            tasks.Add(Task.Run(() => {
                if (useLock) arch.Lock();
                arch.components.Foreach(a, i1, i2);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

    public Task ForeachAsync<TFilter, T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> a, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;
            int i2 = arch.IndexOfComponent(t2);
            if (i2 == -1) continue;

            if (!filter.Filter(arch)) continue;

            tasks.Add(Task.Run(() => {
                if (useLock) arch.Lock();
                arch.components.Foreach(a, i1, i2);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

#endregion normal

#region batched

    public void ForeachBatched<T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> batched, EcsDelegates.ComponentDelegate<T1, T2> basic, int batchSize, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;
            int i2 = arch.IndexOfComponent(t2);
            if (i2 == -1) continue;

            if (useLock) arch.Lock();
            arch.components.ForeachBatched(batched, basic, batchSize, i1, i2);
            if (useLock) arch.Unlock();
        }
    }

    public void ForeachBatched<TFilter, T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> batched, EcsDelegates.ComponentDelegate<T1, T2> basic, int batchSize, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;
            int i2 = arch.IndexOfComponent(t2);
            if (i2 == -1) continue;

            if (!filter.Filter(arch)) continue;

            if (useLock) arch.Lock();
            arch.components.ForeachBatched(batched, basic, batchSize, i1, i2);
            if (useLock) arch.Unlock();
        }
    }

    public Task ForeachAsyncBatched<T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> batched, EcsDelegates.ComponentDelegate<T1, T2> basic, int batchSize, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;
            int i2 = arch.IndexOfComponent(t2);
            if (i2 == -1) continue;

            tasks.Add(Task.Run(() => {
                if (useLock) arch.Lock();
                arch.components.ForeachBatched(batched, basic, batchSize, i1, i2);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

    public Task ForeachAsyncBatched<TFilter, T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> batched, EcsDelegates.ComponentDelegate<T1, T2> basic, int batchSize, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;
            int i2 = arch.IndexOfComponent(t2);
            if (i2 == -1) continue;

            if (!filter.Filter(arch)) continue;

            tasks.Add(Task.Run(() => {
                if (useLock) arch.Lock();
                arch.components.ForeachBatched(batched, basic, batchSize, i1, i2);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

#endregion batched

#region normal

    public void Foreach<T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> a, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;
            int i2 = arch.IndexOfComponent(t2);
            if (i2 == -1) continue;
            int i3 = arch.IndexOfComponent(t3);
            if (i3 == -1) continue;

            if (useLock) arch.Lock();
            arch.components.Foreach(a, i1, i2, i3);
            if (useLock) arch.Unlock();
        }
    }

    public void Foreach<TFilter, T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> a, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;
            int i2 = arch.IndexOfComponent(t2);
            if (i2 == -1) continue;
            int i3 = arch.IndexOfComponent(t3);
            if (i3 == -1) continue;

            if (!filter.Filter(arch)) continue;

            if (useLock) arch.Lock();
            arch.components.Foreach(a, i1, i2, i3);
            if (useLock) arch.Unlock();
        }
    }

    public Task ForeachAsync<T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> a, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;
            int i2 = arch.IndexOfComponent(t2);
            if (i2 == -1) continue;
            int i3 = arch.IndexOfComponent(t3);
            if (i3 == -1) continue;

            tasks.Add(Task.Run(() => {
                if (useLock) arch.Lock();
                arch.components.Foreach(a, i1, i2, i3);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

    public Task ForeachAsync<TFilter, T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> a, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;
            int i2 = arch.IndexOfComponent(t2);
            if (i2 == -1) continue;
            int i3 = arch.IndexOfComponent(t3);
            if (i3 == -1) continue;

            if (!filter.Filter(arch)) continue;

            tasks.Add(Task.Run(() => {
                if (useLock) arch.Lock();
                arch.components.Foreach(a, i1, i2, i3);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

#endregion normal

#region batched

    public void ForeachBatched<T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> batched, EcsDelegates.ComponentDelegate<T1, T2, T3> basic, int batchSize, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;
            int i2 = arch.IndexOfComponent(t2);
            if (i2 == -1) continue;
            int i3 = arch.IndexOfComponent(t3);
            if (i3 == -1) continue;

            if (useLock) arch.Lock();
            arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3);
            if (useLock) arch.Unlock();
        }
    }

    public void ForeachBatched<TFilter, T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> batched, EcsDelegates.ComponentDelegate<T1, T2, T3> basic, int batchSize, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;
            int i2 = arch.IndexOfComponent(t2);
            if (i2 == -1) continue;
            int i3 = arch.IndexOfComponent(t3);
            if (i3 == -1) continue;

            if (!filter.Filter(arch)) continue;

            if (useLock) arch.Lock();
            arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3);
            if (useLock) arch.Unlock();
        }
    }

    public Task ForeachAsyncBatched<T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> batched, EcsDelegates.ComponentDelegate<T1, T2, T3> basic, int batchSize, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;
            int i2 = arch.IndexOfComponent(t2);
            if (i2 == -1) continue;
            int i3 = arch.IndexOfComponent(t3);
            if (i3 == -1) continue;

            tasks.Add(Task.Run(() => {
                if (useLock) arch.Lock();
                arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

    public Task ForeachAsyncBatched<TFilter, T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> batched, EcsDelegates.ComponentDelegate<T1, T2, T3> basic, int batchSize, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;
            int i2 = arch.IndexOfComponent(t2);
            if (i2 == -1) continue;
            int i3 = arch.IndexOfComponent(t3);
            if (i3 == -1) continue;

            if (!filter.Filter(arch)) continue;

            tasks.Add(Task.Run(() => {
                if (useLock) arch.Lock();
                arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

#endregion batched

#region normal

    public void Foreach<T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> a, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;
            int i2 = arch.IndexOfComponent(t2);
            if (i2 == -1) continue;
            int i3 = arch.IndexOfComponent(t3);
            if (i3 == -1) continue;
            int i4 = arch.IndexOfComponent(t4);
            if (i4 == -1) continue;

            if (useLock) arch.Lock();
            arch.components.Foreach(a, i1, i2, i3, i4);
            if (useLock) arch.Unlock();
        }
    }

    public void Foreach<TFilter, T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> a, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;
            int i2 = arch.IndexOfComponent(t2);
            if (i2 == -1) continue;
            int i3 = arch.IndexOfComponent(t3);
            if (i3 == -1) continue;
            int i4 = arch.IndexOfComponent(t4);
            if (i4 == -1) continue;

            if (!filter.Filter(arch)) continue;

            if (useLock) arch.Lock();
            arch.components.Foreach(a, i1, i2, i3, i4);
            if (useLock) arch.Unlock();
        }
    }

    public Task ForeachAsync<T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> a, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;
            int i2 = arch.IndexOfComponent(t2);
            if (i2 == -1) continue;
            int i3 = arch.IndexOfComponent(t3);
            if (i3 == -1) continue;
            int i4 = arch.IndexOfComponent(t4);
            if (i4 == -1) continue;

            tasks.Add(Task.Run(() => {
                if (useLock) arch.Lock();
                arch.components.Foreach(a, i1, i2, i3, i4);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

    public Task ForeachAsync<TFilter, T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> a, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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
                if (useLock) arch.Lock();
                arch.components.Foreach(a, i1, i2, i3, i4);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

#endregion normal

#region batched

    public void ForeachBatched<T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4> basic, int batchSize, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;
            int i2 = arch.IndexOfComponent(t2);
            if (i2 == -1) continue;
            int i3 = arch.IndexOfComponent(t3);
            if (i3 == -1) continue;
            int i4 = arch.IndexOfComponent(t4);
            if (i4 == -1) continue;

            if (useLock) arch.Lock();
            arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4);
            if (useLock) arch.Unlock();
        }
    }

    public void ForeachBatched<TFilter, T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4> basic, int batchSize, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;
            int i2 = arch.IndexOfComponent(t2);
            if (i2 == -1) continue;
            int i3 = arch.IndexOfComponent(t3);
            if (i3 == -1) continue;
            int i4 = arch.IndexOfComponent(t4);
            if (i4 == -1) continue;

            if (!filter.Filter(arch)) continue;

            if (useLock) arch.Lock();
            arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4);
            if (useLock) arch.Unlock();
        }
    }

    public Task ForeachAsyncBatched<T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4> basic, int batchSize, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

            int i1 = arch.IndexOfComponent(t1);
            if (i1 == -1) continue;
            int i2 = arch.IndexOfComponent(t2);
            if (i2 == -1) continue;
            int i3 = arch.IndexOfComponent(t3);
            if (i3 == -1) continue;
            int i4 = arch.IndexOfComponent(t4);
            if (i4 == -1) continue;

            tasks.Add(Task.Run(() => {
                if (useLock) arch.Lock();
                arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

    public Task ForeachAsyncBatched<TFilter, T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4> basic, int batchSize, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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
                if (useLock) arch.Lock();
                arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

#endregion batched

#region normal

    public void Foreach<T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> a, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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

            if (useLock) arch.Lock();
            arch.components.Foreach(a, i1, i2, i3, i4, i5);
            if (useLock) arch.Unlock();
        }
    }

    public void Foreach<TFilter, T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> a, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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

            if (useLock) arch.Lock();
            arch.components.Foreach(a, i1, i2, i3, i4, i5);
            if (useLock) arch.Unlock();
        }
    }

    public Task ForeachAsync<T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> a, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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
                if (useLock) arch.Lock();
                arch.components.Foreach(a, i1, i2, i3, i4, i5);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

    public Task ForeachAsync<TFilter, T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> a, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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
                if (useLock) arch.Lock();
                arch.components.Foreach(a, i1, i2, i3, i4, i5);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

#endregion normal

#region batched

    public void ForeachBatched<T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> basic, int batchSize, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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

            if (useLock) arch.Lock();
            arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5);
            if (useLock) arch.Unlock();
        }
    }

    public void ForeachBatched<TFilter, T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> basic, int batchSize, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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

            if (useLock) arch.Lock();
            arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5);
            if (useLock) arch.Unlock();
        }
    }

    public Task ForeachAsyncBatched<T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> basic, int batchSize, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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
                if (useLock) arch.Lock();
                arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

    public Task ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> basic, int batchSize, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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
                if (useLock) arch.Lock();
                arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

#endregion batched

#region normal

    public void Foreach<T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> a, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();
        ComponentType t6 = typeof(T6).ToEcsComponent();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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

            if (useLock) arch.Lock();
            arch.components.Foreach(a, i1, i2, i3, i4, i5, i6);
            if (useLock) arch.Unlock();
        }
    }

    public void Foreach<TFilter, T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> a, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();
        ComponentType t6 = typeof(T6).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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

            if (useLock) arch.Lock();
            arch.components.Foreach(a, i1, i2, i3, i4, i5, i6);
            if (useLock) arch.Unlock();
        }
    }

    public Task ForeachAsync<T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> a, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();
        ComponentType t6 = typeof(T6).ToEcsComponent();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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
                if (useLock) arch.Lock();
                arch.components.Foreach(a, i1, i2, i3, i4, i5, i6);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

    public Task ForeachAsync<TFilter, T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> a, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();
        ComponentType t6 = typeof(T6).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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
                if (useLock) arch.Lock();
                arch.components.Foreach(a, i1, i2, i3, i4, i5, i6);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

#endregion normal

#region batched

    public void ForeachBatched<T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> basic, int batchSize, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();
        ComponentType t6 = typeof(T6).ToEcsComponent();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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

            if (useLock) arch.Lock();
            arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6);
            if (useLock) arch.Unlock();
        }
    }

    public void ForeachBatched<TFilter, T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> basic, int batchSize, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();
        ComponentType t6 = typeof(T6).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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

            if (useLock) arch.Lock();
            arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6);
            if (useLock) arch.Unlock();
        }
    }

    public Task ForeachAsyncBatched<T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> basic, int batchSize, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();
        ComponentType t6 = typeof(T6).ToEcsComponent();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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
                if (useLock) arch.Lock();
                arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

    public Task ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> basic, int batchSize, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();
        ComponentType t6 = typeof(T6).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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
                if (useLock) arch.Lock();
                arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

#endregion batched

#region normal

    public void Foreach<T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> a, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();
        ComponentType t6 = typeof(T6).ToEcsComponent();
        ComponentType t7 = typeof(T7).ToEcsComponent();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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

            if (useLock) arch.Lock();
            arch.components.Foreach(a, i1, i2, i3, i4, i5, i6, i7);
            if (useLock) arch.Unlock();
        }
    }

    public void Foreach<TFilter, T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> a, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();
        ComponentType t6 = typeof(T6).ToEcsComponent();
        ComponentType t7 = typeof(T7).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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

            if (useLock) arch.Lock();
            arch.components.Foreach(a, i1, i2, i3, i4, i5, i6, i7);
            if (useLock) arch.Unlock();
        }
    }

    public Task ForeachAsync<T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> a, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();
        ComponentType t6 = typeof(T6).ToEcsComponent();
        ComponentType t7 = typeof(T7).ToEcsComponent();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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
                if (useLock) arch.Lock();
                arch.components.Foreach(a, i1, i2, i3, i4, i5, i6, i7);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

    public Task ForeachAsync<TFilter, T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> a, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();
        ComponentType t6 = typeof(T6).ToEcsComponent();
        ComponentType t7 = typeof(T7).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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
                if (useLock) arch.Lock();
                arch.components.Foreach(a, i1, i2, i3, i4, i5, i6, i7);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

#endregion normal

#region batched

    public void ForeachBatched<T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> basic, int batchSize, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();
        ComponentType t6 = typeof(T6).ToEcsComponent();
        ComponentType t7 = typeof(T7).ToEcsComponent();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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

            if (useLock) arch.Lock();
            arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6, i7);
            if (useLock) arch.Unlock();
        }
    }

    public void ForeachBatched<TFilter, T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> basic, int batchSize, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();
        ComponentType t6 = typeof(T6).ToEcsComponent();
        ComponentType t7 = typeof(T7).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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

            if (useLock) arch.Lock();
            arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6, i7);
            if (useLock) arch.Unlock();
        }
    }

    public Task ForeachAsyncBatched<T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> basic, int batchSize, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();
        ComponentType t6 = typeof(T6).ToEcsComponent();
        ComponentType t7 = typeof(T7).ToEcsComponent();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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
                if (useLock) arch.Lock();
                arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6, i7);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

    public Task ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> basic, int batchSize, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();
        ComponentType t6 = typeof(T6).ToEcsComponent();
        ComponentType t7 = typeof(T7).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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
                if (useLock) arch.Lock();
                arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6, i7);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

#endregion batched

#region normal

    public void Foreach<T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> a, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        where T8 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();
        ComponentType t6 = typeof(T6).ToEcsComponent();
        ComponentType t7 = typeof(T7).ToEcsComponent();
        ComponentType t8 = typeof(T8).ToEcsComponent();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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

            if (useLock) arch.Lock();
            arch.components.Foreach(a, i1, i2, i3, i4, i5, i6, i7, i8);
            if (useLock) arch.Unlock();
        }
    }

    public void Foreach<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> a, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        where T8 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();
        ComponentType t6 = typeof(T6).ToEcsComponent();
        ComponentType t7 = typeof(T7).ToEcsComponent();
        ComponentType t8 = typeof(T8).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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

            if (useLock) arch.Lock();
            arch.components.Foreach(a, i1, i2, i3, i4, i5, i6, i7, i8);
            if (useLock) arch.Unlock();
        }
    }

    public Task ForeachAsync<T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> a, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        where T8 : unmanaged, IComponent {
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
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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
                if (useLock) arch.Lock();
                arch.components.Foreach(a, i1, i2, i3, i4, i5, i6, i7, i8);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

    public Task ForeachAsync<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> a, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        where T8 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();
        ComponentType t6 = typeof(T6).ToEcsComponent();
        ComponentType t7 = typeof(T7).ToEcsComponent();
        ComponentType t8 = typeof(T8).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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
                if (useLock) arch.Lock();
                arch.components.Foreach(a, i1, i2, i3, i4, i5, i6, i7, i8);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

#endregion normal

#region batched

    public void ForeachBatched<T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> basic, int batchSize, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        where T8 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();
        ComponentType t6 = typeof(T6).ToEcsComponent();
        ComponentType t7 = typeof(T7).ToEcsComponent();
        ComponentType t8 = typeof(T8).ToEcsComponent();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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

            if (useLock) arch.Lock();
            arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6, i7, i8);
            if (useLock) arch.Unlock();
        }
    }

    public void ForeachBatched<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> basic, int batchSize, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        where T8 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();
        ComponentType t6 = typeof(T6).ToEcsComponent();
        ComponentType t7 = typeof(T7).ToEcsComponent();
        ComponentType t8 = typeof(T8).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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

            if (useLock) arch.Lock();
            arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6, i7, i8);
            if (useLock) arch.Unlock();
        }
    }

    public Task ForeachAsyncBatched<T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> basic, int batchSize, bool useLock = false)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        where T8 : unmanaged, IComponent {
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
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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
                if (useLock) arch.Lock();
                arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6, i7, i8);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

    public Task ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> basic, int batchSize, bool useLock = false)
        where TFilter : IEcsFilter, new()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        where T8 : unmanaged, IComponent {
        ComponentType t1 = typeof(T1).ToEcsComponent();
        ComponentType t2 = typeof(T2).ToEcsComponent();
        ComponentType t3 = typeof(T3).ToEcsComponent();
        ComponentType t4 = typeof(T4).ToEcsComponent();
        ComponentType t5 = typeof(T5).ToEcsComponent();
        ComponentType t6 = typeof(T6).ToEcsComponent();
        ComponentType t7 = typeof(T7).ToEcsComponent();
        ComponentType t8 = typeof(T8).ToEcsComponent();

        TFilter filter = new();

        int archCount = _archetypes.Count;
        List<Task> tasks = new(archCount);
        for (int i = 0; i < archCount; i++) {
            Archetype arch = _archetypes[i];

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
                if (useLock) arch.Lock();
                arch.components.ForeachBatched(batched, basic, batchSize, i1, i2, i3, i4, i5, i6, i7, i8);
                if (useLock) arch.Unlock();
            }));
        }

        return Task.WhenAll(tasks.ToArray());
    }

#endregion batched

}