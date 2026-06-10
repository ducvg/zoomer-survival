using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

namespace Zoomer
{
	[UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct CharacterMoverSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
			state.RequireForUpdate<CharacterMoveDirection>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
			foreach (var (moveDirection, moveSpeed, velocity) in SystemAPI.Query<RefRO<CharacterMoveDirection>, RefRO<CharacterMoveSpeed>, RefRW<PhysicsVelocity>>())
			{
				var moveDelta = moveDirection.ValueRO.Value * moveSpeed.ValueRO.Value;
				velocity.ValueRW.Linear = new float3(moveDelta, 0);
			}
        }
    }
}
