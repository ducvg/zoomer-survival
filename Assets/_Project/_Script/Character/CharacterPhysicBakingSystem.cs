using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Zoomer
{
	[WorldSystemFilter(WorldSystemFilterFlags.BakingSystem)]
	public partial struct CharacterPhysicBakingSystem : ISystem
	{
		private EntityQuery _query;

		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			_query = SystemAPI.QueryBuilder()
				.WithAllRW<LocalTransform>()
				.WithAllRW<PhysicsMass>()
				.WithAllRW<PhysicsVelocity>()
				.Build();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			state.Dependency = new Job().ScheduleParallel(_query, state.Dependency);
		}

		[BurstCompile]
		private partial struct Job : IJobEntity
		{
			private void Execute(ref LocalTransform ltrans, ref PhysicsMass mass, ref PhysicsVelocity velocity)
			{
				ltrans.Rotation = quaternion.identity;
				mass.InverseInertia = float3.zero;
				velocity.Angular = float3.zero;
				velocity.Linear = float3.zero;
			}
		}
	}
}
