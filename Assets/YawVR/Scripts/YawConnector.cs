using UnityEngine;
using YawVR;

public class YawConnector : MonoBehaviour, YawControllerDelegate {
    [SerializeField] private Transform vehicle;
    private YawController yaw;

    void Start() {
        yaw = YawController.Instance();
        yaw.ControllerDelegate = this;
        yaw.SetTiltLimits(-1, -1, -1);
    }

    void Update() {
        yaw.TrackerObject.SetRotation(vehicle.eulerAngles);
    }

    public void DidFoundDevice(YawDevice device) {
        if (yaw.State == ControllerState.Initial && device.Status != DeviceStatus.Reserved) {
            yaw.ConnectToDevice(device, () => yaw.StartDevice(), Debug.LogError);
        }
    }

    public void ControllerStateChanged(ControllerState state) { }
    public void DidDisconnectFrom(YawDevice device) { }
    public void DeviceStoppedFromApp() { }
    public void DeviceStartedFromApp() { }
}