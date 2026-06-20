using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Zoomer.Graphic.Animation
{
	[UpdateBefore(typeof(FrameSimulationSystem))]
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial struct ChangeAnimationSystem : ISystem
	{
		private EntityQuery _query;

		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			_query = SystemAPI.QueryBuilder()
				.WithAll<AnimationData>()
				.WithAllRW<ChangeAnimationData>() //enabled
				.WithAllRW<ActionAnimationData>()
				.Build();

			state.RequireForUpdate(_query);
			state.RequireForUpdate<NativeAnimationStorageData>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var storage = SystemAPI.GetSingleton<NativeAnimationStorageData>();

			state.Dependency = new ChangeAnimationJob
			{
				CharactersAnimation = storage.Characters.AsReadOnly()
			}.ScheduleParallel(_query, state.Dependency);
		}

		[BurstCompile]
		private partial struct ChangeAnimationJob : IJobEntity
		{
			[ReadOnly, NativeDisableContainerSafetyRestriction]
			public NativeHashMap<EntityId, NativeAnimationStorageData.NativeCharacterAnimationData>.ReadOnly CharactersAnimation;

			private void Execute(in AnimationData animationData, ref ActionAnimationData actionData,
				EnabledRefRW<ChangeAnimationData> changeDataTag, ref ChangeAnimationData changeData)
			{
				var charAnimData = CharactersAnimation[animationData.AnimationConfigId];
				var nativeActionData = charAnimData.Actions[(byte)changeData.NewAction];

				actionData = new ActionAnimationData
				{
					CurrentAction = changeData.NewAction,
					FrameIndex = 0,
					FrameTimer = 0,
					NativeData = nativeActionData
				};

				changeDataTag.ValueRW = false;
			}
		}
	}
}
