using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Base enemy controller with AI behavior and health system
/// </summary>
public class EnemyController : MonoBehaviour
{
    [Header("Enemy Type")]
    [SerializeField] private EnemyType enemyType;
    
    [Header("Stats")]
    [SerializeField] private float maxHealth = 50f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.5f;
    
    [Header("Ranged (Caster Only)")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;
    
    private NavMeshAgent navAgent;
    private Animator animator;
    private Transform player;
    private float currentHealth;
    private float attackTimer;
    private bool isDead;

    public enum EnemyType
    {
        Rusher,  // Fast, low health, melee
        Brute,   // Slow, high health, heavy melee
        Caster   // Medium speed, fragile, ranged attacks
    }

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        
        currentHealth = maxHealth;
        
        if (navAgent == null)
        {
            navAgent = gameObject.AddComponent<NavMeshAgent>();
        }
        
        navAgent.speed = moveSpeed;
        
        // Configure based on enemy type
        ConfigureEnemyType();
    }

    void ConfigureEnemyType()
    {
        switch (enemyType)
        {
            case EnemyType.Rusher:
                moveSpeed = 6f;
                maxHealth = 30f;
                attackDamage = 8f;
                attackRange = 1.5f;
                attackCooldown = 1f;
                break;
            
            case EnemyType.Brute:
                moveSpeed = 2f;
                maxHealth = 100f;
                attackDamage = 25f;
                attackRange = 2.5f;
                attackCooldown = 2f;
                transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                break;
            
            case EnemyType.Caster:
                moveSpeed = 3f;
                maxHealth = 20f;
                attackDamage = 12f;
                attackRange = 8f;
                attackCooldown = 2.5f;
                break;
        }
        
        currentHealth = maxHealth;
        navAgent.speed = moveSpeed;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        // Movement behavior
        if (enemyType == EnemyType.Caster)
        {
            // Casters keep distance
            if (distanceToPlayer < attackRange * 0.7f)
            {
                // Move away from player
                Vector3 retreatDirection = (transform.position - player.position).normalized;
                navAgent.SetDestination(transform.position + retreatDirection * 3f);
            }
            else if (distanceToPlayer > attackRange)
            {
                // Move closer but maintain range
                Vector3 approachPosition = player.position + (transform.position - player.position).normalized * (attackRange * 0.8f);
                navAgent.SetDestination(approachPosition);
            }
            else
            {
                navAgent.SetDestination(transform.position);
            }
        }
        else
        {
            // Melee enemies chase directly
            navAgent.SetDestination(player.position);
        }

        // Look at player
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0;
        if (lookDirection.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), 5f * Time.deltaTime);
        }

        // Update animation
        if (animator != null)
        {
            animator.SetBool("IsMoving", navAgent.velocity.magnitude > 0.1f);
        }

        // Attack logic
        attackTimer -= Time.deltaTime;
        
        if (distanceToPlayer <= attackRange && attackTimer <= 0)
        {
            Attack();
        }
    }

    void Attack()
    {
        attackTimer = attackCooldown;
        
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        if (enemyType == EnemyType.Caster)
        {
            // Ranged attack
            Invoke("FireProjectile", 0.3f);
        }
        else
        {
            // Melee attack
            Invoke("DealMeleeDamage", 0.3f);
        }
    }

    void FireProjectile()
    {
        if (player == null) return;

        Vector3 spawnPosition = transform.position + Vector3.up * 1.5f + transform.forward * 0.5f;
        Vector3 direction = (player.position + Vector3.up - spawnPosition).normalized;

        if (projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.LookRotation(direction));
            Projectile projScript = projectile.GetComponent<Projectile>();
            if (projScript != null)
            {
                projScript.Initialize(direction, projectileSpeed, attackDamage);
            }
        }
        else
        {
            // Create a simple projectile if prefab is missing
            GameObject proj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            proj.transform.position = spawnPosition;
            proj.transform.localScale = Vector3.one * 0.3f;
            Rigidbody rb = proj.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.velocity = direction * projectileSpeed;
            
            Projectile projScript = proj.AddComponent<Projectile>();
            projScript.Initialize(direction, projectileSpeed, attackDamage);
            
            Destroy(proj, 5f);
        }
    }

    void DealMeleeDamage()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(attackDamage);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }
        
        if (navAgent != null)
        {
            navAgent.enabled = false;
        }

        WaveSpawner.Instance?.OnEnemyDeath();
        
        Destroy(gameObject, 2f);
    }

    public EnemyType GetEnemyType() => enemyType;
    public void SetEnemyType(EnemyType type)
    {
        enemyType = type;
        ConfigureEnemyType();
    }
}
