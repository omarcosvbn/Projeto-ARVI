using UnityEngine;

public class ScrollingEnvironment : MonoBehaviour {
    [SerializeField] private float speed = 40f;
    [SerializeField] private float segmentLength = 40f;
    [SerializeField] private float recycleZ = -80f;

    void Update() {
        float loopLength = segmentLength * transform.childCount;
        foreach (Transform segment in transform) {
            segment.localPosition += Vector3.back * speed * Time.deltaTime;
            if (segment.localPosition.z < recycleZ) {
                segment.localPosition += Vector3.forward * loopLength;
            }
        }
    }
}
