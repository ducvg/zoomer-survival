using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using Zoomer.Camera;

namespace Zoomer
{
    public class PlayerAuthoring : MonoBehaviour
    {
		[SerializeField] private float _moveSpeed = 5f;

		class PlayerBaker : Baker<PlayerAuthoring>
		{
			public override void Bake(PlayerAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				
				AddComponent<PlayerTag>(entity);
				AddComponent<CharacterMoveDirection>(entity);
				AddComponent(entity, new CharacterMoveSpeed
				{
					Value = authoring._moveSpeed
				});

			}
		}
    }
}
