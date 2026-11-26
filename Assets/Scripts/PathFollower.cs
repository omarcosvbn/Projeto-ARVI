using UnityEngine;

public class PathFollower : MonoBehaviour {
    [SerializeField] private float speed = 5f;
    [SerializeField] private float turnSpeed = 8f;

    private EnemyPath path;
    private int index;

    public void SetPath(EnemyPath newPath) {
        path = newPath;
        index = 1;
    }

    void Update() {
        Vector3 target = path.GetPoint(index);
        Vector3 direction = target - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (direction != Vector3.zero) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
        }

        if (transform.position == target) {
            index++;
            if (index == path.Count) {
                Destroy(gameObject);
            }
        }
    }
}
