using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
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
			// RequireForUpdate<DrawFrameData>();
		}

		protected override void OnUpdate()
		{
			var charAnimConfigDict = AnimationStorageSO.CharAnimConfigDict;
			RenderParams rp = new(AnimationStorageSO.SharedMaterial);
			foreach (var (animData, actionAnimData, ltw) in SystemAPI
				.Query<RefRO<CharacterAnimationData>, RefRO<ActionAnimationData>, RefRO<LocalToWorld>>())
			{
				var animConfigId = animData.ValueRO.AnimationConfigId;
				var actionData = actionAnimData.ValueRO;
				Sprite sprite = charAnimConfigDict[animConfigId][actionData.CurrentAction].Frames[actionData.FrameIndex];
				SpriteParams sp = new(sprite);
				Graphics.RenderSprite(rp, sp, submeshIndex: 0, ltw.ValueRO.Value);
			}
		}

		// protected void OnUpdateInstancing()
		// {
		// 	var drawData = SystemAPI.GetSingleton<DrawFrameData>();
		// 	var drawBatches = drawData.DrawBatches;

		// 	var charAnimConfigDict = AnimationStorageSO.CharAnimConfigDict;
		// 	RenderParams rp = new(AnimationStorageSO.SharedMaterial);
		// 	foreach (var batch in drawBatches)
		// 	{
		// 		var batchData = batch.Key;
		// 		var positions = batch.Value.AsArray();

		// 		Sprite batchSprite = charAnimConfigDict[batchData.CharConfigId][batchData.ActionKind].Frames[batchData.FrameIndex];
		// 		SpriteParams sp = new(batchSprite);

		// 		Graphics.RenderSpriteInstanced(rp, sp, submeshIndex: 0, positions);
		// 	}
		// }
	}
}
