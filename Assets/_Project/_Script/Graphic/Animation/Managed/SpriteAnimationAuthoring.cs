using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Zoomer.Graphic.Animation
{
	public sealed class SpriteAnimationAuthoring : MonoBehaviour
	{
		[SerializeField] private CharacterAnimationConfigSO _characterAnimationStorage;
		[SerializeField] private ActionKind _defaultAnimation;

		private sealed class AnimationBaker : Baker<SpriteAnimationAuthoring>
		{
			public override void Bake(SpriteAnimationAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.Renderable);

				AddComponent(entity, new CharacterAnimationData
				{
					AnimationConfigId = authoring._characterAnimationStorage.GetEntityId()
				});
				AddComponent(entity, new ActionAnimationData
				{
					CurrentAction = authoring._defaultAnimation,
					NativeData = new NativeActionAnimationData
					{
						FrameCount = authoring._characterAnimationStorage[authoring._defaultAnimation].Frames.Length,
						Fps = authoring._characterAnimationStorage[authoring._defaultAnimation].Fps
					}
				});
				AddComponent<ChangeAnimationData>(entity);
				// AddComponent<SpawnCharacterGraphicTag>(entity);
				SetComponentEnabled<ChangeAnimationData>(entity, false);
			}
		}
	}
}
