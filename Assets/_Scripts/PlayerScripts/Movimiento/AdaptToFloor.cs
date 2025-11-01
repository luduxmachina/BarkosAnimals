
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class AdaptToFloor : MonoBehaviour
{
    private Rigidbody rb;
    public Vector3 upVector = new Vector3(0, 1, 0);
    [Header("Ground Check")]
    [SerializeField] private float heightFromGround = 1.1f;
    [SerializeField] private float groundAdaptationSpeed = 0.2f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void FixedUpdate()
    {

            AdaptBodyToFloor();

    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, heightFromGround);
    }
    private void AdaptBodyToFloor() //lets go gepeto
    {

        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);


        if (Physics.Raycast(ray, out RaycastHit hit, heightFromGround))
        {
            upVector = hit.normal;
            Vector3 right = transform.right;

            // up comes from the ground normal (smoothed or direct)
            Vector3 up = upVector;

            // recompute forward as orthogonal cross product
            Vector3 forward = Vector3.Cross(right, up).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(forward, up);
            // rebuild rotation from those three axes
            rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, groundAdaptationSpeed * Time.fixedDeltaTime);
        }

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - Vector3.up * heightFromGround);
        Gizmos.DrawSphere(transform.position - (Vector3.up * (heightFromGround - 0.1f)), 0.1f);
    }

}
