using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour {
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private UnityEvent onDeath;

    private float health;

    void Awake() {
        health = maxHealth;
    }

    public void TakeDamage(float amount) {
        health -= amount;
        if (health <= 0f) {
            onDeath.Invoke();
            Destroy(gameObject);
        }
    }
}