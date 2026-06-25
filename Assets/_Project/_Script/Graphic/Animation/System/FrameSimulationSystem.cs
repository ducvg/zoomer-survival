using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Zoomer.Graphic.Animation
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial struct FrameSimulationSystem : ISystem
	{
		private EntityQuery _query;
		private EntityQuery _moveQuery;

		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			_query = SystemAPI.QueryBuilder()
				.WithAllRW<ActionAnimationData>()
				.Build();

			_moveQuery = SystemAPI.QueryBuilder()
				.WithAll<LocalToWorld>()
				.WithAll<MoveDirection>()
				.WithAllRW<AnimationTransformData>()
				.Build();

			state.RequireForUpdate<ActionAnimationData>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var updateFrameHandle = new UpdateFrameJob
			{
				DeltaTime = SystemAPI.Time.DeltaTime
			}.ScheduleParallel(_query, state.Dependency);

			var updateDirectionHandle = new UpdateAnimationTransformJob().ScheduleParallel(_moveQuery, state.Dependency);

			state.Dependency = JobHandle.CombineDependencies(updateFrameHandle, updateDirectionHandle);
		}

		[BurstCompile]
		private partial struct UpdateFrameJob : IJobEntity
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

		[BurstCompile]
		private partial struct UpdateAnimationTransformJob : IJobEntity
		{
			private void Execute(in LocalToWorld ltw, ref AnimationTransformData animationTransform, ref MoveDirection moveDirection)
			{
				ref var matrix = ref animationTransform.Martix;
				matrix.SetColumn(3, ltw.Value.c3); //position

				float curDirX = moveDirection.Value.x;
				if (math.abs(curDirX) > 0.01f)
				{
					float sign = math.sign(curDirX);
					matrix.m00 = animationTransform.IsFaceLeftDefault ? -sign : sign;
				}
			}
		}
	}
}
