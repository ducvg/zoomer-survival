using Unity.Entities;

namespace Zoomer.Graphic.Animation
{
    public struct AnimationData : IComponentData
    {
		public UnityObjectRef<AnimationStorageSO> AnimationStorage;
    }
}
