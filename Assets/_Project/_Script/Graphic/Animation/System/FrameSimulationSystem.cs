using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
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

			var updateDirectionHandle = new UpdateAnimationDirectionJob().ScheduleParallel(_moveQuery, state.Dependency);

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
		private partial struct UpdateAnimationDirectionJob : IJobEntity
		{
			private void Execute(in LocalToWorld ltw, ref AnimationTransformData animationTransform, ref MoveDirection moveDirection)
			{
				var matrix = ltw.Value;

				if (moveDirection.Value.x < -0.01f) matrix.c0 *= animationTransform.IsDefaultFlipX ? 1f : -1f;

				animationTransform.Martix = matrix;
			}
		}
	}
}
