using UnityEngine;

public class StickRotationLimiter : MonoBehaviour {
    [SerializeField, Range(0f, 180f)] private float maxAngle = 30f;

    private void LateUpdate() {
        Vector3 localEulerAngles = transform.localEulerAngles;
        float zAngle = Mathf.DeltaAngle(0f, localEulerAngles.z);
        float clampedZAngle = Mathf.Clamp(zAngle, -maxAngle, maxAngle);

        transform.localRotation = Quaternion.Euler(0f, 0f, clampedZAngle);
    }

    private void OnValidate() {
        maxAngle = Mathf.Clamp(maxAngle, 0f, 180f);
    }
}
