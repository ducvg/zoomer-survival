using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Zoomer.Graphic.Animation
{
	using DrawBatch = DrawFrameData.DrawBatch;

	[UpdateAfter(typeof(FrameSimulationSystem))]
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial class DrawFrameSystem : SystemBase
	{

		protected override void OnCreate()
		{
			RequireForUpdate<DrawFrameData>();
		}

		protected override void OnUpdate()
		{
			var drawData = SystemAPI.GetSingleton<DrawFrameData>();
			var drawBatches = drawData.DrawBatches; //multihashmap<DrawBatch, Matrix4x4>
			var drawBatchSet = drawData.DrawBatchSet.ToNativeArray(WorldUpdateAllocator); //hashset<DrawBatch>
			var batchCount = drawBatchSet.Length;
			if (batchCount <= 0) return;

			var batchPositionsArray = CollectionHelper.CreateNativeArray<NativeArray<Matrix4x4>>(batchCount, WorldUpdateAllocator);
			var jobs = CollectionHelper.CreateNativeArray<JobHandle>(batchCount, WorldUpdateAllocator);
			for (int i = 0; i < batchCount; ++i)
			{
				var batchData = drawBatchSet[i];
				batchPositionsArray[i] = CollectionHelper.CreateNativeArray<Matrix4x4>(drawBatches.CountValuesForKey(batchData), WorldUpdateAllocator);

				jobs[i] = new InitalizePositionJob
				{
					UpdateAllocator = WorldUpdateAllocator,
					DrawBatchData = batchData,
					DrawBatches = drawBatches,
					BatchPositions = batchPositionsArray[i]
				}.Schedule(Dependency);
			}
			Dependency = JobHandle.CombineDependencies(jobs);
			Dependency.Complete();

			Material mat = AnimationStorageSO.SharedMaterial;
			var charAnimConfigDict = AnimationStorageSO.CharAnimConfigDict;
			for (int i = 0; i < batchCount; ++i)
			{
				var batchData = drawBatchSet[i];
				Sprite batchSprite = charAnimConfigDict[batchData.CharConfigId][batchData.ActionKind].Frames[batchData.FrameIndex];

				RenderParams rp = new(mat);
				SpriteParams sp = new(batchSprite);
				Graphics.RenderSpriteInstanced(rp, sp, submeshIndex: 0, batchPositionsArray[i]);
			}
		}

		[BurstCompile]
		private struct InitalizePositionJob : IJob
		{
			[ReadOnly] public Allocator UpdateAllocator;
			[ReadOnly] public DrawBatch DrawBatchData;
			[ReadOnly] public NativeParallelMultiHashMap<DrawBatch, Matrix4x4> DrawBatches;
			[WriteOnly] public NativeArray<Matrix4x4> BatchPositions;

			public void Execute()
			{
				bool isEmpty = !DrawBatches.TryGetFirstValue(key: DrawBatchData, out Matrix4x4 value, out var iter);
				if (Hint.Unlikely(isEmpty)) return;

				int i = 0;
				do
				{
					BatchPositions[i] = value;
				}
				while (DrawBatches.TryGetNextValue(out value, ref iter));
			}
		}
	}
}
