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
    [SerializeField] private AdaptToFloor adaptToFloor;
    [Header("Audios")]
    [SerializeField] private PlayerSoundManager soundManager;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (!playerCurrentStats.canMove) return;

        if(rb.angularVelocity.sqrMagnitude < (2f *Time.fixedDeltaTime))
        {
            rb.angularVelocity = Vector3.zero;
        }
        if (moveInput.sqrMagnitude > 0.1f)
        {
            MoveAndRotate();
            soundManager.StartMovementLoop();
        }
        else
        {
            animator.SetTrigger("Idle");
            soundManager.StopMovementLoop();
        }
    }
  
    private void MoveAndRotate()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        move = Vector3.ProjectOnPlane(move, adaptToFloor.upVector).normalized;
        move *= playerCurrentStats.currentStats.moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
        


        if (move.sqrMagnitude > 0.0001f)
        {
            Vector3 forward = move.normalized;
            Vector3 right = Vector3.Cross(adaptToFloor.upVector, forward);
            forward = Vector3.Cross(right, adaptToFloor.upVector);

            Quaternion targetRotation = Quaternion.LookRotation(forward, adaptToFloor.upVector);
            rb.angularVelocity *= 0.5f;
              rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, playerCurrentStats.currentStats.rotationSpeed * Time.fixedDeltaTime));
         
           
        }

                
    }
    public void Dash()
    {
        if(!playerCurrentStats.canMove) return;
        if (!playerCurrentStats.canDash) return;


        animator.SetTrigger("Dash");
        soundManager.ActivarSonidoDash();
        rb.AddForce(rb.mass*transform.forward*playerCurrentStats.currentStats.dashForce, ForceMode.Impulse);
        playerInSceneEffects.AddOnDashEffects();
        Debug.Log("Dash");
    }
    public void Jump()
    {

        if (!playerCurrentStats.canMove) return;
        if (!playerCurrentStats.canJump) return;
        if (!adaptToFloor.IsGrounded()) return;
        
        playerInSceneEffects.AddOnJumpEffects();
        animator.SetTrigger("Jump");
        soundManager.ActivarSonidoSalto();
        rb.AddForce(rb.mass*Vector3.up * playerCurrentStats.currentStats.jumpForce, ForceMode.Impulse);

        
    }
  

}
