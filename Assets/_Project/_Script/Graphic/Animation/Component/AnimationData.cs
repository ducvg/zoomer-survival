using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Zoomer.Graphic.Animation
{
	public struct AnimationTransformData : IComponentData
	{
		public bool IsDefaultFlipX;
		public Matrix4x4 Martix;
	}

	public struct ChangeAnimationData : IComponentData, IEnableableComponent
	{
		public ActionKind NewAction;
		// public ActionKind PrevAction;
	}

	public struct AnimationData : IComponentData
	{
		public EntityId AnimationConfigId;
	}

	public struct ActionAnimationData : IComponentData
	{
		public ActionKind CurrentAction;
		public float FrameTimer;
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
}
