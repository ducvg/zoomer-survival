using Unity.Collections;
using Unity.Entities;
using UnityEngine.Jobs;

namespace Zoomer.Graphic
{
	public struct SpriteGraphicStorageData : IComponentData
	{
		public TransformAccessArray TransformAccessArray;
		public NativeList<Entity> EntityGraphicList;
	}

    public struct SpriteGraphicRef : IComponentData
    {
		public UnityObjectRef<SpriteGraphic> Value;
		public int DataIndex;
    }
}
