using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Zoomer.Graphic.Animation
{
	[RequireComponent(typeof(SpriteGraphicAuthoring))]
	public sealed class SpriteAnimationAuthoring : MonoBehaviour
	{
		[SerializeField] private CharacterAnimationConfigSO _characterAnimationStorage;

		private sealed class AnimationBaker : Baker<SpriteAnimationAuthoring>
		{
			public override void Bake(SpriteAnimationAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.Renderable);

				AddComponent(entity, new CharacterAnimationData
				{
					AnimationStorageId = authoring._characterAnimationStorage.GetEntityId()
				});
				AddComponent(entity, new ActionAnimationData
				{
					CurrentAction = ActionKind.Idle
				});
				AddComponent(entity, new ChangeAnimationData
				{
					NewAction = ActionKind.Idle
				});
			}
		}
	}
}
