using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

namespace Zoomer
{
	[UpdateInGroup(typeof(SimulationSystemGroup))]
	public partial struct CharacterMoverSystem : ISystem
	{
		private EntityQuery _moveQuery;

		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			_moveQuery = SystemAPI.QueryBuilder()
				.WithAll<PhysicsVelocity, MoveDirection, MoveSpeed>()
				.Build();
			state.RequireForUpdate<MoveDirection>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var job = new MoveVelocityJob();
			state.Dependency = job.ScheduleParallel(_moveQuery, state.Dependency);
		}

		[BurstCompile]
		private partial struct MoveVelocityJob : IJobEntity
		{
			public void Execute(ref PhysicsVelocity velocity, in MoveDirection moveDirection, in MoveSpeed moveSpeed)
			{
				var moveDelta = moveDirection.Value * moveSpeed.Value;
				velocity.Linear = new float3(moveDelta, 0);
			}
		}
	}
}
