using Unity.Cinemachine;
using UnityEngine;

namespace Zoomer.Camera
{
    public sealed class CameraManager : Singleton<CameraManager>
    {
        [field: SerializeField] public CinemachineCamera MainCinemachineCam { get; private set; }
		[field: SerializeField] public Transform MainCamTarget { get; private set; }
    }
}
