using UnityEngine;
using UnityEngine.InputSystem;

public class StickRotationLimiter : MonoBehaviour {
    [SerializeField] private InputAction gripAction;
    [SerializeField, Range(0f, 1f)] private float gripThreshold = 0.1f;
    [SerializeField, Range(0f, 180f)] private float maxAngle = 30f;

    private void OnEnable() {
        gripAction?.Enable();
    }

    private void OnDisable() {
        gripAction?.Disable();
    }

    private void LateUpdate() {
        if (gripAction == null || gripAction.ReadValue<float>() < gripThreshold) {
            transform.localRotation = Quaternion.identity;
            return;
        }

        Vector3 localEulerAngles = transform.localEulerAngles;
        float zAngle = -Mathf.DeltaAngle(0f, localEulerAngles.z);
        float clampedZAngle = Mathf.Clamp(zAngle, -maxAngle, maxAngle);

        transform.localRotation = Quaternion.Euler(0f, 0f, clampedZAngle);
    }

    private void OnValidate() {
        gripThreshold = Mathf.Clamp01(gripThreshold);
        maxAngle = Mathf.Clamp(maxAngle, 0f, 180f);
    }
}
