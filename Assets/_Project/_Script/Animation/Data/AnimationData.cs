using Unity.Entities;

namespace Zoomer.Animation
{
    public struct AnimationData : IComponentData
    {
		public UnityObjectRef<AnimationStorageSO> AnimationStorage;
    }

	public struct CharacterGraphicData : IComponentData
	{
		public Entity CharacterGraphicEntity;
	}
}
