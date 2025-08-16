// RobotController.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class RobotController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;           // tốc độ di chuyển ngang
    public float acceleration = 10f;       // làm mượt tăng vận tốc
    public float jumpForce = 6f;           // lực nhảy

    [Header("Ground Check")]
    public LayerMask groundLayer;          // layer của mặt đất
    public float groundCheckDistance = 0.1f;
    public Vector3 groundCheckOffset = new Vector3(0, -0.5f, 0);

    [Header("Movement Bounds (X,Z)")]
    public float minX = -10f;
    public float maxX = 10f;
    public float minZ = -10f;
    public float maxZ = 10f;

    [Header("Collision")]
    public CollisionDetectionMode collisionMode = CollisionDetectionMode.Continuous;

    // Internal
    Rigidbody rb;
    Vector3 currentVel = Vector3.zero;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; // tránh lật robot
        rb.collisionDetectionMode = collisionMode;
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
        EnforceBounds();
    }

    void HandleMovement()
    {
        // Lấy input WASD / Arrow keys
        float h = 0f;
        float v = 0f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) v += 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) v -= 1f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) h -= 1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) h += 1f;

        Vector3 inputDir = new Vector3(h, 0f, v);
        if (inputDir.sqrMagnitude > 1f) inputDir.Normalize();

        // Di chuyển theo hướng local (nếu muốn là global, dùng transform.TransformDirection)
        // Ở đây giả sử robot di chuyển theo hướng world (x,z)
        Vector3 targetVelocity = inputDir * moveSpeed;
        // Giữ nguyên vận tốc y (rơi / nhảy)
        Vector3 newVelocity = Vector3.Lerp(new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z), targetVelocity, acceleration * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector3(newVelocity.x, rb.linearVelocity.y, newVelocity.z);

        // Quay hướng robot về hướng di chuyển (nếu có input)
        if (inputDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(new Vector3(inputDir.x, 0, inputDir.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.fixedDeltaTime);
        }
    }

    bool IsGrounded()
    {
        Vector3 origin = transform.position + groundCheckOffset;
        // Raycast xuống để kiểm tra chạm đất
        return Physics.Raycast(origin, Vector3.down, groundCheckDistance + 0.01f, groundLayer);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            // reset thành phần y trước khi thêm lực để nhảy ổn định hơn
            Vector3 vel = rb.linearVelocity;
            vel.y = 0f;
            rb.linearVelocity = vel;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void EnforceBounds()
    {
        Vector3 pos = rb.position;
        float clampedX = Mathf.Clamp(pos.x, minX, maxX);
        float clampedZ = Mathf.Clamp(pos.z, minZ, maxZ);

        if (clampedX != pos.x || clampedZ != pos.z)
        {
            // đặt lại vị trí vào trong biên
            rb.position = new Vector3(clampedX, pos.y, clampedZ);
            // loại bỏ vận tốc ngang hướng ra ngoài biên để tránh "dính" vào cạnh
            Vector3 v = rb.linearVelocity;
            v.x = Mathf.Clamp(v.x, -moveSpeed, moveSpeed);
            v.z = Mathf.Clamp(v.z, -moveSpeed, moveSpeed);
            rb.linearVelocity = v;
        }
    }

    // Tuỳ ý: detect va chạm với tường nếu muốn trigger hành vi
    void OnCollisionEnter(Collision collision)
    {
        // Ví dụ: nếu đối tượng có tag "Wall", ta có thể in ra hoặc dừng vận tốc ngang
        if (collision.gameObject.CompareTag("Wall"))
        {
            // dừng thành phần vận tốc ngang để không tiếp tục "đẩy" vào tường
            Vector3 v = rb.linearVelocity;
            v.x = 0f;
            v.z = 0f;
            rb.linearVelocity = new Vector3(v.x, rb.linearVelocity.y, v.z);
            // Debug.Log("Va chạm tường: " + collision.gameObject.name);
        }
    }

    // Gizmos để thấy vùng giới hạn trong Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 center = new Vector3((minX + maxX) / 2f, transform.position.y, (minZ + maxZ) / 2f);
        Vector3 size = new Vector3(Mathf.Abs(maxX - minX), 0.1f, Mathf.Abs(maxZ - minZ));
        Gizmos.DrawWireCube(center, size);

        Gizmos.color = Color.yellow;
        Vector3 origin = transform.position + groundCheckOffset;
        Gizmos.DrawLine(origin, origin + Vector3.down * groundCheckDistance);
    }
}
