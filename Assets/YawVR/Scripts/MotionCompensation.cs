using UnityEngine;

namespace YawVR {
    public class MotionCompensation : MonoBehaviour {
        [SerializeField] private Transform cameraOffsetTransform;
        [SerializeField] private YawController yawController;
        [SerializeField, Range(0f, 0.9f)] private float smoothing = 0.7f;

        private float yaw;
        private float offset;

        public void UpdateOffset() {
            offset = yaw;
        }

        void Update() {
            if (yawController.State < ControllerState.Connected) {
                return;
            }
            yaw = -yawController.DeviceYaw;
            cameraOffsetTransform.rotation = Quaternion.Slerp(cameraOffsetTransform.rotation, Quaternion.Euler(0f, yaw - offset, 0f), 1f - smoothing);
        }
    }
}
