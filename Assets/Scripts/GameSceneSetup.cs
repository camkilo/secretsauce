using UnityEngine;

/// <summary>
/// Setup helper script to automatically configure the game scene
/// Attach this to an empty GameObject to auto-setup the arena
/// </summary>
public class GameSceneSetup : MonoBehaviour
{
    [Header("Auto Setup Configuration")]
    [SerializeField] private bool autoSetupOnStart = true;
    [SerializeField] private bool createPlayer = true;
    [SerializeField] private bool createArena = true;
    [SerializeField] private bool createManagers = true;
    [SerializeField] private bool createUI = true;

    void Start()
    {
        if (autoSetupOnStart)
        {
            SetupScene();
        }
    }

    [ContextMenu("Setup Scene")]
    public void SetupScene()
    {
        Debug.Log("Starting scene setup...");

        if (createPlayer)
        {
            CreatePlayerSetup();
        }

        if (createArena)
        {
            CreateArenaSetup();
        }

        if (createManagers)
        {
            CreateManagersSetup();
        }

        if (createUI)
        {
            CreateUISetup();
        }

        Debug.Log("Scene setup complete!");
    }

    void CreatePlayerSetup()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if (player == null)
        {
            player = new GameObject("Player");
            player.tag = "Player";
            player.transform.position = new Vector3(0, 1, 0);

            CharacterController controller = player.AddComponent<CharacterController>();
            controller.height = 2f;
            controller.radius = 0.5f;
            controller.center = new Vector3(0, 1, 0);

            player.AddComponent<PlayerController>();

            // Visual representation
            GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            visual.name = "PlayerModel";
            visual.transform.parent = player.transform;
            visual.transform.localPosition = Vector3.up;
            Destroy(visual.GetComponent<Collider>()); // Remove collider from visual

            Debug.Log("Player created successfully");
        }
        else
        {
            Debug.Log("Player already exists");
        }
    }

    void CreateArenaSetup()
    {
        GameObject arena = GameObject.Find("Arena");
        
        if (arena == null)
        {
            arena = new GameObject("Arena");
            arena.AddComponent<ArenaBuilder>();
            Debug.Log("Arena created successfully");
        }
        else
        {
            Debug.Log("Arena already exists");
        }
    }

    void CreateManagersSetup()
    {
        // Game Manager
        if (GameManager.Instance == null)
        {
            GameObject gm = new GameObject("GameManager");
            gm.AddComponent<GameManager>();
            Debug.Log("GameManager created successfully");
        }

        // Wave Spawner
        if (WaveSpawner.Instance == null)
        {
            GameObject spawner = new GameObject("WaveSpawner");
            spawner.AddComponent<WaveSpawner>();
            Debug.Log("WaveSpawner created successfully");
        }

        // Camera
        Camera mainCam = Camera.main;
        if (mainCam != null && mainCam.GetComponent<CameraController>() == null)
        {
            mainCam.gameObject.AddComponent<CameraController>();
            Debug.Log("CameraController added to Main Camera");
        }
    }

    void CreateUISetup()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
            
            // Add UI Controller
            canvasObj.AddComponent<GameUIController>();
            
            Debug.Log("UI Canvas created - Please configure UI elements in GameUIController");
        }
        else
        {
            if (canvas.GetComponent<GameUIController>() == null)
            {
                canvas.gameObject.AddComponent<GameUIController>();
                Debug.Log("GameUIController added to existing Canvas");
            }
        }
    }
}
