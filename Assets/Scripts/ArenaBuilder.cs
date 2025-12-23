using UnityEngine;

/// <summary>
/// Procedurally generates a circular stone arena with pillars
/// </summary>
public class ArenaBuilder : MonoBehaviour
{
    [Header("Arena Settings")]
    [SerializeField] private float arenaRadius = 15f;
    [SerializeField] private float wallHeight = 5f;
    [SerializeField] private int wallSegments = 32;
    
    [Header("Pillars")]
    [SerializeField] private int pillarCount = 8;
    [SerializeField] private float pillarRadius = 0.8f;
    [SerializeField] private float pillarHeight = 4f;
    [SerializeField] private float pillarDistanceFromCenter = 10f;
    
    [Header("Materials")]
    [SerializeField] private Material stoneMaterial;
    [SerializeField] private Material floorMaterial;
    
    [Header("Lighting")]
    [SerializeField] private int torchCount = 12;
    [SerializeField] private GameObject torchPrefab;

    void Start()
    {
        BuildArena();
    }

    void BuildArena()
    {
        CreateFloor();
        CreateWalls();
        CreatePillars();
        CreateTorches();
        SetupFog();
    }

    void CreateFloor()
    {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        floor.name = "Arena Floor";
        floor.transform.position = Vector3.zero;
        floor.transform.localScale = new Vector3(arenaRadius * 2, 0.2f, arenaRadius * 2);
        
        if (floorMaterial != null)
        {
            floor.GetComponent<Renderer>().material = floorMaterial;
        }
        else
        {
            floor.GetComponent<Renderer>().material.color = new Color(0.3f, 0.3f, 0.3f);
        }
        
        floor.transform.parent = transform;
    }

    void CreateWalls()
    {
        GameObject wallParent = new GameObject("Arena Walls");
        wallParent.transform.parent = transform;

        float angleStep = 360f / wallSegments;
        
        for (int i = 0; i < wallSegments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float nextAngle = (i + 1) * angleStep * Mathf.Deg2Rad;
            
            Vector3 start = new Vector3(Mathf.Cos(angle) * arenaRadius, 0, Mathf.Sin(angle) * arenaRadius);
            Vector3 end = new Vector3(Mathf.Cos(nextAngle) * arenaRadius, 0, Mathf.Sin(nextAngle) * arenaRadius);
            
            CreateWallSegment(start, end, wallParent.transform);
        }
    }

    void CreateWallSegment(Vector3 start, Vector3 end, Transform parent)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = "Wall Segment";
        
        Vector3 center = (start + end) / 2f;
        center.y = wallHeight / 2f;
        wall.transform.position = center;
        
        float length = Vector3.Distance(start, end);
        wall.transform.localScale = new Vector3(length, wallHeight, 0.5f);
        
        Vector3 direction = end - start;
        wall.transform.rotation = Quaternion.LookRotation(Vector3.up, direction);
        wall.transform.Rotate(90, 0, 0);
        
        if (stoneMaterial != null)
        {
            wall.GetComponent<Renderer>().material = stoneMaterial;
        }
        else
        {
            wall.GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 0.4f);
        }
        
        wall.transform.parent = parent;
    }

    void CreatePillars()
    {
        GameObject pillarParent = new GameObject("Pillars");
        pillarParent.transform.parent = transform;

        float angleStep = 360f / pillarCount;
        
        for (int i = 0; i < pillarCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 position = new Vector3(
                Mathf.Cos(angle) * pillarDistanceFromCenter,
                pillarHeight / 2f,
                Mathf.Sin(angle) * pillarDistanceFromCenter
            );
            
            CreatePillar(position, pillarParent.transform);
        }
    }

    void CreatePillar(Vector3 position, Transform parent)
    {
        GameObject pillar = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        pillar.name = "Pillar";
        pillar.transform.position = position;
        pillar.transform.localScale = new Vector3(pillarRadius * 2, pillarHeight / 2f, pillarRadius * 2);
        
        if (stoneMaterial != null)
        {
            pillar.GetComponent<Renderer>().material = stoneMaterial;
        }
        else
        {
            pillar.GetComponent<Renderer>().material.color = new Color(0.35f, 0.35f, 0.35f);
        }
        
        pillar.tag = "Cover";
        pillar.transform.parent = parent;
    }

    void CreateTorches()
    {
        GameObject torchParent = new GameObject("Torches");
        torchParent.transform.parent = transform;

        float angleStep = 360f / torchCount;
        
        for (int i = 0; i < torchCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 position = new Vector3(
                Mathf.Cos(angle) * (arenaRadius - 1f),
                2.5f,
                Mathf.Sin(angle) * (arenaRadius - 1f)
            );
            
            CreateTorch(position, torchParent.transform);
        }
    }

    void CreateTorch(Vector3 position, Transform parent)
    {
        GameObject torch;
        
        if (torchPrefab != null)
        {
            torch = Instantiate(torchPrefab, position, Quaternion.identity);
        }
        else
        {
            torch = new GameObject("Torch");
            torch.transform.position = position;
            
            // Create light
            Light torchLight = torch.AddComponent<Light>();
            torchLight.type = LightType.Point;
            torchLight.color = new Color(1f, 0.6f, 0.3f);
            torchLight.intensity = 2f;
            torchLight.range = 8f;
            torchLight.shadows = LightShadows.Soft;
            
            // Add flickering effect
            torch.AddComponent<TorchFlicker>();
        }
        
        torch.transform.parent = parent;
    }

    void SetupFog()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = 0.02f;
        RenderSettings.fogColor = new Color(0.1f, 0.1f, 0.15f);
        
        // Set ambient lighting
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.2f, 0.2f, 0.25f);
    }
}
