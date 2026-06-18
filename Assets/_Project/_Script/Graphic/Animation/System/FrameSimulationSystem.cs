using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Entities;
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
				.WithAll<CharacterAnimationData>()
				.WithAllRW<ActionAnimationData>()
				.WithAll<LocalToWorld>()
				.Build();

			state.RequireForUpdate<ActionAnimationData>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var drawData = SystemAPI.GetSingleton<DrawFrameData>();
			int maxBatchSize = drawData.MaxBatchCount;
			// _drawData.DrawBatches = new NativeParallelHashMap<DrawFrameData.DrawBatch, NativeArray<Matrix4x4>>(maxBatchSize, state.WorldUpdateAllocator);
			drawData.DrawBatches = new NativeParallelMultiHashMap<DrawBatch, Matrix4x4>(_query.CalculateEntityCount(), state.WorldUpdateAllocator);
			drawData.DrawBatchSet = new NativeParallelHashSet<DrawBatch>(maxBatchSize, state.WorldUpdateAllocator);

			new UpdateFrameIndexJob
			{
				DeltaTime = SystemAPI.Time.DeltaTime,
				DrawBatches = drawData.DrawBatches.AsParallelWriter(),
				DrawBatchSet = drawData.DrawBatchSet.AsParallelWriter()
			}.ScheduleParallel(_query, state.Dependency).Complete();

			SystemAPI.SetSingleton(drawData);
		}

		[BurstCompile]
		private partial struct UpdateFrameIndexJob : IJobEntity
		{
			[ReadOnly] public float DeltaTime;
			[WriteOnly] public NativeParallelMultiHashMap<DrawBatch, Matrix4x4>.ParallelWriter DrawBatches;
			[WriteOnly] public NativeParallelHashSet<DrawBatch>.ParallelWriter DrawBatchSet;

			private void Execute(ref ActionAnimationData actionData,
			in CharacterAnimationData charAnimData, in LocalToWorld localToWorld)
			{
				var drawBatch = new DrawBatch
				{
					CharConfigId = charAnimData.AnimationConfigId,
					ActionKind = actionData.CurrentAction,
					FrameIndex = actionData.FrameIndex
				};

				if (DrawBatchSet.TryAdd(drawBatch))
				{
					DrawBatches.Add(drawBatch, localToWorld.Value);
				}

				actionData.FrameTimer += DeltaTime;
				float frameDelay = 1f / actionData.NativeData.Fps;
				if (Hint.Likely(actionData.FrameTimer < frameDelay)) return;

				actionData.FrameTimer = 0;
				actionData.FrameIndex = (actionData.FrameIndex + 1) % actionData.NativeData.FrameCount;


			}
		}
	}
}
