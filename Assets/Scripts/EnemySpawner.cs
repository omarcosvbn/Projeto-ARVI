using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private PathFollower enemyPrefab;
    [SerializeField] private EnemyPath path;
    [SerializeField] private int count = 5;
    [SerializeField] private float interval = 0.6f;
    [SerializeField] private float startDelay = 0f;

    IEnumerator Start() {
        yield return new WaitForSeconds(startDelay);
        Vector3 start = path.GetPoint(0);
        Quaternion facing = Quaternion.LookRotation(path.GetPoint(1) - start);

        for (int i = 0; i < count; i++) {
            Instantiate(enemyPrefab, start, facing).SetPath(path);
            yield return new WaitForSeconds(interval);
        }
    }
}
