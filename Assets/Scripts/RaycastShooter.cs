using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastShooter : MonoBehaviour {
    [SerializeField] private InputAction shootAction;
    [SerializeField] private float damage = 25f;
    [SerializeField] private float range = 100f;

    void OnEnable() {
        shootAction.performed += OnShoot;
        shootAction.Enable();
    }

    void OnDisable() {
        shootAction.performed -= OnShoot;
        shootAction.Disable();
    }

    void OnShoot(InputAction.CallbackContext context) {
        Shoot();
    }

    public void Shoot() {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range)) {
            hit.collider.GetComponentInParent<Enemy>()?.TakeDamage(damage);
        }
    }
}