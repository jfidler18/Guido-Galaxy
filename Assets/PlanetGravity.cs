using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    public Transform planet; // The planet's transform
    public float gravity = -10f; // Gravity strength
    public float moveSpeed = 5f; // Movement speed
    public float jumpForce = 5f; // Jumping force
    public float rotationSpeed = 1f;

    private Rigidbody rb;
    private Vector3 groundNormal;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;

    }

    void FixedUpdate()
    {
        Vector3 gravityDirection = (transform.position - planet.position).normalized;
        rb.AddForce(gravityDirection * gravity);

        // SphereCast parameters
        float sphereRadius = 0.5f; // Adjust based on your player's size
        float castDistance = 10.0f; // Max distance to check for ground
        Vector3 castDirection = -transform.up; // Casting downwards relative to the player
        RaycastHit hitInfo;

        // Perform the SphereCast
        bool hasHit = Physics.SphereCast(transform.position, sphereRadius, castDirection, out hitInfo, castDistance);

        if (hasHit)
        {
            isGrounded = true;
            groundNormal = hitInfo.normal;

            // Adjust player's height above the ground as needed
            float desiredHeight = 0.1f; // Desired height above terrain
            float currentHeight = hitInfo.distance - sphereRadius; // Adjust for sphere radius
            float heightDifference = desiredHeight - currentHeight;
            Vector3 heightAdjustment = groundNormal * heightDifference * Time.deltaTime * 5;
            rb.MovePosition(rb.position + heightAdjustment);
        }
        else
        {
            isGrounded = false;
            groundNormal = -gravityDirection;
        }

        // Rotate the player to align with the ground normal
        Quaternion toRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

        MovePlayer();

        // Check for jump input
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void MovePlayer()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Transform cameraTransform = Camera.main.transform;
        Vector3 forward = Vector3.Cross(cameraTransform.right, groundNormal).normalized;
        Vector3 right = Vector3.Cross(groundNormal, cameraTransform.forward).normalized;
        Vector3 moveDir = (forward * v + right * h).normalized;

        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);

        // Apply rotation only on horizontal input
        if (h != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }


}

