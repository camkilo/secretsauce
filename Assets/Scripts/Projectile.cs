using UnityEngine;

/// <summary>
/// Projectile controller for ranged enemy attacks
/// </summary>
public class Projectile : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private float damage;
    private bool initialized;

    public void Initialize(Vector3 dir, float spd, float dmg)
    {
        direction = dir.normalized;
        speed = spd;
        damage = dmg;
        initialized = true;
        
        // Ensure proper collision detection
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // We control movement manually
        }
        
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true; // Enable trigger events
        }
    }

    void Update()
    {
        if (initialized)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Enemy") && !other.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }
}
