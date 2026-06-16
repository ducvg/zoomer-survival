using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Zoomer.Graphic.Animation
{
	public struct CharacterAnimationData : IComponentData
	{
		public EntityId AnimationStorageId;
	}

	public struct ActionAnimationData : IComponentData
	{
		public ActionKind CurrentAction;
		public int FrameIndex;
		public NativeActionAnimationData NativeData;
	}

	public struct NativeActionAnimationData
	{
		public int FrameCount;
		public int Fps;
	}

	public struct NativeAnimationStorageData : IComponentData
	{
		public NativeHashMap<EntityId, NativeCharacterAnimationData> Characters;

		public struct NativeCharacterAnimationData
		{
			public NativeArray<NativeActionAnimationData> Actions;
		}
	}

	public struct ChangeAnimationData : IComponentData
	{
		public ActionKind NewAction;
		// public ActionKind PrevAction;
	}
}
