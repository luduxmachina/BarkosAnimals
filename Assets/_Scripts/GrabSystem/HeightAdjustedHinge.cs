using UnityEngine;

public class HeightAdjustedHinge : MonoBehaviour
{
    [SerializeField]
    Transform rotationTransform;
    [SerializeField]
    float distanceToGround = 1.0f;
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
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position - Vector3.up * 10f);
        Gizmos.DrawSphere(transform.position - Vector3.up * distanceToGround, 0.1f);
    }
}
