using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Jobs;

namespace Zoomer.Animation
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct SyncCharacterGraphicSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
			var storage = SystemAPI.GetSingleton<CharacterGraphicStorageData>();
			var graphicTransformArray = storage.TransformAccessArray;
			NativeArray<float3> entityPosition = new(graphicTransformArray.length, Allocator.TempJob);

			var fetchJob = new FetchTransformPositionJob {
                positions = entityPosition,
            };
            state.Dependency = fetchJob.ScheduleParallel(state.Dependency);

            var syncJob = new SyncPositionToTransformJob {
                positions = entityPosition.AsReadOnly(),
            };
            state.Dependency = syncJob.Schedule(graphicTransformArray, state.Dependency);

			state.Dependency = entityPosition.Dispose(state.Dependency);
        }

		[BurstCompile]
        private partial struct FetchTransformPositionJob : IJobEntity
        {
            [NativeDisableParallelForRestriction] public NativeArray<float3> positions;

            private void Execute(in CharacterGraphicRef graphicRef, in LocalToWorld localToWorld)
            {
                positions[graphicRef.DataIndex] = localToWorld.Position;
            }
        }

        [BurstCompile]
        private partial struct SyncPositionToTransformJob : IJobParallelForTransform
        {
            [ReadOnly] public NativeArray<float3>.ReadOnly positions;

            [BurstCompile]
            public void Execute(int index, TransformAccess transform)
            {
                transform.position = positions[index];
            }
        }
    }
}
