using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastShooter : MonoBehaviour {
    [System.Serializable]
    public class GunType {
        public string name;
        public float damage;
        public float fireInterval;
        public Tracer tracerPrefab;
    }

    [SerializeField] private InputAction shootAction;
    [SerializeField] private InputAction switchAction;
    [SerializeField] private float range = 100f;
    [SerializeField] private GunType[] guns = {
        new GunType { name = "Blaster", damage = 25f, fireInterval = 0.1f },
        new GunType { name = "Cannon", damage = 100f, fireInterval = 0.6f }
    };

    private int current;
    private float nextShotTime;

    void OnEnable() {
        shootAction.Enable();
        switchAction.Enable();
    }

    void OnDisable() {
        shootAction.Disable();
        switchAction.Disable();
    }

    void Update() {
        if (switchAction.WasPressedThisFrame()) {
            current = (current + 1) % guns.Length;
        }
        if (shootAction.IsPressed() && Time.time >= nextShotTime) {
            nextShotTime = Time.time + guns[current].fireInterval;
            Shoot();
        }
    }

    public void Shoot() {
        GunType gun = guns[current];
        float distance = range;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range)) {
            hit.collider.GetComponentInParent<Enemy>()?.TakeDamage(gun.damage);
            distance = hit.distance;
        }
        Instantiate(gun.tracerPrefab, transform.position, transform.rotation).Fire(distance);
    }
}
