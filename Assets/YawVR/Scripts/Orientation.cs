using UnityEngine;

public class Orientation : MonoBehaviour {
    public float pitch => -Mathf.Asin(Mathf.Clamp(transform.forward.y, -1f, 1f)) * Mathf.Rad2Deg;

    public float roll => Mathf.Asin(Mathf.Clamp(transform.right.y, -1f, 1f)) * Mathf.Rad2Deg;

    public float yaw {
        get {
            var forward = Ground(transform.forward);
            if (forward.magnitude < 0.5f) {
                var right = Ground(transform.right);
                var rightAngle = Vector3.Angle(right, Vector3.right);
                return right.z < 0 ? rightAngle : -rightAngle;
            }
            var angle = Vector3.Angle(forward, Vector3.forward);
            return forward.x > 0 ? angle : -angle;
        }
    }

    static Vector3 Ground(Vector3 v) {
        v.y = 0f;
        return v;
    }
}
