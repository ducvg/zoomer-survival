using Unity.Entities;
using UnityEngine;
using Zoomer.Graphic;
using Zoomer.Graphic.Animation;

namespace Zoomer
{
    public sealed class CharacterAuthoring : MonoBehaviour
    {
		[SerializeField] private float _moveSpeed = 5f;
	
		private sealed class CharacterBaker : Baker<CharacterAuthoring>
		{
			public override void Bake(CharacterAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				
				AddComponent<CharacterMoveDirection>(entity);
				AddComponent(entity, new CharacterMoveSpeed
				{
					Value = authoring._moveSpeed
				});
			}
		}
    }
}
