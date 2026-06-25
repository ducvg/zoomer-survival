using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
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

				AddComponent<MoveDirection>(entity);
				AddComponent(entity, new MoveSpeed
				{
					Value = authoring._moveSpeed
				});
			}
		}
	}
}
