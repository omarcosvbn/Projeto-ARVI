using UnityEngine;

public class Shiptest : MonoBehaviour {
    [SerializeField] private Transform movementStick;
    [SerializeField, Range(0f, 180f)] private float maxStickAngle = 30f;
    [SerializeField, Min(0f)] private float speed = 2f;

    private void Update() {
        if (movementStick == null || maxStickAngle <= 0f) {
            return;
        }

        float stickAngle = Mathf.DeltaAngle(0f, movementStick.localEulerAngles.z);
        float horizontalInput = Mathf.Clamp(stickAngle / maxStickAngle, -1f, 1f);
        transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime, Space.World);
    }

    private void OnValidate() {
        maxStickAngle = Mathf.Clamp(maxStickAngle, 0f, 180f);
    }
}
