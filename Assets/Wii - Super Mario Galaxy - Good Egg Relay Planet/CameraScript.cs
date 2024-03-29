using UnityEngine;

public class UnifiedCameraControl : MonoBehaviour
{
    public Transform target; // The player's transform
    public Transform planet; // The planet's transform, used for orientation
    public float smoothSpeed = 0.125f; // Adjust for smoother transitions
    public float minimumDistance = 1.0f; // Minimum distance the camera can be from the player
    private Vector3 defaultOffset = new Vector3(0, 4, -5); // Default camera offset from the player
    private float maxDistance; // Max distance calculated from the default offset

    void Start()
    {
        maxDistance = defaultOffset.magnitude; // Set max distance from the default offset magnitude
    }

    void LateUpdate()
    {
        // Calculate dynamic offset based on player's orientation and gravity direction
        Vector3 gravityDirection = (target.position - planet.position).normalized;
        Vector3 playerUp = target.up;
        float dynamicDistance = maxDistance * (Quaternion.FromToRotation(playerUp, gravityDirection).eulerAngles.magnitude / 180);
        
        // Adjust dynamicDistance based on collision detection
        Vector3 desiredCameraPosition = target.position - target.forward * dynamicDistance + playerUp * defaultOffset.y;
        RaycastHit hit;
        
        // Perform raycast to detect collisions
        if (Physics.Raycast(target.position, desiredCameraPosition - target.position, out hit, dynamicDistance))
        {
            dynamicDistance = Mathf.Clamp(hit.distance, minimumDistance, dynamicDistance);
        }

        // Calculate the final adjusted offset based on the dynamic distance and any collision adjustments
        Vector3 adjustedOffset = -target.forward * dynamicDistance + playerUp * defaultOffset.y;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, target.position + adjustedOffset, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Ensure the camera always looks at the player
        transform.LookAt(target);
    }
}