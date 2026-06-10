using Unity.Burst;
using Unity.Entities;

namespace Zoomer.Input
{
    public partial struct InputReaderSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
			var input = SystemAPI.GetSingleton<InputData>();

			foreach (var moveDirection in SystemAPI.Query<RefRW<CharacterMoveDirection>>().WithAll<PlayerTag>())
			{
				moveDirection.ValueRW.Value = input.MoveDirection;
			}
        }
    }
}
