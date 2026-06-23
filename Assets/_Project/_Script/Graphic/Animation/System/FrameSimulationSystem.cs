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
				.WithAllRW<ActionAnimationData>()
				.Build();

			state.RequireForUpdate<ActionAnimationData>();
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
	}
}
