using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField, ReadOnly] public Vector2 moveInput;
    private Rigidbody rb;
    public bool canMove = true;
    private bool canDash = true;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 720f; // Degrees per second
    [SerializeField] private float dashForce = 5f;
    [SerializeField] private float dashCooldown = 0.2f;
    private float dashCooldownTimer=0.0f;

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
        rb.AddForce(transform.forward*dashForce, ForceMode.Impulse);
        Debug.Log("Dash");
    }

}
