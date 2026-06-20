using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Zoomer.Graphic.Animation
{

	public struct ChangeAnimationTag : IComponentData, IEnableableComponent { }
	public struct ChangeAnimationData : IComponentData
	{
		public ActionKind NewAction;
		// public ActionKind PrevAction;
	}

	public struct DrawFrameData : IComponentData //idk
	{
		public NativeHashMap<DrawBatch, NativeList<Matrix4x4>> DrawBatches;
		public int MaxBatchCount;

		public struct DrawBatch : IEquatable<DrawBatch>
		{
			public EntityId CharConfigId;
			public ActionKind ActionKind;
			public int FrameIndex;

			public bool Equals(DrawBatch other) => CharConfigId == other.CharConfigId && ActionKind == other.ActionKind && FrameIndex == other.FrameIndex;
		}
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
