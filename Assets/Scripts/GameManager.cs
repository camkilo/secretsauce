using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Main game manager handling game state and flow
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    private bool gameActive = false;
    private float survivalTime = 0f;
    private bool gameStarted = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (gameActive)
        {
            survivalTime += Time.deltaTime;
        }

        // Restart game
        if (!gameActive && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        // Pause/Resume
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void StartGame()
    {
        gameActive = true;
        gameStarted = true;
        survivalTime = 0f;
        Time.timeScale = 1f;
    }

    public void OnPlayerDeath()
    {
        gameActive = false;
        Debug.Log($"Game Over! Survival Time: {GetFormattedTime()}");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TogglePause()
    {
        if (gameActive)
        {
            Time.timeScale = Time.timeScale > 0 ? 0 : 1;
        }
    }

    public bool IsGameActive() => gameActive;
    public float GetSurvivalTime() => survivalTime;
    
    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(survivalTime / 60f);
        int seconds = Mathf.FloorToInt(survivalTime % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}
