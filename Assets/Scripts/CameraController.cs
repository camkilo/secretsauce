using UnityEngine;

/// <summary>
/// Third-person camera controller that follows the player
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    
    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -7);
    [SerializeField] private float smoothSpeed = 10f;
    [SerializeField] private float rotationSpeed = 5f;
    
    [Header("Mouse Control")]
    [SerializeField] private bool enableMouseRotation = true;
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private float minVerticalAngle = 10f;
    [SerializeField] private float maxVerticalAngle = 80f;

    private float currentHorizontalAngle;
    private float currentVerticalAngle = 30f;

    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }

        if (enableMouseRotation)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        HandleRotation();
        UpdateCameraPosition();
    }

    void HandleRotation()
    {
        if (enableMouseRotation)
        {
            currentHorizontalAngle += Input.GetAxis("Mouse X") * mouseSensitivity;
            currentVerticalAngle -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, minVerticalAngle, maxVerticalAngle);
        }
    }

    void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0);
        Vector3 rotatedOffset = rotation * new Vector3(0, offset.y, offset.z);
        
        Vector3 desiredPosition = target.position + rotatedOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        transform.LookAt(target.position + Vector3.up * 1.5f);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
