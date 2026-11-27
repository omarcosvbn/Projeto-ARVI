using UnityEngine;

public class Tracer : MonoBehaviour {
    [SerializeField] private float speed = 80f;

    private float remaining;

    public void Fire(float distance) {
        remaining = distance;
    }

    private void Update() {
        float step = speed * Time.deltaTime;
        transform.position += transform.forward * step;
        remaining -= step;
        if (remaining <= 0f) {
            Destroy(gameObject);
        }
    }
}
