using Unity.Entities;
using Unity.Mathematics;

namespace Zoomer.Input
{
	public struct InputData : IComponentData
	{
		public float2 MoveDirection;
	}
}
