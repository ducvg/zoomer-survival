using Unity.Collections;
using Unity.Entities;

namespace Zoomer.Animation
{
	public partial class DestroyCharacterGraphicSystem : SystemBase
	{
		protected override void OnCreate()
		{
			RequireForUpdate<DestroyCharacterGraphicTag>();
		}

		protected override void OnUpdate()
		{
			var storage = SystemAPI.GetSingleton<CharacterGraphicStorageData>();
			var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>()
				.CreateCommandBuffer(World.Unmanaged);
			foreach (var (graphicRef, entity) in SystemAPI
				.Query<RefRO<CharacterGraphicRef>>()
				.WithAll<DestroyCharacterGraphicTag>()
				.WithEntityAccess())
			{
				CharacterGraphicFactory.Return(graphicRef.ValueRO.Value);
				int removeIndex = graphicRef.ValueRO.DataIndex;
				HandleRemoveSwapback(removeIndex, ref storage);

				ecb.RemoveComponent<DestroyCharacterGraphicTag>(entity);
			}
		}

		private void HandleRemoveSwapback(int removeIndex, ref CharacterGraphicStorageData storage)
		{
			var transformArray = storage.TransformAccessArray;
			var entityList = storage.EntityGraphicList;

			int lastIndex = entityList.Length - 1;

			if (removeIndex < 0 || removeIndex > lastIndex) return;

			transformArray.RemoveAtSwapBack(removeIndex);
			entityList.RemoveAtSwapBack(removeIndex);

			if (removeIndex != lastIndex)
			{
				var graphicref = SystemAPI.GetComponentRW<CharacterGraphicRef>(entityList[removeIndex]);
				graphicref.ValueRW.DataIndex = removeIndex;
			}
		}
	}
}
