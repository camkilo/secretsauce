using UnityEngine;

/// <summary>
/// Helper script to visualize attack ranges and debug information
/// Attach to Player or Enemies for visual debugging
/// </summary>
public class DebugVisualizer : MonoBehaviour
{
    [Header("Debug Options")]
    [SerializeField] private bool showAttackRange = true;
    [SerializeField] private bool showMovementPath = true;
    [SerializeField] private bool showStats = true;
    
    [Header("Colors")]
    [SerializeField] private Color attackRangeColor = Color.red;
    [SerializeField] private Color movementPathColor = Color.blue;

    private PlayerController player;
    private EnemyController enemy;

    void Start()
    {
        player = GetComponent<PlayerController>();
        enemy = GetComponent<EnemyController>();
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        if (showAttackRange)
        {
            DrawAttackRange();
        }
    }

    void DrawAttackRange()
    {
        if (player != null)
        {
            Gizmos.color = attackRangeColor;
            Gizmos.DrawWireSphere(transform.position + transform.forward * 1.25f, 2.5f);
        }
        else if (enemy != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 2f);
        }
    }

    void OnGUI()
    {
        if (!showStats) return;

        if (player != null)
        {
            GUILayout.BeginArea(new Rect(10, 10, 250, 200));
            GUILayout.Label($"Health: {player.GetHealth():F0} / {player.GetMaxHealth():F0}");
            GUILayout.Label($"Stamina: {player.GetStamina():F0} / {player.GetMaxStamina():F0}");
            GUILayout.EndArea();
        }
    }
}
