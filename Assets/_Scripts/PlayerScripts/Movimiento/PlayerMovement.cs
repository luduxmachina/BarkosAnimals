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
            rb.angularVelocity *= 0.05f;
            soundManager.StopMovementLoop();
        }
        if (adaptToFloor.IsGrounded())
        {
            animator.SetTrigger("Idle");

        }
    }
  
    private void MoveAndRotate()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

            //primero solo adapt to floor
            Vector3 currentForward = transform.forward;
            Vector3 currentRight = Vector3.Cross(adaptToFloor.upVector, currentForward);
            currentForward = Vector3.Cross(currentRight, adaptToFloor.upVector);
            Quaternion currentRotation = Quaternion.LookRotation(currentForward, adaptToFloor.upVector);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, currentRotation, playerCurrentStats.currentStats.rotationSpeed * Time.fixedDeltaTime));

            //ahora puto aplicar el intento de giro del player
            Vector3 newForward = move;
            float angleDiff = Vector3.SignedAngle(currentForward, newForward, adaptToFloor.upVector);
            rb.angularVelocity = adaptToFloor.upVector * Mathf.Deg2Rad * angleDiff * Mathf.Deg2Rad * playerCurrentStats.currentStats.rotationSpeed;



            //ultimo moverse
            move = Vector3.ProjectOnPlane(move, adaptToFloor.upVector).normalized;
            rb.MovePosition(rb.position + (move * playerCurrentStats.currentStats.moveSpeed * Time.fixedDeltaTime));



        /*if (move.sqrMagnitude > 0.0001f)
        {

            //primero solo adapt to floor
            Vector3 currentForward = transform.forward;
            Vector3 currentRight = Vector3.Cross(adaptToFloor.upVector, currentForward);
            currentForward = Vector3.Cross(currentRight, adaptToFloor.upVector);
            Quaternion currentRotation = Quaternion.LookRotation(currentForward, adaptToFloor.upVector);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, currentRotation, playerCurrentStats.currentStats.rotationSpeed * Time.fixedDeltaTime));

            //ahora puto aplicar el intento de giro del player
            Vector3 newForward = move;
            float angleDiff = Vector3.SignedAngle(currentForward, newForward, adaptToFloor.upVector);
            rb.angularVelocity = adaptToFloor.upVector * Mathf.Deg2Rad *angleDiff * Mathf.Deg2Rad * playerCurrentStats.currentStats.rotationSpeed;

        }*/
        /* Vector3 forward = move;
           Vector3 right = Vector3.Cross(adaptToFloor.upVector, forward);
           forward = Vector3.Cross(right, adaptToFloor.upVector);





           Quaternion targetRotation = Quaternion.LookRotation(forward, adaptToFloor.upVector);

           float angleDiff = Vector3.SignedAngle(currentForward, forward, adaptToFloor.upVector);
           float torque = angleDiff * Mathf.Deg2Rad * playerCurrentStats.currentStats.rotationSpeed * rb.mass;
           if(Mathf.Abs(torque) < 0.1f)
           {

                rb.angularVelocity *= 0.0f;

                 rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, playerCurrentStats.currentStats.rotationSpeed * Time.fixedDeltaTime));
               return;
           }

           rb.AddRelativeTorque(Vector3.up * torque);
          */

        // rb.angularVelocity *= 0.5f;

        //  rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, playerCurrentStats.currentStats.rotationSpeed * Time.fixedDeltaTime));
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
