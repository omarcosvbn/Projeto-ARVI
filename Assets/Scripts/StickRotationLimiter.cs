using UnityEngine;
using UnityEngine.InputSystem;

public class StickRotationLimiter : MonoBehaviour {
    [SerializeField] private InputAction gripAction;
    [SerializeField, Range(0f, 1f)] private float gripThreshold = 0.1f;
    [SerializeField, Range(0f, 180f)] private float maxAngle = 10f;

    private void OnEnable() {
        gripAction.Enable();
    }

    private void OnDisable() {
        gripAction.Disable();
    }

    private void LateUpdate() {
        if (gripAction.ReadValue<float>() < gripThreshold) {
            transform.localRotation = Quaternion.identity;
            return;
        }

        Vector3 up = transform.parent.InverseTransformDirection(transform.up);
        float zAngle = Mathf.Atan2(up.x, up.y) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Clamp(zAngle, -maxAngle, maxAngle));
    }
}
