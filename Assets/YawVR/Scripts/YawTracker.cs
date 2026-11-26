using UnityEngine;

namespace YawVR {
    public class YawTracker : MonoBehaviour {
        public void SetRotation(Vector3 eulerAngles) {
            transform.eulerAngles = eulerAngles;
        }
    }
}
