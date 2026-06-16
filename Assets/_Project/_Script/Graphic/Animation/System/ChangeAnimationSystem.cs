using Unity.Entities;

namespace Zoomer.Graphic.Animation
{
	[UpdateBefore(typeof(FrameSimulationSystem))]
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial struct ChangeAnimationSystem : ISystem
	{
		public void OnCreate(ref SystemState state)
		{
		}

		public void OnUpdate(ref SystemState state)
		{
			foreach (var (animData, changeData) in SystemAPI.Query<RefRW<ActionAnimationData>, RefRO<ChangeAnimationData>>())
			{

			}
		}
	}
}
