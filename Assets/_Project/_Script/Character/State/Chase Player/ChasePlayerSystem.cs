using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Zoomer
{
	partial struct FindNearestPlayerSystem : ISystem
	{
		private EntityQuery _playerQuery, _chaseQuery;

		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			_playerQuery = SystemAPI.QueryBuilder()
				.WithAll<LocalTransform>()
				.WithAll<PlayerTag>()
				.Build();

			_chaseQuery = SystemAPI.QueryBuilder()
				.WithAll<ChaseStateData>()
				.Build();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{

		}
	}
}
