using UnityEngine;

public class HeightAdjustedHinge : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    Transform rotationTransform;
    [SerializeField]
    float distanceToGround = 1.0f;
    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }
    private void FixedUpdate()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("Default");
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f, layerMask))
        {
            float heightDiff = hit.distance - distanceToGround;
            transform.position -= new Vector3(0, heightDiff, 0);
        }
        rb.MovePosition(transform.position); //para que se acutalice la pos del rigid body, no lo puedo poner congelado porque entonces no me coge los cambios el hinge cuando no giro lol
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position - Vector3.up * 10f);
        Gizmos.DrawSphere(transform.position - Vector3.up * distanceToGround, 0.1f);
    }
}
