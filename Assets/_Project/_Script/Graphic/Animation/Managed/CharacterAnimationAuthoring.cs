using Unity.Entities;
using UnityEngine;

namespace Zoomer.Graphic.Animation
{
	public sealed class CharacterAnimationAuthoring : MonoBehaviour
	{
		[SerializeField] private bool _DefaultFlipX;
		[SerializeField] private CharacterAnimationSO _characterAnimation;
		[SerializeField] private ActionKind _defaultAnimation;

		private sealed class AnimationBaker : Baker<CharacterAnimationAuthoring>
		{
			public override void Bake(CharacterAnimationAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.Renderable);
				AddComponent(entity, new AnimationTransformData()
				{
					IsDefaultFlipX = authoring._DefaultFlipX
				});
				AddComponent(entity, new AnimationData
				{
					AnimationConfigId = authoring._characterAnimation.GetEntityId()
				});
				AddComponent(entity, new ActionAnimationData
				{
					CurrentAction = authoring._defaultAnimation,
					NativeData = new NativeActionAnimationData
					{
						FrameCount = authoring._characterAnimation[authoring._defaultAnimation].Frames.Length,
						Fps = authoring._characterAnimation[authoring._defaultAnimation].Fps
					}
				});
				AddComponent<ChangeAnimationData>(entity);
				SetComponentEnabled<ChangeAnimationData>(entity, false);

				// AddComponent<SpawnCharacterGraphicTag>(entity);
			}
		}
	}
}
