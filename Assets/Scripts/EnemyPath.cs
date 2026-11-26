using UnityEngine;

public class EnemyPath : MonoBehaviour {
    public int Count => transform.childCount;

    public Vector3 GetPoint(int index) {
        return transform.GetChild(index).position;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        for (int i = 0; i < transform.childCount; i++) {
            Gizmos.DrawSphere(GetPoint(i), 0.2f);
            if (i > 0) {
                Gizmos.DrawLine(GetPoint(i - 1), GetPoint(i));
            }
        }
    }
}
