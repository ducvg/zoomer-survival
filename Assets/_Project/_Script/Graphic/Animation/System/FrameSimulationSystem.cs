using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

namespace Zoomer.Graphic.Animation
{
	using DrawBatch = DrawFrameData.DrawBatch;

	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial struct FrameSimulationSystem : ISystem
	{
		private EntityQuery _query;

		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{

			_query = SystemAPI.QueryBuilder()
				// .WithAll<CharacterAnimationData>()
				.WithAllRW<ActionAnimationData>()
				// .WithAll<LocalToWorld>()
				.Build();

			state.RequireForUpdate<ActionAnimationData>();
			// state.RequireForUpdate<DrawFrameData>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			state.Dependency = new UpdateFrameIndexJob
			{
				DeltaTime = SystemAPI.Time.DeltaTime,
			}.ScheduleParallel(_query, state.Dependency);
		}

		[BurstCompile]
		private partial struct UpdateFrameIndexJob : IJobEntity
		{
			[ReadOnly] public float DeltaTime;

			private void Execute(ref ActionAnimationData actionData)
			{
				actionData.FrameTimer += DeltaTime;
				float frameDelay = 1f / actionData.NativeData.Fps;
				if (Hint.Likely(actionData.FrameTimer < frameDelay)) return;

				actionData.FrameTimer = 0;
				actionData.FrameIndex = (actionData.FrameIndex + 1) % actionData.NativeData.FrameCount;
			}
		}

		// private void OnUpdateInstancing(ref SystemState state)
		// {
		// 	var drawData = SystemAPI.GetSingleton<DrawFrameData>();
		// 	int entityCount = _query.CalculateEntityCount();
		// 	int maxBatchSize = drawData.MaxBatchCount;

		// 	var batchDataList = new NativeList<(DrawBatch, Matrix4x4)>(entityCount, state.WorldUpdateAllocator);
		// 	new UpdateFrameIndexInstancingJob
		// 	{
		// 		DeltaTime = SystemAPI.Time.DeltaTime,
		// 		DrawBatchDataList = batchDataList.AsParallelWriter()
		// 	}.ScheduleParallel(_query, state.Dependency).Complete();

		// 	ref var batches = ref drawData.DrawBatches;
		// 	batches.Clear();
		// 	var span = batchDataList.AsReadOnlySpan();
		// 	for (int i = 0; i < span.Length; ++i)
		// 	{
		// 		var batchData = span[i].Item1;
		// 		var position = span[i].Item2;

		// 		if (Hint.Unlikely(batches.TryAdd(batchData, new NativeList<Matrix4x4>(64, state.WorldUpdateAllocator))))
		// 		{
		// 			batches[batchData].AddNoResize(position);
		// 		}
		// 		else
		// 		{
		// 			batches[batchData].Add(position);
		// 		}
		// 	}

		// 	SystemAPI.SetSingleton(drawData);
		// }

		// [BurstCompile]
		// private partial struct UpdateFrameIndexInstancingJob : IJobEntity
		// {
		// 	[ReadOnly] public float DeltaTime;
		// 	[WriteOnly] public NativeList<(DrawBatch, Matrix4x4)>.ParallelWriter DrawBatchDataList;

		// 	private void Execute(ref ActionAnimationData actionData, in CharacterAnimationData charAnimData, in LocalToWorld localToWorld)
		// 	{
		// 		var drawBatch = new DrawBatch
		// 		{
		// 			CharConfigId = charAnimData.AnimationConfigId,
		// 			ActionKind = actionData.CurrentAction,
		// 			FrameIndex = actionData.FrameIndex
		// 		};
		// 		DrawBatchDataList.AddNoResize((drawBatch, localToWorld.Value));

		// 		actionData.FrameTimer += DeltaTime;
		// 		float frameDelay = 1f / actionData.NativeData.Fps;
		// 		if (Hint.Likely(actionData.FrameTimer < frameDelay)) return;

		// 		actionData.FrameTimer = 0;
		// 		actionData.FrameIndex = (actionData.FrameIndex + 1) % actionData.NativeData.FrameCount;
		// 	}
		// }
	}
}
