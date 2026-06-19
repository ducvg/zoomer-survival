using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Zoomer.Graphic.Animation
{
	using NativeCharacterAnimationData = NativeAnimationStorageData.NativeCharacterAnimationData;

	[UpdateAfter(typeof(InitializeSpriteGraphicSystem))]
	[UpdateInGroup(typeof(InitializationSystemGroup))]
	public partial class InitializeAnimationSystem : SystemBase
	{
		private NativeAnimationStorageData _nativeStorage;
		private DrawFrameData _drawData;

		protected override void OnCreate()
		{
			CreateNativeAnimationStorage();
			_drawData = new()
			{
				DrawBatches = new NativeHashMap<DrawFrameData.DrawBatch, NativeList<Matrix4x4>>(512, Allocator.Persistent)
			};
			// EntityManager.CreateSingleton(_drawData);
		}

		protected override void OnDestroy()
		{
			DiposeNativeAnimationStorage();
			// _drawData.DrawBatches.Dispose();
		}

		private void CreateNativeAnimationStorage()
		{
			int charAnimCount = AnimationStorageSO.CharAnimationCount;
			var charAnimMap = new NativeHashMap<EntityId, NativeCharacterAnimationData>(charAnimCount, Allocator.Persistent);
			foreach (var (configId, charAnimConfig) in AnimationStorageSO.CharAnimConfigDict)
			{
				int actionCount = charAnimConfig.Actions.Length;
				var charAnim = new NativeCharacterAnimationData();
				charAnim.Actions = new(actionCount, Allocator.Persistent);
				for (int i = 0; i < actionCount; ++i)
				{
					var actionConfig = charAnimConfig.Actions[i];
					int frameCount = actionConfig.Frames.Length;
					charAnim.Actions[i] = new NativeActionAnimationData()
					{
						Fps = actionConfig.Fps,
						FrameCount = frameCount
					};
					_drawData.MaxBatchCount += frameCount;
				}
				charAnimMap[configId] = charAnim;
			}
			_nativeStorage = new NativeAnimationStorageData
			{
				Characters = charAnimMap
			};

			EntityManager.CreateSingleton(_nativeStorage);
		}
		private void DiposeNativeAnimationStorage()
		{
			using var kv = _nativeStorage.Characters.GetValueArray(Allocator.Temp);
			for (int i = 0; i < kv.Length; i++)
			{
				kv[i].Actions.Dispose();
			}
			_nativeStorage.Characters.Dispose();
		}

		protected override void OnUpdate() { }
	}
}
