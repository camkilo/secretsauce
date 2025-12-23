using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI controller for displaying player stats and game info
/// </summary>
public class GameUIController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Image staminaBarFill;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalTimeText;

    [Header("Settings")]
    [SerializeField] private Color healthColor = Color.red;
    [SerializeField] private Color staminaColor = Color.green;

    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        
        if (healthBarFill != null)
        {
            healthBarFill.color = healthColor;
        }
        
        if (staminaBarFill != null)
        {
            staminaBarFill.color = staminaColor;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Update()
    {
        UpdateHealthBar();
        UpdateStaminaBar();
        UpdateTimer();
        UpdateWaveCounter();
        UpdateGameOver();
    }

    void UpdateHealthBar()
    {
        if (player != null && healthBarFill != null)
        {
            float healthPercent = player.GetHealth() / player.GetMaxHealth();
            healthBarFill.fillAmount = healthPercent;
        }
    }

    void UpdateStaminaBar()
    {
        if (player != null && staminaBarFill != null)
        {
            float staminaPercent = player.GetStamina() / player.GetMaxStamina();
            staminaBarFill.fillAmount = staminaPercent;
        }
    }

    void UpdateTimer()
    {
        if (timerText != null && GameManager.Instance != null)
        {
            timerText.text = "Time: " + GameManager.Instance.GetFormattedTime();
        }
    }

    void UpdateWaveCounter()
    {
        if (waveText != null && WaveSpawner.Instance != null)
        {
            waveText.text = $"Wave: {WaveSpawner.Instance.GetCurrentWave()}";
        }
    }

    void UpdateGameOver()
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsGameActive())
        {
            if (gameOverPanel != null && !gameOverPanel.activeSelf)
            {
                gameOverPanel.SetActive(true);
                
                if (finalTimeText != null)
                {
                    finalTimeText.text = $"Survived: {GameManager.Instance.GetFormattedTime()}";
                }
            }
        }
    }
}
