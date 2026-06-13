using Unity.Entities;

namespace Zoomer.Animation
{
    public struct AnimationData : IComponentData
    {
		public UnityObjectRef<AnimationStorageSO> AnimationStorage; //int
    }
}
