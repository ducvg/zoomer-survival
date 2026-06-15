using Unity.Entities;
using UnityEngine;

namespace Zoomer.Graphic
{
    public sealed class SpriteGraphicAuthoring : MonoBehaviour
    {
		private class SpriteGraphicAuthoringBaker : Baker<SpriteGraphicAuthoring>
		{
			public override void Bake(SpriteGraphicAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.Renderable);

				AddComponent<SpriteGraphicRef>(entity);
				AddComponent<SpawnCharacterGraphicTag>(entity);
			}
		}
    }

    
}
