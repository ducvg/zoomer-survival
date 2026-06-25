using Unity.Entities;
using Unity.Mathematics;

namespace Zoomer
{
	public struct MoveDirection : IComponentData
	{
		public float2 Value;
	}

	public struct MoveSpeed : IComponentData
	{
		public float Value;
	}
}
