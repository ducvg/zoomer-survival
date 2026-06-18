using Unity.Entities;

namespace Zoomer.Graphic.Animation
{
	[UpdateBefore(typeof(FrameSimulationSystem))]
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial struct ChangeAnimationSystem : ISystem
	{
		private EntityQuery _query;

		public void OnCreate(ref SystemState state)
		{
			_query = SystemAPI.QueryBuilder()
				.WithAllRW<ChangeAnimationData>()
				.WithAllRW<ActionAnimationData>()
				.Build();

			state.RequireForUpdate(_query);
			state.RequireForUpdate<NativeAnimationStorageData>();
		}

		public void OnUpdate(ref SystemState state)
		{
			var storage = SystemAPI.GetSingleton<NativeAnimationStorageData>();


		}
	}
}
