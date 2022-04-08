using Quartz.objects.ecs.archetypes;
using Quartz.objects.ecs.components;
using Quartz.objects.ecs.components.attributes;
using Quartz.objects.ecs.delegates;
using Quartz.objects.ecs.entities;
using Quartz.objects.ecs.filters;
using Quartz.objects.memory;

namespace Quartz.objects.ecs.world;

public partial class World {
	/// <summary>
	///     get or create new <see cref="Archetype"/> with exact match of components <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <typeparam name="T1">component #1</typeparam>
	public Archetype GetArchetype<T1>()
		where T1 : unmanaged, IComponent
		=> GetArchetype(typeof(T1))!;

	/// <summary>
	///     create new <see cref="Entity"/> with exact match of components <br/><br/>
	///     you can add/remove components later, but with small performance hit <br/><br/>
	///     if you want to add multiple entities, use <see cref="CreateEntities{T1}"/> or <see cref="CreateEntity(Archetype, InitMode)"/> with archetype from <see cref="GetArchetype{T1}"/>  <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	public EntityId CreateEntity<T1>(InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		=> CreateEntity(GetArchetype<T1>(), initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	///     if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1}"/> <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	public void CreateEntities<T1>(int count, InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		=> CreateEntities(count, GetArchetype<T1>(), _ => { }, initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	///     if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1}"/> <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity</param>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	public void CreateEntitiesForeach<T1>(int count, Action<EntityId> onEntityCreation, InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		=> CreateEntities(count, GetArchetype<T1>(), onEntityCreation, initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components, and initialize components <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity, used for component initialization</param>
	/// <typeparam name="T1">component #1</typeparam>
	public unsafe void CreateEntitiesForeachComp<T1>(int count, EcsDelegates.ComponentDelegate<T1> onEntityCreation)
		where T1 : unmanaged, IComponent {
		Archetype archetype = GetArchetype<T1>();
		archetype.PreAllocate(count);

		T1* c1 = (T1*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T1).ToEcsComponent()), 0);
		CreateEmptyEntities(count, i => {
			archetypes.AddEntity(i, archetype);
			uint offset = archetype.GetComponentIdFromEntity(i);
			onEntityCreation(c1 + offset);
		});
	}

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="T1">component #1</typeparam>
	public void Foreach<T1>(EcsDelegates.ComponentDelegate<T1> a)
		where T1 : unmanaged, IComponent
		=> archetypes.Foreach(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="T1">component #1</typeparam>
	public void ForeachBatched<T1>(EcsDelegates.ComponentDelegate<T1> batched, EcsDelegates.ComponentDelegate<T1> basic, int batchSize)
		where T1 : unmanaged, IComponent
		=> archetypes.ForeachBatched(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	public void Foreach<TFilter, T1>(EcsDelegates.ComponentDelegate<T1> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		=> archetypes.Foreach<TFilter, T1>(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	public void ForeachBatched<TFilter, T1>(EcsDelegates.ComponentDelegate<T1> batched, EcsDelegates.ComponentDelegate<T1> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		=> archetypes.ForeachBatched<TFilter, T1>(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="T1">component #1</typeparam>
	public Task ForeachAsync<T1>(EcsDelegates.ComponentDelegate<T1> a)
		where T1 : unmanaged, IComponent
		=> archetypes.ForeachAsync(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="T1">component #1</typeparam>
	public Task ForeachAsyncBatched<T1>(EcsDelegates.ComponentDelegate<T1> batched, EcsDelegates.ComponentDelegate<T1> basic, int batchSize)
		where T1 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	public Task ForeachAsync<TFilter, T1>(EcsDelegates.ComponentDelegate<T1> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		=> archetypes.ForeachAsync<TFilter, T1>(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	public Task ForeachAsyncBatched<TFilter, T1>(EcsDelegates.ComponentDelegate<T1> batched, EcsDelegates.ComponentDelegate<T1> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<TFilter, T1>(batched, basic, batchSize);

	/// <summary>
	///     get or create new <see cref="Archetype"/> with exact match of components <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	public Archetype GetArchetype<T1, T2>()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		=> GetArchetype(typeof(T1), typeof(T2))!;

	/// <summary>
	///     create new <see cref="Entity"/> with exact match of components <br/><br/>
	///     you can add/remove components later, but with small performance hit <br/><br/>
	///     if you want to add multiple entities, use <see cref="CreateEntities{T1, T2}"/> or <see cref="CreateEntity(Archetype, InitMode)"/> with archetype from <see cref="GetArchetype{T1, T2}"/>  <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	public EntityId CreateEntity<T1, T2>(InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		=> CreateEntity(GetArchetype<T1, T2>(), initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	///     if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2}"/> <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	public void CreateEntities<T1, T2>(int count, InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		=> CreateEntities(count, GetArchetype<T1, T2>(), _ => { }, initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	///     if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2}"/> <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity</param>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	public void CreateEntitiesForeach<T1, T2>(int count, Action<EntityId> onEntityCreation, InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		=> CreateEntities(count, GetArchetype<T1, T2>(), onEntityCreation, initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components, and initialize components <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
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

		T1* c1 = (T1*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T1).ToEcsComponent()), 0);
		T2* c2 = (T2*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T2).ToEcsComponent()), 0);
		CreateEmptyEntities(count, i => {
			archetypes.AddEntity(i, archetype);
			uint offset = archetype.GetComponentIdFromEntity(i);
			onEntityCreation(c1 + offset, c2 + offset);
		});
	}

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	public void Foreach<T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		=> archetypes.Foreach(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	public void ForeachBatched<T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> batched, EcsDelegates.ComponentDelegate<T1, T2> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		=> archetypes.ForeachBatched(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	public void Foreach<TFilter, T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		=> archetypes.Foreach<TFilter, T1, T2>(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	public void ForeachBatched<TFilter, T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> batched, EcsDelegates.ComponentDelegate<T1, T2> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		=> archetypes.ForeachBatched<TFilter, T1, T2>(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	public Task ForeachAsync<T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		=> archetypes.ForeachAsync(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	public Task ForeachAsyncBatched<T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> batched, EcsDelegates.ComponentDelegate<T1, T2> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	public Task ForeachAsync<TFilter, T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		=> archetypes.ForeachAsync<TFilter, T1, T2>(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	public Task ForeachAsyncBatched<TFilter, T1, T2>(EcsDelegates.ComponentDelegate<T1, T2> batched, EcsDelegates.ComponentDelegate<T1, T2> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<TFilter, T1, T2>(batched, basic, batchSize);

	/// <summary>
	///     get or create new <see cref="Archetype"/> with exact match of components <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	public Archetype GetArchetype<T1, T2, T3>()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		=> GetArchetype(typeof(T1), typeof(T2), typeof(T3))!;

	/// <summary>
	///     create new <see cref="Entity"/> with exact match of components <br/><br/>
	///     you can add/remove components later, but with small performance hit <br/><br/>
	///     if you want to add multiple entities, use <see cref="CreateEntities{T1, T2, T3}"/> or <see cref="CreateEntity(Archetype, InitMode)"/> with archetype from <see cref="GetArchetype{T1, T2, T3}"/>  <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	public EntityId CreateEntity<T1, T2, T3>(InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		=> CreateEntity(GetArchetype<T1, T2, T3>(), initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	///     if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3}"/> <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	public void CreateEntities<T1, T2, T3>(int count, InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		=> CreateEntities(count, GetArchetype<T1, T2, T3>(), _ => { }, initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	///     if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3}"/> <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity</param>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	public void CreateEntitiesForeach<T1, T2, T3>(int count, Action<EntityId> onEntityCreation, InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		=> CreateEntities(count, GetArchetype<T1, T2, T3>(), onEntityCreation, initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components, and initialize components <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
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

		T1* c1 = (T1*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T1).ToEcsComponent()), 0);
		T2* c2 = (T2*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T2).ToEcsComponent()), 0);
		T3* c3 = (T3*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T3).ToEcsComponent()), 0);
		CreateEmptyEntities(count, i => {
			archetypes.AddEntity(i, archetype);
			uint offset = archetype.GetComponentIdFromEntity(i);
			onEntityCreation(c1 + offset, c2 + offset, c3 + offset);
		});
	}

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	public void Foreach<T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		=> archetypes.Foreach(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	public void ForeachBatched<T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> batched, EcsDelegates.ComponentDelegate<T1, T2, T3> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		=> archetypes.ForeachBatched(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	public void Foreach<TFilter, T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		=> archetypes.Foreach<TFilter, T1, T2, T3>(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	public void ForeachBatched<TFilter, T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> batched, EcsDelegates.ComponentDelegate<T1, T2, T3> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		=> archetypes.ForeachBatched<TFilter, T1, T2, T3>(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	public Task ForeachAsync<T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		=> archetypes.ForeachAsync(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	public Task ForeachAsyncBatched<T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> batched, EcsDelegates.ComponentDelegate<T1, T2, T3> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	public Task ForeachAsync<TFilter, T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		=> archetypes.ForeachAsync<TFilter, T1, T2, T3>(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	public Task ForeachAsyncBatched<TFilter, T1, T2, T3>(EcsDelegates.ComponentDelegate<T1, T2, T3> batched, EcsDelegates.ComponentDelegate<T1, T2, T3> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<TFilter, T1, T2, T3>(batched, basic, batchSize);

	/// <summary>
	///     get or create new <see cref="Archetype"/> with exact match of components <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
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
		=> GetArchetype(typeof(T1), typeof(T2), typeof(T3), typeof(T4))!;

	/// <summary>
	///     create new <see cref="Entity"/> with exact match of components <br/><br/>
	///     you can add/remove components later, but with small performance hit <br/><br/>
	///     if you want to add multiple entities, use <see cref="CreateEntities{T1, T2, T3, T4}"/> or <see cref="CreateEntity(Archetype, InitMode)"/> with archetype from <see cref="GetArchetype{T1, T2, T3, T4}"/>  <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	public EntityId CreateEntity<T1, T2, T3, T4>(InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		=> CreateEntity(GetArchetype<T1, T2, T3, T4>(), initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	///     if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3, T4}"/> <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	public void CreateEntities<T1, T2, T3, T4>(int count, InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		=> CreateEntities(count, GetArchetype<T1, T2, T3, T4>(), _ => { }, initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	///     if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3, T4}"/> <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity</param>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	public void CreateEntitiesForeach<T1, T2, T3, T4>(int count, Action<EntityId> onEntityCreation, InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		=> CreateEntities(count, GetArchetype<T1, T2, T3, T4>(), onEntityCreation, initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components, and initialize components <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
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

		T1* c1 = (T1*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T1).ToEcsComponent()), 0);
		T2* c2 = (T2*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T2).ToEcsComponent()), 0);
		T3* c3 = (T3*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T3).ToEcsComponent()), 0);
		T4* c4 = (T4*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T4).ToEcsComponent()), 0);
		CreateEmptyEntities(count, i => {
			archetypes.AddEntity(i, archetype);
			uint offset = archetype.GetComponentIdFromEntity(i);
			onEntityCreation(c1 + offset, c2 + offset, c3 + offset, c4 + offset);
		});
	}

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	public void Foreach<T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		=> archetypes.Foreach(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	public void ForeachBatched<T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		=> archetypes.ForeachBatched(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	public void Foreach<TFilter, T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		=> archetypes.Foreach<TFilter, T1, T2, T3, T4>(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	public void ForeachBatched<TFilter, T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		=> archetypes.ForeachBatched<TFilter, T1, T2, T3, T4>(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	public Task ForeachAsync<T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		=> archetypes.ForeachAsync(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	public Task ForeachAsyncBatched<T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	public Task ForeachAsync<TFilter, T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		=> archetypes.ForeachAsync<TFilter, T1, T2, T3, T4>(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	public Task ForeachAsyncBatched<TFilter, T1, T2, T3, T4>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<TFilter, T1, T2, T3, T4>(batched, basic, batchSize);

	/// <summary>
	///     get or create new <see cref="Archetype"/> with exact match of components <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
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
		=> GetArchetype(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5))!;

	/// <summary>
	///     create new <see cref="Entity"/> with exact match of components <br/><br/>
	///     you can add/remove components later, but with small performance hit <br/><br/>
	///     if you want to add multiple entities, use <see cref="CreateEntities{T1, T2, T3, T4, T5}"/> or <see cref="CreateEntity(Archetype, InitMode)"/> with archetype from <see cref="GetArchetype{T1, T2, T3, T4, T5}"/>  <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	public EntityId CreateEntity<T1, T2, T3, T4, T5>(InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		=> CreateEntity(GetArchetype<T1, T2, T3, T4, T5>(), initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	///     if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3, T4, T5}"/> <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	public void CreateEntities<T1, T2, T3, T4, T5>(int count, InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		=> CreateEntities(count, GetArchetype<T1, T2, T3, T4, T5>(), _ => { }, initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	///     if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3, T4, T5}"/> <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity</param>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	public void CreateEntitiesForeach<T1, T2, T3, T4, T5>(int count, Action<EntityId> onEntityCreation, InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		=> CreateEntities(count, GetArchetype<T1, T2, T3, T4, T5>(), onEntityCreation, initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components, and initialize components <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
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

		T1* c1 = (T1*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T1).ToEcsComponent()), 0);
		T2* c2 = (T2*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T2).ToEcsComponent()), 0);
		T3* c3 = (T3*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T3).ToEcsComponent()), 0);
		T4* c4 = (T4*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T4).ToEcsComponent()), 0);
		T5* c5 = (T5*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T5).ToEcsComponent()), 0);
		CreateEmptyEntities(count, i => {
			archetypes.AddEntity(i, archetype);
			uint offset = archetype.GetComponentIdFromEntity(i);
			onEntityCreation(c1 + offset, c2 + offset, c3 + offset, c4 + offset, c5 + offset);
		});
	}

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	public void Foreach<T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		=> archetypes.Foreach(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	public void ForeachBatched<T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		=> archetypes.ForeachBatched(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	public void Foreach<TFilter, T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		=> archetypes.Foreach<TFilter, T1, T2, T3, T4, T5>(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	public void ForeachBatched<TFilter, T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		=> archetypes.ForeachBatched<TFilter, T1, T2, T3, T4, T5>(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	public Task ForeachAsync<T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		=> archetypes.ForeachAsync(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	public Task ForeachAsyncBatched<T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	public Task ForeachAsync<TFilter, T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		=> archetypes.ForeachAsync<TFilter, T1, T2, T3, T4, T5>(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	public Task ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5>(batched, basic, batchSize);

	/// <summary>
	///     get or create new <see cref="Archetype"/> with exact match of components <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
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
		=> GetArchetype(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6))!;

	/// <summary>
	///     create new <see cref="Entity"/> with exact match of components <br/><br/>
	///     you can add/remove components later, but with small performance hit <br/><br/>
	///     if you want to add multiple entities, use <see cref="CreateEntities{T1, T2, T3, T4, T5, T6}"/> or <see cref="CreateEntity(Archetype, InitMode)"/> with archetype from <see cref="GetArchetype{T1, T2, T3, T4, T5, T6}"/>  <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	public EntityId CreateEntity<T1, T2, T3, T4, T5, T6>(InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		=> CreateEntity(GetArchetype<T1, T2, T3, T4, T5, T6>(), initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	///     if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3, T4, T5, T6}"/> <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	public void CreateEntities<T1, T2, T3, T4, T5, T6>(int count, InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		=> CreateEntities(count, GetArchetype<T1, T2, T3, T4, T5, T6>(), _ => { }, initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	///     if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3, T4, T5, T6}"/> <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity</param>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	public void CreateEntitiesForeach<T1, T2, T3, T4, T5, T6>(int count, Action<EntityId> onEntityCreation, InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		=> CreateEntities(count, GetArchetype<T1, T2, T3, T4, T5, T6>(), onEntityCreation, initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components, and initialize components <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
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

		T1* c1 = (T1*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T1).ToEcsComponent()), 0);
		T2* c2 = (T2*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T2).ToEcsComponent()), 0);
		T3* c3 = (T3*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T3).ToEcsComponent()), 0);
		T4* c4 = (T4*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T4).ToEcsComponent()), 0);
		T5* c5 = (T5*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T5).ToEcsComponent()), 0);
		T6* c6 = (T6*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T6).ToEcsComponent()), 0);
		CreateEmptyEntities(count, i => {
			archetypes.AddEntity(i, archetype);
			uint offset = archetype.GetComponentIdFromEntity(i);
			onEntityCreation(c1 + offset, c2 + offset, c3 + offset, c4 + offset, c5 + offset, c6 + offset);
		});
	}

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	public void Foreach<T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		=> archetypes.Foreach(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	public void ForeachBatched<T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		=> archetypes.ForeachBatched(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	public void Foreach<TFilter, T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		=> archetypes.Foreach<TFilter, T1, T2, T3, T4, T5, T6>(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	public void ForeachBatched<TFilter, T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		=> archetypes.ForeachBatched<TFilter, T1, T2, T3, T4, T5, T6>(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	public Task ForeachAsync<T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		=> archetypes.ForeachAsync(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	public Task ForeachAsyncBatched<T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	public Task ForeachAsync<TFilter, T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		=> archetypes.ForeachAsync<TFilter, T1, T2, T3, T4, T5, T6>(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	public Task ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5, T6>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5, T6>(batched, basic, batchSize);

	/// <summary>
	///     get or create new <see cref="Archetype"/> with exact match of components <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
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
		=> GetArchetype(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7))!;

	/// <summary>
	///     create new <see cref="Entity"/> with exact match of components <br/><br/>
	///     you can add/remove components later, but with small performance hit <br/><br/>
	///     if you want to add multiple entities, use <see cref="CreateEntities{T1, T2, T3, T4, T5, T6, T7}"/> or <see cref="CreateEntity(Archetype, InitMode)"/> with archetype from <see cref="GetArchetype{T1, T2, T3, T4, T5, T6, T7}"/>  <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	/// <typeparam name="T7">component #7</typeparam>
	public EntityId CreateEntity<T1, T2, T3, T4, T5, T6, T7>(InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		=> CreateEntity(GetArchetype<T1, T2, T3, T4, T5, T6, T7>(), initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	///     if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3, T4, T5, T6, T7}"/> <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	/// <typeparam name="T7">component #7</typeparam>
	public void CreateEntities<T1, T2, T3, T4, T5, T6, T7>(int count, InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		=> CreateEntities(count, GetArchetype<T1, T2, T3, T4, T5, T6, T7>(), _ => { }, initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	///     if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3, T4, T5, T6, T7}"/> <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity</param>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	/// <typeparam name="T7">component #7</typeparam>
	public void CreateEntitiesForeach<T1, T2, T3, T4, T5, T6, T7>(int count, Action<EntityId> onEntityCreation, InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		=> CreateEntities(count, GetArchetype<T1, T2, T3, T4, T5, T6, T7>(), onEntityCreation, initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components, and initialize components <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
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

		T1* c1 = (T1*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T1).ToEcsComponent()), 0);
		T2* c2 = (T2*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T2).ToEcsComponent()), 0);
		T3* c3 = (T3*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T3).ToEcsComponent()), 0);
		T4* c4 = (T4*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T4).ToEcsComponent()), 0);
		T5* c5 = (T5*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T5).ToEcsComponent()), 0);
		T6* c6 = (T6*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T6).ToEcsComponent()), 0);
		T7* c7 = (T7*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T7).ToEcsComponent()), 0);
		CreateEmptyEntities(count, i => {
			archetypes.AddEntity(i, archetype);
			uint offset = archetype.GetComponentIdFromEntity(i);
			onEntityCreation(c1 + offset, c2 + offset, c3 + offset, c4 + offset, c5 + offset, c6 + offset, c7 + offset);
		});
	}

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	/// <typeparam name="T7">component #7</typeparam>
	public void Foreach<T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		=> archetypes.Foreach(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	/// <typeparam name="T7">component #7</typeparam>
	public void ForeachBatched<T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		=> archetypes.ForeachBatched(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	/// <typeparam name="T7">component #7</typeparam>
	public void Foreach<TFilter, T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		=> archetypes.Foreach<TFilter, T1, T2, T3, T4, T5, T6, T7>(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	/// <typeparam name="T7">component #7</typeparam>
	public void ForeachBatched<TFilter, T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		=> archetypes.ForeachBatched<TFilter, T1, T2, T3, T4, T5, T6, T7>(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	/// <typeparam name="T7">component #7</typeparam>
	public Task ForeachAsync<T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		=> archetypes.ForeachAsync(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	/// <typeparam name="T7">component #7</typeparam>
	public Task ForeachAsyncBatched<T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	/// <typeparam name="T7">component #7</typeparam>
	public Task ForeachAsync<TFilter, T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		=> archetypes.ForeachAsync<TFilter, T1, T2, T3, T4, T5, T6, T7>(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	/// <typeparam name="T7">component #7</typeparam>
	public Task ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5, T6, T7>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5, T6, T7>(batched, basic, batchSize);

	/// <summary>
	///     get or create new <see cref="Archetype"/> with exact match of components <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
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
		=> GetArchetype(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8))!;

	/// <summary>
	///     create new <see cref="Entity"/> with exact match of components <br/><br/>
	///     you can add/remove components later, but with small performance hit <br/><br/>
	///     if you want to add multiple entities, use <see cref="CreateEntities{T1, T2, T3, T4, T5, T6, T7, T8}"/> or <see cref="CreateEntity(Archetype, InitMode)"/> with archetype from <see cref="GetArchetype{T1, T2, T3, T4, T5, T6, T7, T8}"/>  <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	/// <typeparam name="T7">component #7</typeparam>
	/// <typeparam name="T8">component #8</typeparam>
	public EntityId CreateEntity<T1, T2, T3, T4, T5, T6, T7, T8>(InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		where T8 : unmanaged, IComponent
		=> CreateEntity(GetArchetype<T1, T2, T3, T4, T5, T6, T7, T8>(), initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	///     if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3, T4, T5, T6, T7, T8}"/> <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	/// <typeparam name="T7">component #7</typeparam>
	/// <typeparam name="T8">component #8</typeparam>
	public void CreateEntities<T1, T2, T3, T4, T5, T6, T7, T8>(int count, InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		where T8 : unmanaged, IComponent
		=> CreateEntities(count, GetArchetype<T1, T2, T3, T4, T5, T6, T7, T8>(), _ => { }, initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components <br/><br/>
	///     if you want to initialize components, use overload with EcsDelegates.ComponentDelegate{T1, T2, T3, T4, T5, T6, T7, T8}"/> <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
	/// </summary>
	/// <param name="count">count of entities to spawn</param>
	/// <param name="onEntityCreation">delegate, called for every spawned entity</param>
	/// <param name="initMode">initialization mode of each object</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	/// <typeparam name="T7">component #7</typeparam>
	/// <typeparam name="T8">component #8</typeparam>
	public void CreateEntitiesForeach<T1, T2, T3, T4, T5, T6, T7, T8>(int count, Action<EntityId> onEntityCreation, InitMode initMode = InitMode.ctor)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		where T8 : unmanaged, IComponent
		=> CreateEntities(count, GetArchetype<T1, T2, T3, T4, T5, T6, T7, T8>(), onEntityCreation, initMode);

	/// <summary>
	///     create multiple <see cref="Entity"/>'ties with exact match of components, and initialize components <br/><br/>
	///     will also add required attributes (<see cref="RequireAttribute"/>)
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

		T1* c1 = (T1*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T1).ToEcsComponent()), 0);
		T2* c2 = (T2*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T2).ToEcsComponent()), 0);
		T3* c3 = (T3*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T3).ToEcsComponent()), 0);
		T4* c4 = (T4*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T4).ToEcsComponent()), 0);
		T5* c5 = (T5*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T5).ToEcsComponent()), 0);
		T6* c6 = (T6*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T6).ToEcsComponent()), 0);
		T7* c7 = (T7*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T7).ToEcsComponent()), 0);
		T8* c8 = (T8*)archetype.GetComponent(archetype.IndexOfComponent(typeof(T8).ToEcsComponent()), 0);
		CreateEmptyEntities(count, i => {
			archetypes.AddEntity(i, archetype);
			uint offset = archetype.GetComponentIdFromEntity(i);
			onEntityCreation(c1 + offset, c2 + offset, c3 + offset, c4 + offset, c5 + offset, c6 + offset, c7 + offset, c8 + offset);
		});
	}

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	/// <typeparam name="T7">component #7</typeparam>
	/// <typeparam name="T8">component #8</typeparam>
	public void Foreach<T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		where T8 : unmanaged, IComponent
		=> archetypes.Foreach(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	/// <typeparam name="T7">component #7</typeparam>
	/// <typeparam name="T8">component #8</typeparam>
	public void ForeachBatched<T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		where T8 : unmanaged, IComponent
		=> archetypes.ForeachBatched(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	/// <typeparam name="T7">component #7</typeparam>
	/// <typeparam name="T8">component #8</typeparam>
	public void Foreach<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		where T8 : unmanaged, IComponent
		=> archetypes.Foreach<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	/// <typeparam name="T7">component #7</typeparam>
	/// <typeparam name="T8">component #8</typeparam>
	public void ForeachBatched<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		where T8 : unmanaged, IComponent
		=> archetypes.ForeachBatched<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	/// <typeparam name="T7">component #7</typeparam>
	/// <typeparam name="T8">component #8</typeparam>
	public Task ForeachAsync<T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> a)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		where T8 : unmanaged, IComponent
		=> archetypes.ForeachAsync(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components, even if there are other components <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     to iterate with exact match / excluded components, use overload with <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	/// <typeparam name="T7">component #7</typeparam>
	/// <typeparam name="T8">component #8</typeparam>
	public Task ForeachAsyncBatched<T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> basic, int batchSize)
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		where T8 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched(batched, basic, batchSize);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="a">delegate, called for every matched entity component</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	/// <typeparam name="T7">component #7</typeparam>
	/// <typeparam name="T8">component #8</typeparam>
	public Task ForeachAsync<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> a)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		where T8 : unmanaged, IComponent
		=> archetypes.ForeachAsync<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(a);

	/// <summary>
	///     invoke delegate for every entity, which has all of the specified components and matches filter <br/><br/>
	///     every used archetype will execute in different thread <br/>
	///     archetype sizes can vary a lot, so it`s not good at load-balancing <br/><br/>
	///     filters, that can include components: <see cref="All{T1, T2}"/>, <see cref="Any{T1, T2}"/> or exclude <see cref="None{T1, T2}"/>  <br/>
	///     filters, that can combine other filters: <see cref="And{TF1, TF2}"/>, <see cref="Or{TF1, TF2}"/>, <see cref="Not{TFilter}"/>  <br/>
	///     you can create custom filters, see: <see cref="IEcsFilter"/>
	/// </summary>
	/// <param name="batched">delegate, called for every matched entity component batch</param>
	/// <param name="basic">delegate, called for every matched entity, which is not included to batch</param>
	/// <param name="batchSize">size of single batch</param>
	/// <typeparam name="TFilter">filter</typeparam>
	/// <typeparam name="T1">component #1</typeparam>
	/// <typeparam name="T2">component #2</typeparam>
	/// <typeparam name="T3">component #3</typeparam>
	/// <typeparam name="T4">component #4</typeparam>
	/// <typeparam name="T5">component #5</typeparam>
	/// <typeparam name="T6">component #6</typeparam>
	/// <typeparam name="T7">component #7</typeparam>
	/// <typeparam name="T8">component #8</typeparam>
	public Task ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> batched, EcsDelegates.ComponentDelegate<T1, T2, T3, T4, T5, T6, T7, T8> basic, int batchSize)
		where TFilter : IEcsFilter, new()
		where T1 : unmanaged, IComponent
		where T2 : unmanaged, IComponent
		where T3 : unmanaged, IComponent
		where T4 : unmanaged, IComponent
		where T5 : unmanaged, IComponent
		where T6 : unmanaged, IComponent
		where T7 : unmanaged, IComponent
		where T8 : unmanaged, IComponent
		=> archetypes.ForeachAsyncBatched<TFilter, T1, T2, T3, T4, T5, T6, T7, T8>(batched, basic, batchSize);
}