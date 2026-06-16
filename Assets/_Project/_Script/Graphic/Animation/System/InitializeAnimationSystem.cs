using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using NativeCharacterAnimationData = Zoomer.Graphic.Animation.NativeAnimationStorageData.NativeCharacterAnimationData;

namespace Zoomer.Graphic.Animation
{
	[UpdateAfter(typeof(InitializeSpriteGraphicSystem))]
	[UpdateInGroup(typeof(InitializationSystemGroup))]
	public partial class InitializeAnimationSystem : SystemBase
	{
		private NativeAnimationStorageData _nativeStorage;

		protected override void OnCreate()
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
					charAnim.Actions[i] = new NativeActionAnimationData()
					{
						Fps = actionConfig.Fps,
						FrameCount = actionConfig.Frames.Length
					};
				}
				charAnimMap[configId] = charAnim;
			}
			_nativeStorage = new NativeAnimationStorageData
			{
				Characters = charAnimMap
			};

			EntityManager.CreateSingleton(_nativeStorage);
		}

		protected override void OnDestroy()
		{
			using var kv = _nativeStorage.Characters.GetKeyValueArrays(Allocator.Temp);
			for (int i = 0; i < kv.Length; i++)
			{
				kv.Values[i].Actions.Dispose();
			}
			_nativeStorage.Characters.Dispose();
		}

		protected override void OnUpdate()
		{

		}
	}
}
