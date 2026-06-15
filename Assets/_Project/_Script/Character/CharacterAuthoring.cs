using Unity.Entities;
using UnityEngine;
using Zoomer.Animation;

namespace Zoomer
{
    public sealed class CharacterAuthoring : MonoBehaviour
    {
		[SerializeField] private AnimationStorageSO _animationStorage;
		[SerializeField] private float _moveSpeed = 5f;
	
		class CharacterBaker : Baker<CharacterAuthoring>
		{
			public override void Bake(CharacterAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				
				if(authoring._animationStorage) AddComponent<AnimationData>(entity);
				
				AddComponent<CharacterGraphicRef>(entity);
				AddComponent<CreateCharacterGraphicTag>(entity);

				AddComponent<CharacterMoveDirection>(entity);
				AddComponent(entity, new CharacterMoveSpeed
				{
					Value = authoring._moveSpeed
				});

			}
		}
    }
}
