using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

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
    private bool onDashCooldown = false;
    private float dashCooldownTimer = 0.0f;
    private float dashNoGravityTimer = 0.0f;

    [Header("Negative effects")]
    [SerializeField] private float slowMoveSpeed = 2f;
    [SerializeField, ReadOnly] private float stunTime = 0.0f;
    public bool canMove = true;
    [SerializeField] private bool impedeExtraMoveset = false;
    private bool isSlowed { get { return slowStack > 0; } }
    private int slowStack = 0;
    private bool isStunned = false;
    public void ImpedeExtraMoveset()
    {
        impedeExtraMoveset = true;
    }
    public void AllowExtraMoveSet()
    {
        impedeExtraMoveset = false;
    }
    public void ApplySlow()
    {
        slowStack++;
    }
    public void RemoveSlow()
    {
        slowStack--;
    }
    public void ApplyStun(float duration)
    {
        isStunned = true;
        stunTime += duration;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if(isStunned)
        {
            stunTime -= Time.deltaTime;
            if(stunTime < 0.0f)
            {
                isStunned = false;      
            }
        }

        if (onDashCooldown)
        {
            dashCooldownTimer -= Time.fixedDeltaTime;
            if (dashCooldownTimer < 0.0f)
            {
                onDashCooldown = false;
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
        if (isStunned) return;
        if (moveInput.sqrMagnitude > 0.1f)
        {
            MoveAndRotate();
        }
    }
    private void MoveAndRotate()
    {
        float moveSpeed = isSlowed ? slowMoveSpeed : this.moveSpeed;
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
        
        Quaternion targetRotation = Quaternion.LookRotation(move);
        rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        

    }
    public void Dash()
    {
        if (!canMove) return;
        if (impedeExtraMoveset) return;
        if (onDashCooldown) return;

        onDashCooldown = true;
        dashCooldownTimer = dashCooldown;



        dashNoGravityTimer = dashNoGravityDuration;
        rb.useGravity = false;
        rb.AddForce(transform.forward*dashForce, ForceMode.Impulse);
        Debug.Log("Dash");
    }
    public void Jump()
    {
        if (!canMove) return;
        if (impedeExtraMoveset) return;
        if (!IsGrounded()) return;

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
