using Unity.Entities;
using Unity.Mathematics;

namespace Zoomer
{
	public struct CharacterMoveDirection : IComponentData
	{
		public float2 Value;
	}

	public struct CharacterMoveSpeed : IComponentData
	{
		public float Value;
	}
}
