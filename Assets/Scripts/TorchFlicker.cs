using UnityEngine;

/// <summary>
/// Creates flickering effect for torch lights
/// </summary>
public class TorchFlicker : MonoBehaviour
{
    [SerializeField] private float minIntensity = 1.5f;
    [SerializeField] private float maxIntensity = 2.5f;
    [SerializeField] private float flickerSpeed = 0.1f;

    private Light torchLight;
    private float baseIntensity;

    void Start()
    {
        torchLight = GetComponent<Light>();
        if (torchLight != null)
        {
            baseIntensity = torchLight.intensity;
            minIntensity = baseIntensity * 0.8f;
            maxIntensity = baseIntensity * 1.2f;
        }
    }

    void Update()
    {
        if (torchLight != null)
        {
            float flicker = Mathf.PerlinNoise(Time.time * 10f, 0f);
            torchLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, flicker);
        }
    }
}
