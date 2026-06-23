using Unity.Entities;
using UnityEngine;

namespace Zoomer.Graphic.Animation
{
	public sealed class CharacterAnimationAuthoring : MonoBehaviour
	{
		[SerializeField] private CharacterAnimationSO _characterAnimationStorage;
		[SerializeField] private ActionKind _defaultAnimation;

		private sealed class AnimationBaker : Baker<CharacterAnimationAuthoring>
		{
			public override void Bake(CharacterAnimationAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.Dynamic); //need localToWorld for draw position

				AddComponent(entity, new AnimationData
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
				SetComponentEnabled<ChangeAnimationData>(entity, true);
				// AddComponent<SpawnCharacterGraphicTag>(entity);
			}
		}
	}
}
