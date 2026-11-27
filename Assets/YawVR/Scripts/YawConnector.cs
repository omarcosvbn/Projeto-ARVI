using UnityEngine;
using YawVR;

public class YawConnector : MonoBehaviour {
    [SerializeField] private Transform vehicle;
    [SerializeField] private float rollPerSpeed = 5f;
    [SerializeField] private float maxRoll = 15f;
    [SerializeField] private float rollSmoothing = 5f;
    [SerializeField] private float pitchReturn = 4f;

    private Vector3 lastPosition;
    private float roll;
    private float pitch;

    public void Kick(float angle) {
        pitch = Mathf.Max(pitch, angle);
    }

    void Start() {
        lastPosition = vehicle.position;
    }

    void Update() {
        Vector3 velocity = (vehicle.position - lastPosition) / Time.deltaTime;
        lastPosition = vehicle.position;

        // Bank into the direction of sideways movement
        float sideSpeed = Vector3.Dot(velocity, vehicle.right);
        float targetRoll = Mathf.Clamp(-sideSpeed * rollPerSpeed, -maxRoll, maxRoll);
        roll = Mathf.Lerp(roll, targetRoll, rollSmoothing * Time.deltaTime);
        pitch = Mathf.Lerp(pitch, 0f, pitchReturn * Time.deltaTime);

        YawController.Instance.Tracker.SetRotation(vehicle.eulerAngles + new Vector3(-pitch, 0f, roll));
    }
}
