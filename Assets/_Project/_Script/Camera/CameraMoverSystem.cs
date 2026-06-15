using Unity.Entities;
using Unity.Transforms;

namespace Zoomer.Camera
{
	[UpdateAfter(typeof(TransformSystemGroup))]
    public sealed partial class CameraMoverSystem : SystemBase
    {
		protected override void OnUpdate()
		{
			var mainCamTarget = CameraManager.Instance.MainCamTarget;
			foreach (var transform in SystemAPI.Query<RefRO<LocalToWorld>>().WithAll<PlayerTag>())
			{
				mainCamTarget.position = transform.ValueRO.Position;
			}
		}
	}
}
