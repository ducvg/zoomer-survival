using Unity.Collections;
using Unity.Entities;
using UnityEngine.Jobs;

namespace Zoomer.Animation
{
	public struct CharacterGraphicStorageData : IComponentData
	{
		public TransformAccessArray TransformAccessArray;
		public NativeList<Entity> EntityGraphicList;
	}

    public struct CharacterGraphicRef : IComponentData
    {
		public UnityObjectRef<CharacterGraphic> Value;
		public int DataIndex;
    }
}
