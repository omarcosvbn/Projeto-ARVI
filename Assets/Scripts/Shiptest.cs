using UnityEngine;
using UnityEngine.InputSystem;

public class ActionRotator : MonoBehaviour {
    [SerializeField] private InputActionReference yawLeft;
    [SerializeField] private InputActionReference yawRight;
    [SerializeField] private InputActionReference pitchUp;
    [SerializeField] private InputActionReference pitchDown;
    [SerializeField] private float speed = 45f;

    private void OnEnable() {
        yawLeft.action.Enable();
        yawRight.action.Enable();
        pitchUp.action.Enable();
        pitchDown.action.Enable();
    }

    private void Update() {
        float yaw = Read(yawRight) - Read(yawLeft);
        float pitch = Read(pitchDown) - Read(pitchUp);

        transform.Rotate(Vector3.up, yaw * speed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.right, pitch * speed * Time.deltaTime, Space.Self);
    }

    private float Read(InputActionReference reference) {
        return reference.action.ReadValue<float>();
    }
}