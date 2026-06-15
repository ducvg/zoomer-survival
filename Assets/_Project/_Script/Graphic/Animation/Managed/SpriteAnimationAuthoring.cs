using Unity.Entities;
using UnityEngine;

namespace Zoomer.Graphic.Animation
{
	[RequireComponent(typeof(SpriteGraphicAuthoring))]
	public sealed class SpriteAnimationAuthoring : MonoBehaviour
	{
		[SerializeField] private AnimationStorageSO _characterAnimationStorage;

		private sealed class AnimationBaker : Baker<SpriteAnimationAuthoring>
		{
			public override void Bake(SpriteAnimationAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.Renderable);
				AddComponent(entity, new AnimationData
				{
					AnimationStorage = authoring._characterAnimationStorage
				});
			}
		}
	}
}
