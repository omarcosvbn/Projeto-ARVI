using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class RaycastShooter : MonoBehaviour {
    [System.Serializable]
    public class GunType {
        public string name;
        public float damage;
        public float fireInterval;
        public Tracer tracerPrefab;
        public float chairKick;
        public float hapticAmplitude;
        public float hapticDuration;
    }

    [SerializeField] private InputAction shootAction;
    [SerializeField] private InputAction switchAction;
    [SerializeField] private float range = 100f;
    [SerializeField] private YawConnector chair;
    [SerializeField] private HapticImpulsePlayer haptics;
    [SerializeField] private GunType[] guns = {
        new GunType { name = "Blaster", damage = 25f, fireInterval = 0.15f, hapticAmplitude = 0.4f, hapticDuration = 0.05f },
        new GunType { name = "Cannon", damage = 100f, fireInterval = 0.6f, chairKick = 6f }
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
        chair.Kick(gun.chairKick);
        haptics.SendHapticImpulse(gun.hapticAmplitude, gun.hapticDuration);
    }
}
