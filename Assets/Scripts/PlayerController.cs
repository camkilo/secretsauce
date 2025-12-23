using UnityEngine;

/// <summary>
/// Player controller handling movement, dodge roll, and melee combat
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    
    [Header("Dodge Roll")]
    [SerializeField] private float dodgeSpeed = 12f;
    [SerializeField] private float dodgeDuration = 0.5f;
    [SerializeField] private float dodgeCooldown = 1f;
    [SerializeField] private float staminaCostDodge = 25f;
    
    [Header("Combat")]
    [SerializeField] private float lightAttackDamage = 15f;
    [SerializeField] private float heavyAttackDamage = 35f;
    [SerializeField] private float lightAttackCooldown = 0.5f;
    [SerializeField] private float heavyAttackCooldown = 1.5f;
    [SerializeField] private float attackRange = 2.5f;
    [SerializeField] private float staminaCostLight = 10f;
    [SerializeField] private float staminaCostHeavy = 30f;
    
    [Header("Stats")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaRegenRate = 15f;
    
    private CharacterController characterController;
    private Animator animator;
    private Camera mainCamera;
    
    private float currentHealth;
    private float currentStamina;
    private bool isDodging;
    private bool isInvulnerable;
    private bool isAttacking;
    private float dodgeTimer;
    private float dodgeCooldownTimer;
    private float attackCooldownTimer;
    
    private Vector3 moveDirection;
    private Vector3 dodgeDirection;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        
        if (characterController == null)
        {
            characterController = gameObject.AddComponent<CharacterController>();
            characterController.height = 2f;
            characterController.radius = 0.5f;
            characterController.center = new Vector3(0, 1, 0);
        }
    }

    void Update()
    {
        HandleMovement();
        HandleDodge();
        HandleCombat();
        RegenerateStamina();
        UpdateCooldowns();
    }

    void HandleMovement()
    {
        if (isDodging || isAttacking) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        moveDirection = (forward * vertical + right * horizontal).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            Vector3 movement = moveDirection * moveSpeed * Time.deltaTime;
            characterController.Move(movement);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
            if (animator != null)
            {
                animator.SetBool("IsWalking", true);
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("IsWalking", false);
            }
        }

        // Apply gravity
        characterController.Move(Vector3.down * 9.81f * Time.deltaTime);
    }

    void HandleDodge()
    {
        if (isDodging)
        {
            dodgeTimer -= Time.deltaTime;
            Vector3 dodgeMovement = dodgeDirection * dodgeSpeed * Time.deltaTime;
            characterController.Move(dodgeMovement);

            if (dodgeTimer <= 0)
            {
                isDodging = false;
                isInvulnerable = false;
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && dodgeCooldownTimer <= 0 && currentStamina >= staminaCostDodge)
        {
            if (moveDirection.magnitude > 0.1f)
            {
                dodgeDirection = moveDirection;
            }
            else
            {
                dodgeDirection = transform.forward;
            }

            isDodging = true;
            isInvulnerable = true;
            dodgeTimer = dodgeDuration;
            dodgeCooldownTimer = dodgeCooldown;
            currentStamina -= staminaCostDodge;
            
            if (animator != null)
            {
                animator.SetTrigger("Dodge");
            }
        }
    }

    void HandleCombat()
    {
        if (isAttacking || isDodging) return;

        // Light attack
        if (Input.GetMouseButtonDown(0) && attackCooldownTimer <= 0 && currentStamina >= staminaCostLight)
        {
            PerformAttack(lightAttackDamage, lightAttackCooldown, staminaCostLight, "LightAttack");
        }
        // Heavy attack
        else if (Input.GetMouseButtonDown(1) && attackCooldownTimer <= 0 && currentStamina >= staminaCostHeavy)
        {
            PerformAttack(heavyAttackDamage, heavyAttackCooldown, staminaCostHeavy, "HeavyAttack");
        }
    }

    void PerformAttack(float damage, float cooldown, float staminaCost, string animationTrigger)
    {
        isAttacking = true;
        attackCooldownTimer = cooldown;
        currentStamina -= staminaCost;

        if (animator != null)
        {
            animator.SetTrigger(animationTrigger);
        }

        // Detect enemies in range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * attackRange * 0.5f, attackRange);
        foreach (Collider col in hitColliders)
        {
            EnemyController enemy = col.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        Invoke("ResetAttack", cooldown * 0.7f);
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    void RegenerateStamina()
    {
        if (!isDodging && !isAttacking && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
        }
    }

    void UpdateCooldowns()
    {
        if (dodgeCooldownTimer > 0)
        {
            dodgeCooldownTimer -= Time.deltaTime;
        }
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isInvulnerable) return;

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
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }
        
        GameManager.Instance?.OnPlayerDeath();
        enabled = false;
    }

    public float GetHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
    public float GetStamina() => currentStamina;
    public float GetMaxStamina() => maxStamina;
    public bool IsAlive() => currentHealth > 0;
}
