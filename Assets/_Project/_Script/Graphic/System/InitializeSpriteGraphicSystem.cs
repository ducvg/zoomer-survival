using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Zoomer.Graphic
{
	[UpdateInGroup(typeof(InitializationSystemGroup))]
	public partial struct InitializeSpriteGraphicSystem : ISystem
	{
		private SpriteGraphicStorageData _storage;

		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			_storage = new SpriteGraphicStorageData
			{
				TransformAccessArray = new(256),
				EntityGraphicList = new(256, Allocator.Persistent)
			};
			state.EntityManager.CreateSingleton(_storage);
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state)
		{
			_storage.TransformAccessArray.Dispose();
			_storage.EntityGraphicList.Dispose();
		}
	}
}
