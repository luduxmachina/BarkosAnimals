using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField, ReadOnly] public Vector2 moveInput;
    public Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField]
    private PlayerCurrentStats playerCurrentStats;
    [SerializeField]
    private PlayerInSceneEffects playerInSceneEffects;
    [Header("Ground Check")]
    [SerializeField] private float heightFromGround = 1.1f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {

        if (!playerCurrentStats.canMove) return;
        if (moveInput.sqrMagnitude > 0.1f)
        {
            MoveAndRotate();
        }
        else
        {
            animator.SetTrigger("Idle");
        }
    }
    private void MoveAndRotate()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * playerCurrentStats.currentStats.moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        Quaternion targetRotation = Quaternion.LookRotation(move);
        rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, playerCurrentStats.currentStats.rotationSpeed * Time.fixedDeltaTime);
        animator.SetTrigger("Walk");
        
    }
    public void Dash()
    {
        if(!playerCurrentStats.canMove) return;
        if (!playerCurrentStats.canDash) return;


        animator.SetTrigger("Dash");
        rb.AddForce(transform.forward*playerCurrentStats.currentStats.dashForce, ForceMode.Impulse);
        playerInSceneEffects.AddOnDashEffects();
        Debug.Log("Dash");
    }
    public void Jump()
    {

        if (!playerCurrentStats.canMove) return;
        if (!playerCurrentStats.canJump) return;
        if (!IsGrounded()) return;
        
        playerInSceneEffects.AddOnJumpEffects();
        animator.SetTrigger("Jump");
        rb.AddForce(Vector3.up * playerCurrentStats.currentStats.jumpForce, ForceMode.Impulse);

        
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
