using Quartz.objects.ecs.archetypes;
using Quartz.objects.ecs.components;
using Quartz.objects.ecs.delegates;
using Quartz.objects.ecs.entities;
using Quartz.objects.ecs.filters;
using Quartz.objects.ecs.components.attributes;

namespace Quartz.objects.ecs.world; 

public partial class World {
	/// <summary>
	/// get or create new <see cref="Archetype"/> with exact match of components <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <typeparam name="T1">component #1</typeparam>
    public Archetype GetArchetype<T1>()
        where T1 : unmanaged, IComponent
        => GetArchetype(typeof(T1));

	/// <summary>
	/// create new <see cref="Entity"/> with exact match of components <br/><br/>
	/// you can add/remove components later, but with small performance hit <br/><br/>
	/// if you want to add multiple entities, use <see cref="CreateEntities{T1}"/> or <see cref="CreateEntity(Archetype)"/> with archetype from <see cref="GetArchetype{T1}"/>  <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <typeparam name="T1">component #1</typeparam>
    public EntityId CreateEntity<T1>()
        where T1 : unmanaged, IComponent
        => CreateEntity(GetArchetype<T1>());

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	/// if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1}"/> <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <typeparam name="T1">component #1</typeparam>
    public void CreateEntities<T1>(int count)
        where T1 : unmanaged, IComponent
        => CreateEntities(count, GetArchetype<T1>(), _=>{});

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	/// if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1}"/> <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity</param>
	/// <typeparam name="T1">component #1</typeparam>
    public void CreateEntitiesForeach<T1>(int count, Action<EntityId> onEntityCreation)
        where T1 : unmanaged, IComponent
        => CreateEntities(count, GetArchetype<T1>(), onEntityCreation);

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components, and initialize components <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity, used for component initialization</param>
	/// <typeparam name="T1">component #1</typeparam>
    public unsafe void CreateEntitiesForeachComp<T1>(int count, EcsDelegates.ComponentDelegate<T1> onEntityCreation)
        where T1 : unmanaged, IComponent {
        Archetype archetype = GetArchetype<T1>();
        archetype.PreAllocate(count);
       
        T1* c1 = (T1*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T1).ToEcsComponent()), 0);
        entities.AddMultiple(count, i => {
			EntityId ent = i;
			archetypes.AddEntity(ent, archetype);
			uint offset = archetype.GetComponentIdFromEntity(ent);
			onEntityCreation(c1 + offset);
		});
    }

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    public void Foreach<T1>(EcsDelegates.ComponentDelegate<T1> a, bool useLock = false)
		where T1 : unmanaged, IComponent
		=> archetypes.Foreach<T1>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    public void ForeachBatched<T1>(EcsDelegates.ComponentDelegate<T1> batched, EcsDelegates.ComponentDelegate<T1> basic, int batchSize, bool useLock = false)
		where T1 : unmanaged, IComponent
		=> archetypes.ForeachBatched<T1>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    public void Foreach<TFilter, T1>(EcsDelegates.ComponentDelegate<T1> a, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		=> archetypes.Foreach<TFilter, T1>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    public void ForeachBatched<TFilter, T1>(EcsDelegates.ComponentDelegate<T1> batched, EcsDelegates.ComponentDelegate<T1> basic, int batchSize, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		=> archetypes.ForeachBatched<TFilter, T1>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    public Task ForeachAsync<T1>(EcsDelegates.ComponentDelegate<T1> a, bool useLock = false)
		where T1 : unmanaged, IComponent
		=> archetypes.ForeachAsync<T1>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    public Task ForeachAsyncBatched<T1>(EcsDelegates.ComponentDelegate<T1> batched, EcsDelegates.ComponentDelegate<T1> basic, int batchSize, bool useLock = false)
		where T1 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<T1>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    public Task ForeachAsync<TFilter, T1>(EcsDelegates.ComponentDelegate<T1> a, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		=> archetypes.ForeachAsync<TFilter, T1>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    public Task ForeachAsyncBatched<TFilter, T1>(EcsDelegates.ComponentDelegate<T1> batched, EcsDelegates.ComponentDelegate<T1> basic, int batchSize, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<TFilter, T1>(batched, basic, batchSize, useLock);

	/// <summary>
	/// get or create new <see cref="Archetype"/> with exact match of components <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    public Archetype GetArchetype<T1, T2>()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        => GetArchetype(typeof(T1), typeof(T2));

	/// <summary>
	/// create new <see cref="Entity"/> with exact match of components <br/><br/>
	/// you can add/remove components later, but with small performance hit <br/><br/>
	/// if you want to add multiple entities, use <see cref="CreateEntities{T1, T2}"/> or <see cref="CreateEntity(Archetype)"/> with archetype from <see cref="GetArchetype{T1, T2}"/>  <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    public EntityId CreateEntity<T1, T2>()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        => CreateEntity(GetArchetype<T1, T2>());

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	/// if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2}"/> <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    public void CreateEntities<T1, T2>(int count)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        => CreateEntities(count, GetArchetype<T1, T2>(), _=>{});

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	/// if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2}"/> <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    public void CreateEntitiesForeach<T1, T2>(int count, Action<EntityId> onEntityCreation)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        => CreateEntities(count, GetArchetype<T1, T2>(), onEntityCreation);

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components, and initialize components <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity, used for component initialization</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    public unsafe void CreateEntitiesForeachComp<T1, T2>(int count, EcsDelegates.ComponentDelegate<T1, T2> onEntityCreation)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent {
        Archetype archetype = GetArchetype<T1, T2>();
        archetype.PreAllocate(count);
       
        T1* c1 = (T1*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T1).ToEcsComponent()), 0);
        T2* c2 = (T2*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T2).ToEcsComponent()), 0);
        entities.AddMultiple(count, i => {
			EntityId ent = i;
			archetypes.AddEntity(ent, archetype);
			uint offset = archetype.GetComponentIdFromEntity(ent);
			onEntityCreation(c1 + offset, c2 + offset);
		});
    }

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    public void Foreach<T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> a, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
		=> archetypes.Foreach<T1, T2>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    public void ForeachBatched<T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> batched, EcsDelegates.ComponentDelegate<T1, T2> basic, int batchSize, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
		=> archetypes.ForeachBatched<T1, T2>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    public void Foreach<TFilter, T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> a, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
		=> archetypes.Foreach<TFilter, T1, T2>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    public void ForeachBatched<TFilter, T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> batched, EcsDelegates.ComponentDelegate<T1, T2> basic, int batchSize, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
		=> archetypes.ForeachBatched<TFilter, T1, T2>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    public Task ForeachAsync<T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> a, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
		=> archetypes.ForeachAsync<T1, T2>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    public Task ForeachAsyncBatched<T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> batched, EcsDelegates.ComponentDelegate<T1, T2> basic, int batchSize, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<T1, T2>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    public Task ForeachAsync<TFilter, T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> a, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
		=> archetypes.ForeachAsync<TFilter, T1, T2>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    public Task ForeachAsyncBatched<TFilter, T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> batched, EcsDelegates.ComponentDelegate<T1, T2> basic, int batchSize, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<TFilter, T1, T2>(batched, basic, batchSize, useLock);

	/// <summary>
	/// get or create new <see cref="Archetype"/> with exact match of components <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    public Archetype GetArchetype<T1, T2, T3>()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        => GetArchetype(typeof(T1), typeof(T2), typeof(T3));

	/// <summary>
	/// create new <see cref="Entity"/> with exact match of components <br/><br/>
	/// you can add/remove components later, but with small performance hit <br/><br/>
	/// if you want to add multiple entities, use <see cref="CreateEntities{T1, T2, T3}"/> or <see cref="CreateEntity(Archetype)"/> with archetype from <see cref="GetArchetype{T1, T2, T3}"/>  <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    public EntityId CreateEntity<T1, T2, T3>()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        => CreateEntity(GetArchetype<T1, T2, T3>());

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	/// if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3}"/> <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    public void CreateEntities<T1, T2, T3>(int count)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        => CreateEntities(count, GetArchetype<T1, T2, T3>(), _=>{});

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	/// if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3}"/> <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    public void CreateEntitiesForeach<T1, T2, T3>(int count, Action<EntityId> onEntityCreation)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        => CreateEntities(count, GetArchetype<T1, T2, T3>(), onEntityCreation);

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components, and initialize components <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity, used for component initialization</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    public unsafe void CreateEntitiesForeachComp<T1, T2, T3>(int count, EcsDelegates.ComponentDelegate<T1, T2, T3> onEntityCreation)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent {
        Archetype archetype = GetArchetype<T1, T2, T3>();
        archetype.PreAllocate(count);
       
        T1* c1 = (T1*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T1).ToEcsComponent()), 0);
        T2* c2 = (T2*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T2).ToEcsComponent()), 0);
        T3* c3 = (T3*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T3).ToEcsComponent()), 0);
        entities.AddMultiple(count, i => {
			EntityId ent = i;
			archetypes.AddEntity(ent, archetype);
			uint offset = archetype.GetComponentIdFromEntity(ent);
			onEntityCreation(c1 + offset, c2 + offset, c3 + offset);
		});
    }

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    public void Foreach<T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> a, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
		=> archetypes.Foreach<T1, T2, T3>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    public void ForeachBatched<T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> batched, EcsDelegates.ComponentDelegate<T1, T2, T3> basic, int batchSize, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
		=> archetypes.ForeachBatched<T1, T2, T3>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    public void Foreach<TFilter, T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> a, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
		=> archetypes.Foreach<TFilter, T1, T2, T3>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    public void ForeachBatched<TFilter, T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> batched, EcsDelegates.ComponentDelegate<T1, T2, T3> basic, int batchSize, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
		=> archetypes.ForeachBatched<TFilter, T1, T2, T3>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    public Task ForeachAsync<T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> a, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
		=> archetypes.ForeachAsync<T1, T2, T3>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    public Task ForeachAsyncBatched<T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> batched, EcsDelegates.ComponentDelegate<T1, T2, T3> basic, int batchSize, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<T1, T2, T3>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    public Task ForeachAsync<TFilter, T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> a, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
		=> archetypes.ForeachAsync<TFilter, T1, T2, T3>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    public Task ForeachAsyncBatched<TFilter, T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> batched, EcsDelegates.ComponentDelegate<T1, T2, T3> basic, int batchSize, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<TFilter, T1, T2, T3>(batched, basic, batchSize, useLock);

	/// <summary>
	/// get or create new <see cref="Archetype"/> with exact match of components <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    public Archetype GetArchetype<T1, T2, T3, T4>()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        => GetArchetype(typeof(T1), typeof(T2), typeof(T3), typeof(T4));

	/// <summary>
	/// create new <see cref="Entity"/> with exact match of components <br/><br/>
	/// you can add/remove components later, but with small performance hit <br/><br/>
	/// if you want to add multiple entities, use <see cref="CreateEntities{T1, T2, T3, T4}"/> or <see cref="CreateEntity(Archetype)"/> with archetype from <see cref="GetArchetype{T1, T2, T3, T4}"/>  <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    public EntityId CreateEntity<T1, T2, T3, T4>()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        => CreateEntity(GetArchetype<T1, T2, T3, T4>());

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	/// if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3, T4}"/> <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    public void CreateEntities<T1, T2, T3, T4>(int count)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        => CreateEntities(count, GetArchetype<T1, T2, T3, T4>(), _=>{});

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	/// if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3, T4}"/> <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    public void CreateEntitiesForeach<T1, T2, T3, T4>(int count, Action<EntityId> onEntityCreation)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        => CreateEntities(count, GetArchetype<T1, T2, T3, T4>(), onEntityCreation);

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components, and initialize components <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity, used for component initialization</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    public unsafe void CreateEntitiesForeachComp<T1, T2, T3, T4>(int count, EcsDelegates.ComponentDelegate<T1, T2, T3, T4> onEntityCreation)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent {
        Archetype archetype = GetArchetype<T1, T2, T3, T4>();
        archetype.PreAllocate(count);
       
        T1* c1 = (T1*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T1).ToEcsComponent()), 0);
        T2* c2 = (T2*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T2).ToEcsComponent()), 0);
        T3* c3 = (T3*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T3).ToEcsComponent()), 0);
        T4* c4 = (T4*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T4).ToEcsComponent()), 0);
        entities.AddMultiple(count, i => {
			EntityId ent = i;
			archetypes.AddEntity(ent, archetype);
			uint offset = archetype.GetComponentIdFromEntity(ent);
			onEntityCreation(c1 + offset, c2 + offset, c3 + offset, c4 + offset);
		});
    }

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    public void Foreach<T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> a, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
		=> archetypes.Foreach<T1, T2, T3, T4>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    public void ForeachBatched<T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4> basic, int batchSize, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
		=> archetypes.ForeachBatched<T1, T2, T3, T4>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    public void Foreach<TFilter, T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> a, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
		=> archetypes.Foreach<TFilter, T1, T2, T3, T4>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    public void ForeachBatched<TFilter, T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4> basic, int batchSize, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
		=> archetypes.ForeachBatched<TFilter, T1, T2, T3, T4>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    public Task ForeachAsync<T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> a, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
		=> archetypes.ForeachAsync<T1, T2, T3, T4>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    public Task ForeachAsyncBatched<T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4> basic, int batchSize, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<T1, T2, T3, T4>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    public Task ForeachAsync<TFilter, T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> a, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
		=> archetypes.ForeachAsync<TFilter, T1, T2, T3, T4>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    public Task ForeachAsyncBatched<TFilter, T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4> basic, int batchSize, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<TFilter, T1, T2, T3, T4>(batched, basic, batchSize, useLock);

	/// <summary>
	/// get or create new <see cref="Archetype"/> with exact match of components <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    public Archetype GetArchetype<T1, T2, T3, T4, T5>()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        => GetArchetype(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));

	/// <summary>
	/// create new <see cref="Entity"/> with exact match of components <br/><br/>
	/// you can add/remove components later, but with small performance hit <br/><br/>
	/// if you want to add multiple entities, use <see cref="CreateEntities{T1, T2, T3, T4, T5}"/> or <see cref="CreateEntity(Archetype)"/> with archetype from <see cref="GetArchetype{T1, T2, T3, T4, T5}"/>  <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    public EntityId CreateEntity<T1, T2, T3, T4, T5>()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        => CreateEntity(GetArchetype<T1, T2, T3, T4, T5>());

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	/// if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3, T4, T5}"/> <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    public void CreateEntities<T1, T2, T3, T4, T5>(int count)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        => CreateEntities(count, GetArchetype<T1, T2, T3, T4, T5>(), _=>{});

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	/// if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3, T4, T5}"/> <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    public void CreateEntitiesForeach<T1, T2, T3, T4, T5>(int count, Action<EntityId> onEntityCreation)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        => CreateEntities(count, GetArchetype<T1, T2, T3, T4, T5>(), onEntityCreation);

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components, and initialize components <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity, used for component initialization</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    public unsafe void CreateEntitiesForeachComp<T1, T2, T3, T4, T5>(int count, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> onEntityCreation)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent {
        Archetype archetype = GetArchetype<T1, T2, T3, T4, T5>();
        archetype.PreAllocate(count);
       
        T1* c1 = (T1*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T1).ToEcsComponent()), 0);
        T2* c2 = (T2*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T2).ToEcsComponent()), 0);
        T3* c3 = (T3*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T3).ToEcsComponent()), 0);
        T4* c4 = (T4*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T4).ToEcsComponent()), 0);
        T5* c5 = (T5*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T5).ToEcsComponent()), 0);
        entities.AddMultiple(count, i => {
			EntityId ent = i;
			archetypes.AddEntity(ent, archetype);
			uint offset = archetype.GetComponentIdFromEntity(ent);
			onEntityCreation(c1 + offset, c2 + offset, c3 + offset, c4 + offset, c5 + offset);
		});
    }

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    public void Foreach<T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> a, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
		=> archetypes.Foreach<T1, T2, T3, T4, T5>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    public void ForeachBatched<T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> basic, int batchSize, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
		=> archetypes.ForeachBatched<T1, T2, T3, T4, T5>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    public void Foreach<TFilter, T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> a, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
		=> archetypes.Foreach<TFilter, T1, T2, T3, T4, T5>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    public void ForeachBatched<TFilter, T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> basic, int batchSize, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
		=> archetypes.ForeachBatched<TFilter, T1, T2, T3, T4, T5>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    public Task ForeachAsync<T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> a, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
		=> archetypes.ForeachAsync<T1, T2, T3, T4, T5>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    public Task ForeachAsyncBatched<T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> basic, int batchSize, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<T1, T2, T3, T4, T5>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    public Task ForeachAsync<TFilter, T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> a, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
		=> archetypes.ForeachAsync<TFilter, T1, T2, T3, T4, T5>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    public Task ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> basic, int batchSize, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5>(batched, basic, batchSize, useLock);

	/// <summary>
	/// get or create new <see cref="Archetype"/> with exact match of components <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    public Archetype GetArchetype<T1, T2, T3, T4, T5, T6>()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        => GetArchetype(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));

	/// <summary>
	/// create new <see cref="Entity"/> with exact match of components <br/><br/>
	/// you can add/remove components later, but with small performance hit <br/><br/>
	/// if you want to add multiple entities, use <see cref="CreateEntities{T1, T2, T3, T4, T5, T6}"/> or <see cref="CreateEntity(Archetype)"/> with archetype from <see cref="GetArchetype{T1, T2, T3, T4, T5, T6}"/>  <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    public EntityId CreateEntity<T1, T2, T3, T4, T5, T6>()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        => CreateEntity(GetArchetype<T1, T2, T3, T4, T5, T6>());

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	/// if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3, T4, T5, T6}"/> <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    public void CreateEntities<T1, T2, T3, T4, T5, T6>(int count)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        => CreateEntities(count, GetArchetype<T1, T2, T3, T4, T5, T6>(), _=>{});

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	/// if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3, T4, T5, T6}"/> <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    public void CreateEntitiesForeach<T1, T2, T3, T4, T5, T6>(int count, Action<EntityId> onEntityCreation)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        => CreateEntities(count, GetArchetype<T1, T2, T3, T4, T5, T6>(), onEntityCreation);

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components, and initialize components <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity, used for component initialization</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    public unsafe void CreateEntitiesForeachComp<T1, T2, T3, T4, T5, T6>(int count, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> onEntityCreation)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent {
        Archetype archetype = GetArchetype<T1, T2, T3, T4, T5, T6>();
        archetype.PreAllocate(count);
       
        T1* c1 = (T1*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T1).ToEcsComponent()), 0);
        T2* c2 = (T2*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T2).ToEcsComponent()), 0);
        T3* c3 = (T3*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T3).ToEcsComponent()), 0);
        T4* c4 = (T4*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T4).ToEcsComponent()), 0);
        T5* c5 = (T5*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T5).ToEcsComponent()), 0);
        T6* c6 = (T6*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T6).ToEcsComponent()), 0);
        entities.AddMultiple(count, i => {
			EntityId ent = i;
			archetypes.AddEntity(ent, archetype);
			uint offset = archetype.GetComponentIdFromEntity(ent);
			onEntityCreation(c1 + offset, c2 + offset, c3 + offset, c4 + offset, c5 + offset, c6 + offset);
		});
    }

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    public void Foreach<T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> a, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
		=> archetypes.Foreach<T1, T2, T3, T4, T5, T6>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    public void ForeachBatched<T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> basic, int batchSize, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
		=> archetypes.ForeachBatched<T1, T2, T3, T4, T5, T6>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    public void Foreach<TFilter, T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> a, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
		=> archetypes.Foreach<TFilter, T1, T2, T3, T4, T5, T6>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    public void ForeachBatched<TFilter, T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> basic, int batchSize, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
		=> archetypes.ForeachBatched<TFilter, T1, T2, T3, T4, T5, T6>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    public Task ForeachAsync<T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> a, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
		=> archetypes.ForeachAsync<T1, T2, T3, T4, T5, T6>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    public Task ForeachAsyncBatched<T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> basic, int batchSize, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<T1, T2, T3, T4, T5, T6>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    public Task ForeachAsync<TFilter, T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> a, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
		=> archetypes.ForeachAsync<TFilter, T1, T2, T3, T4, T5, T6>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    public Task ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> basic, int batchSize, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5, T6>(batched, basic, batchSize, useLock);

	/// <summary>
	/// get or create new <see cref="Archetype"/> with exact match of components <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    public Archetype GetArchetype<T1, T2, T3, T4, T5, T6, T7>()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        => GetArchetype(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));

	/// <summary>
	/// create new <see cref="Entity"/> with exact match of components <br/><br/>
	/// you can add/remove components later, but with small performance hit <br/><br/>
	/// if you want to add multiple entities, use <see cref="CreateEntities{T1, T2, T3, T4, T5, T6, T7}"/> or <see cref="CreateEntity(Archetype)"/> with archetype from <see cref="GetArchetype{T1, T2, T3, T4, T5, T6, T7}"/>  <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    public EntityId CreateEntity<T1, T2, T3, T4, T5, T6, T7>()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        => CreateEntity(GetArchetype<T1, T2, T3, T4, T5, T6, T7>());

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	/// if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3, T4, T5, T6, T7}"/> <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    public void CreateEntities<T1, T2, T3, T4, T5, T6, T7>(int count)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        => CreateEntities(count, GetArchetype<T1, T2, T3, T4, T5, T6, T7>(), _=>{});

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	/// if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3, T4, T5, T6, T7}"/> <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    public void CreateEntitiesForeach<T1, T2, T3, T4, T5, T6, T7>(int count, Action<EntityId> onEntityCreation)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        => CreateEntities(count, GetArchetype<T1, T2, T3, T4, T5, T6, T7>(), onEntityCreation);

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components, and initialize components <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity, used for component initialization</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    public unsafe void CreateEntitiesForeachComp<T1, T2, T3, T4, T5, T6, T7>(int count, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> onEntityCreation)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent {
        Archetype archetype = GetArchetype<T1, T2, T3, T4, T5, T6, T7>();
        archetype.PreAllocate(count);
       
        T1* c1 = (T1*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T1).ToEcsComponent()), 0);
        T2* c2 = (T2*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T2).ToEcsComponent()), 0);
        T3* c3 = (T3*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T3).ToEcsComponent()), 0);
        T4* c4 = (T4*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T4).ToEcsComponent()), 0);
        T5* c5 = (T5*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T5).ToEcsComponent()), 0);
        T6* c6 = (T6*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T6).ToEcsComponent()), 0);
        T7* c7 = (T7*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T7).ToEcsComponent()), 0);
        entities.AddMultiple(count, i => {
			EntityId ent = i;
			archetypes.AddEntity(ent, archetype);
			uint offset = archetype.GetComponentIdFromEntity(ent);
			onEntityCreation(c1 + offset, c2 + offset, c3 + offset, c4 + offset, c5 + offset, c6 + offset, c7 + offset);
		});
    }

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    public void Foreach<T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> a, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
		=> archetypes.Foreach<T1, T2, T3, T4, T5, T6, T7>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    public void ForeachBatched<T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> basic, int batchSize, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
		=> archetypes.ForeachBatched<T1, T2, T3, T4, T5, T6, T7>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    public void Foreach<TFilter, T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> a, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
		=> archetypes.Foreach<TFilter, T1, T2, T3, T4, T5, T6, T7>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    public void ForeachBatched<TFilter, T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> basic, int batchSize, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
		=> archetypes.ForeachBatched<TFilter, T1, T2, T3, T4, T5, T6, T7>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    public Task ForeachAsync<T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> a, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
		=> archetypes.ForeachAsync<T1, T2, T3, T4, T5, T6, T7>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    public Task ForeachAsyncBatched<T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> basic, int batchSize, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<T1, T2, T3, T4, T5, T6, T7>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    public Task ForeachAsync<TFilter, T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> a, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
		=> archetypes.ForeachAsync<TFilter, T1, T2, T3, T4, T5, T6, T7>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    public Task ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> basic, int batchSize, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5, T6, T7>(batched, basic, batchSize, useLock);

	/// <summary>
	/// get or create new <see cref="Archetype"/> with exact match of components <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    /// <typeparam name="T8">component #8</typeparam>
    public Archetype GetArchetype<T1, T2, T3, T4, T5, T6, T7, T8>()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        where T8 : unmanaged, IComponent
        => GetArchetype(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));

	/// <summary>
	/// create new <see cref="Entity"/> with exact match of components <br/><br/>
	/// you can add/remove components later, but with small performance hit <br/><br/>
	/// if you want to add multiple entities, use <see cref="CreateEntities{T1, T2, T3, T4, T5, T6, T7, T8}"/> or <see cref="CreateEntity(Archetype)"/> with archetype from <see cref="GetArchetype{T1, T2, T3, T4, T5, T6, T7, T8}"/>  <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    /// <typeparam name="T8">component #8</typeparam>
    public EntityId CreateEntity<T1, T2, T3, T4, T5, T6, T7, T8>()
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        where T8 : unmanaged, IComponent
        => CreateEntity(GetArchetype<T1, T2, T3, T4, T5, T6, T7, T8>());

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	/// if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3, T4, T5, T6, T7, T8}"/> <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    /// <typeparam name="T8">component #8</typeparam>
    public void CreateEntities<T1, T2, T3, T4, T5, T6, T7, T8>(int count)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        where T8 : unmanaged, IComponent
        => CreateEntities(count, GetArchetype<T1, T2, T3, T4, T5, T6, T7, T8>(), _=>{});

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	/// if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3, T4, T5, T6, T7, T8}"/> <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    /// <typeparam name="T8">component #8</typeparam>
    public void CreateEntitiesForeach<T1, T2, T3, T4, T5, T6, T7, T8>(int count, Action<EntityId> onEntityCreation)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        where T8 : unmanaged, IComponent
        => CreateEntities(count, GetArchetype<T1, T2, T3, T4, T5, T6, T7, T8>(), onEntityCreation);

	/// <summary>
	/// create multiple <see cref="Entity"/>'ties with exact match of components, and initialize components <br/><br/>
	/// will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity, used for component initialization</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    /// <typeparam name="T8">component #8</typeparam>
    public unsafe void CreateEntitiesForeachComp<T1, T2, T3, T4, T5, T6, T7, T8>(int count, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> onEntityCreation)
        where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        where T8 : unmanaged, IComponent {
        Archetype archetype = GetArchetype<T1, T2, T3, T4, T5, T6, T7, T8>();
        archetype.PreAllocate(count);
       
        T1* c1 = (T1*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T1).ToEcsComponent()), 0);
        T2* c2 = (T2*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T2).ToEcsComponent()), 0);
        T3* c3 = (T3*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T3).ToEcsComponent()), 0);
        T4* c4 = (T4*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T4).ToEcsComponent()), 0);
        T5* c5 = (T5*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T5).ToEcsComponent()), 0);
        T6* c6 = (T6*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T6).ToEcsComponent()), 0);
        T7* c7 = (T7*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T7).ToEcsComponent()), 0);
        T8* c8 = (T8*) archetype.GetComponent(archetype.IndexOfComponent(typeof(T8).ToEcsComponent()), 0);
        entities.AddMultiple(count, i => {
			EntityId ent = i;
			archetypes.AddEntity(ent, archetype);
			uint offset = archetype.GetComponentIdFromEntity(ent);
			onEntityCreation(c1 + offset, c2 + offset, c3 + offset, c4 + offset, c5 + offset, c6 + offset, c7 + offset, c8 + offset);
		});
    }

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    /// <typeparam name="T8">component #8</typeparam>
    public void Foreach<T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> a, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        where T8 : unmanaged, IComponent
		=> archetypes.Foreach<T1, T2, T3, T4, T5, T6, T7, T8>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    /// <typeparam name="T8">component #8</typeparam>
    public void ForeachBatched<T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> basic, int batchSize, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        where T8 : unmanaged, IComponent
		=> archetypes.ForeachBatched<T1, T2, T3, T4, T5, T6, T7, T8>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    /// <typeparam name="T8">component #8</typeparam>
    public void Foreach<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> a, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        where T8 : unmanaged, IComponent
		=> archetypes.Foreach<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    /// <typeparam name="T8">component #8</typeparam>
    public void ForeachBatched<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> basic, int batchSize, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        where T8 : unmanaged, IComponent
		=> archetypes.ForeachBatched<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    /// <typeparam name="T8">component #8</typeparam>
    public Task ForeachAsync<T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> a, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        where T8 : unmanaged, IComponent
		=> archetypes.ForeachAsync<T1, T2, T3, T4, T5, T6, T7, T8>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    /// <typeparam name="T8">component #8</typeparam>
    public Task ForeachAsyncBatched<T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> basic, int batchSize, bool useLock = false)
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        where T8 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<T1, T2, T3, T4, T5, T6, T7, T8>(batched, basic, batchSize, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    /// <typeparam name="T8">component #8</typeparam>
    public Task ForeachAsync<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> a, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        where T8 : unmanaged, IComponent
		=> archetypes.ForeachAsync<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(a, useLock);

	/// <summary>
	/// invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	/// every used archetype will execute in different thread <br/>
	/// archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	/// filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	/// filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	/// you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <param name="useLock">lock used archetypes, to prevent race condition</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
    /// <typeparam name="T2">component #2</typeparam>
    /// <typeparam name="T3">component #3</typeparam>
    /// <typeparam name="T4">component #4</typeparam>
    /// <typeparam name="T5">component #5</typeparam>
    /// <typeparam name="T6">component #6</typeparam>
    /// <typeparam name="T7">component #7</typeparam>
    /// <typeparam name="T8">component #8</typeparam>
    public Task ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> basic, int batchSize, bool useLock = false)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
        where T2 : unmanaged, IComponent
        where T3 : unmanaged, IComponent
        where T4 : unmanaged, IComponent
        where T5 : unmanaged, IComponent
        where T6 : unmanaged, IComponent
        where T7 : unmanaged, IComponent
        where T8 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(batched, basic, batchSize, useLock);

}