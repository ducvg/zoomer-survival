using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Zoomer.Graphic.Animation
{
	[UpdateAfter(typeof(FrameSimulationSystem))]
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial class DrawFrameSystem : SystemBase
	{
		protected override void OnUpdate()
		{
			var charAnimConfigDict = AnimationStorageSO.CharAnimConfigDict;
			RenderParams rp = new(AnimationStorageSO.SharedMaterial);
			foreach (var (animData, actionAnimData, transform) in SystemAPI
				.Query<RefRO<AnimationData>, RefRO<ActionAnimationData>, RefRO<AnimationTransformData>>())
			{
				var animConfigId = animData.ValueRO.AnimationConfigId;
				var actionData = actionAnimData.ValueRO;
				Sprite sprite = charAnimConfigDict[animConfigId][actionData.CurrentAction].Frames[actionData.FrameIndex];
				SpriteParams sp = new(sprite);
				Graphics.RenderSprite(rp, sp, submeshIndex: 0, transform.ValueRO.Martix);
			}
		}
	}
}
