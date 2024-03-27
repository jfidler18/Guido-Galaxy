using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    public Transform planet; // The planet's transform
    public float gravity = -10f; // Gravity strength
    public float moveSpeed = 5f; // Movement speed
    public float jumpForce = 5f; // Jumping force

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

        isGrounded = Physics.Raycast(transform.position, -transform.up, out RaycastHit hitInfo, 4.0f);

        if (isGrounded)
        {
            groundNormal = hitInfo.normal;
        }
        else
        {
            groundNormal = -gravityDirection;
        }

        Quaternion toRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 50 * Time.deltaTime);

        MovePlayer();

        // Check for jump input
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            // Apply an impulse force using transform.up
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void MovePlayer()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = Vector3.Cross(transform.right, groundNormal).normalized;
        Vector3 right = Vector3.Cross(groundNormal, transform.forward).normalized;

        Vector3 moveDir = (forward * v + right * h).normalized;
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }
}
