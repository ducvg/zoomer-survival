using Unity.Collections;
using Unity.Entities;

namespace Zoomer.Graphic
{
	public partial class SpawnSpriteGraphicSystem : SystemBase
	{
		protected override void OnCreate()
		{
			var storage = new SpriteGraphicStorageData
			{
				TransformAccessArray = new(256),
				EntityGraphicList = new(256, Allocator.Persistent)
			};
			EntityManager.CreateSingleton(storage);

			RequireForUpdate<SpawnCharacterGraphicTag>();
		}

		protected override void OnUpdate()
		{
			int index = 0;
			var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>()
				.CreateCommandBuffer(World.Unmanaged);
			var graphicStorage = SystemAPI.GetSingleton<SpriteGraphicStorageData>();

			foreach (var (graphicRef, entity) in SystemAPI
				.Query<RefRW<SpriteGraphicRef>>()
				.WithAll<SpawnCharacterGraphicTag>()
				.WithEntityAccess())
			{
				var graphic = SpriteGraphicFactory.Create();

				graphicRef.ValueRW.Value = graphic;
				graphicRef.ValueRW.DataIndex = index;

				graphicStorage.TransformAccessArray.Add(graphic.transform);
				graphicStorage.EntityGraphicList.Add(entity);

				ecb.RemoveComponent<SpawnCharacterGraphicTag>(entity);
				++index;
			}
		}

		protected override void OnDestroy()
		{
			if (SystemAPI.TryGetSingleton<SpriteGraphicStorageData>(out var storage))
			{
				storage.TransformAccessArray.Dispose();
				storage.EntityGraphicList.Dispose();
			}
		}
	}
}
