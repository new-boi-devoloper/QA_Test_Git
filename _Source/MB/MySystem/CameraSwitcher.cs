using Cinemachine;
using UnityEngine;

namespace MB.MySystem
{
    public class CameraSwitcher : MonoBehaviour
    {
        [field: SerializeField] public CinemachineFreeLook ThirdPersonCamera { get; private set; }
        [field: SerializeField] public CinemachineVirtualCamera RtsCamera { get; private set; }
        [field: SerializeField] public CinemachineVirtualCamera DialogCamera { get; private set; }

        private void Start()
        {
            // Устанавливаем начальную камеру (от третьего лица)
            SwitchToThirdPersonCamera();
        }

        public void SwitchToThirdPersonCamera()
        {
            SetCameraPriority(ThirdPersonCamera, 10);
            SetCameraPriority(RtsCamera, 0);
            SetCameraPriority(DialogCamera, 0);
        }

        public void SwitchToRtsCamera()
        {
            SetCameraPriority(ThirdPersonCamera, 0);
            SetCameraPriority(RtsCamera, 10);
            SetCameraPriority(DialogCamera, 0);
        }

        public void SwitchToDialogCamera()
        {
            SetCameraPriority(ThirdPersonCamera, 0);
            SetCameraPriority(RtsCamera, 0);
            SetCameraPriority(DialogCamera, 10);
        }

        private void SetCameraPriority(CinemachineVirtualCameraBase camera, int priority)
        {
            if (camera != null) camera.Priority = priority;
        }
    }
}