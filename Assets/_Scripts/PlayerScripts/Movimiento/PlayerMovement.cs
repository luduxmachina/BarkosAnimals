using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

    public bool canMove = true;
    [SerializeField, ReadOnly] public Vector2 moveInput;
    private Rigidbody rb;
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 720f; // Degrees per second
    [Header("Jump")]
    [SerializeField] private float heightFromGround = 1f;
    [SerializeField] private float jumpForce = 5f;
    [Header("Dash")]
    [SerializeField] private float dashForce = 5f;
    [SerializeField] private float dashCooldown = 0.3f;
    [SerializeField] private float dashNoGravityDuration = 0.15f;
    private bool canDash = true;
    private float dashCooldownTimer=0.0f;
    private float dashNoGravityTimer=0.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (!canDash)
        {
            dashCooldownTimer -= Time.fixedDeltaTime;
            if (dashCooldownTimer < 0.0f)
            {
                canDash = true;
            }

            dashNoGravityTimer -= Time.fixedDeltaTime;
            if (dashNoGravityTimer < 0.0f)
            {
                rb.useGravity = true;
                dashNoGravityTimer = 100.0f; //para que no vuelva a entrar aqui
            }
        }
    }
    private void FixedUpdate()
    {

        if (!canMove) return;
        if (moveInput.sqrMagnitude > 0.1f)
        {
            MoveAndRotate();
        }
    }
    private void MoveAndRotate()
    {
        
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
        
        Quaternion targetRotation = Quaternion.LookRotation(move);
        rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        

    }
    public void Dash()
    {
        if (!canMove) return;

        canDash = false;
        dashCooldownTimer = dashCooldown;

        dashNoGravityTimer = dashNoGravityDuration;
        rb.useGravity = false;
        rb.AddForce(transform.forward*dashForce, ForceMode.Impulse);
        Debug.Log("Dash");
    }
    public void Jump()
    {
        if (!canMove) return;
        if(!IsGrounded()) return;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, heightFromGround);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - Vector3.up * heightFromGround);
        Gizmos.DrawSphere(transform.position - (Vector3.up * (heightFromGround - 0.1f)), 0.1f);
    }

}
