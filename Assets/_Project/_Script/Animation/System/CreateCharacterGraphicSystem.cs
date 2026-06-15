using Unity.Collections;
using Unity.Entities;

namespace Zoomer.Animation
{
	public partial class CreateCharacterGraphicSystem : SystemBase
	{
		protected override void OnCreate()
		{
			var storage = new CharacterGraphicStorageData
			{
				TransformAccessArray = new(256),
				EntityGraphicList = new(256, Allocator.Persistent)
			};
			EntityManager.CreateSingleton(storage);

			RequireForUpdate<CreateCharacterGraphicTag>();
		}

		protected override void OnUpdate()
		{
			int index = 0;
			var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>()
				.CreateCommandBuffer(World.Unmanaged);
			var graphicStorage = SystemAPI.GetSingleton<CharacterGraphicStorageData>();

			foreach (var (graphicRef, entity) in SystemAPI
				.Query<RefRW<CharacterGraphicRef>>()
				.WithAll<CreateCharacterGraphicTag>()
				.WithEntityAccess())
			{
				var graphic = CharacterGraphicFactory.Create();

				graphicRef.ValueRW.Value = graphic;
				graphicRef.ValueRW.DataIndex = index;

				graphicStorage.TransformAccessArray.Add(graphic.transform);
				graphicStorage.EntityGraphicList.Add(entity);

				ecb.RemoveComponent<CreateCharacterGraphicTag>(entity);
				++index;
			}
		}

		protected override void OnDestroy()
		{
			if (SystemAPI.TryGetSingleton<CharacterGraphicStorageData>(out var storage))
			{
				storage.TransformAccessArray.Dispose();
				storage.EntityGraphicList.Dispose();
			}
		}
	}
}
