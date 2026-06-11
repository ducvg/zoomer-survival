using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

namespace Zoomer
{
	[WorldSystemFilter(WorldSystemFilterFlags.BakingSystem)]
    public partial struct CharacterPhysicBakingSystem : ISystem
    {
        [BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			// disable rotation for all characters
			foreach (var mass in SystemAPI.Query<RefRW<PhysicsMass>>().WithAll<CharacterMoveDirection>())
			{
				mass.ValueRW.InverseInertia = float3.zero;
			}
		}
    }
}
