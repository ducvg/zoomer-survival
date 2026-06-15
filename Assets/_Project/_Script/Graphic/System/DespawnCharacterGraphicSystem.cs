using Unity.Entities;

namespace Zoomer.Graphic
{
	public partial class DespawnCharacterGraphicSystem : SystemBase
	{
		protected override void OnCreate()
		{
			RequireForUpdate<DespawnSpriteGraphicTag>();
		}

		protected override void OnUpdate()
		{
			var storage = SystemAPI.GetSingleton<SpriteGraphicStorageData>();
			var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>()
				.CreateCommandBuffer(World.Unmanaged);
			foreach (var (graphicRef, entity) in SystemAPI
				.Query<RefRO<SpriteGraphicRef>>()
				.WithAll<DespawnSpriteGraphicTag>()
				.WithEntityAccess())
			{
				SpriteGraphicFactory.Return(graphicRef.ValueRO.Value);
				int removeIndex = graphicRef.ValueRO.DataIndex;
				HandleRemoveSwapback(removeIndex, ref storage);

				ecb.RemoveComponent<DespawnSpriteGraphicTag>(entity);
			}
		}

		private void HandleRemoveSwapback(int removeIndex, ref SpriteGraphicStorageData storage)
		{
			var transformArray = storage.TransformAccessArray;
			var entityList = storage.EntityGraphicList;

			int lastIndex = entityList.Length - 1;

			if (removeIndex < 0 || removeIndex > lastIndex) return;

			transformArray.RemoveAtSwapBack(removeIndex);
			entityList.RemoveAtSwapBack(removeIndex);

			if (removeIndex != lastIndex)
			{
				var graphicref = SystemAPI.GetComponentRW<SpriteGraphicRef>(entityList[removeIndex]);
				graphicref.ValueRW.DataIndex = removeIndex;
			}
		}
	}
}
