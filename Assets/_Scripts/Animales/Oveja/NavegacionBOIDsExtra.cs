using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class NavegacionBOIDsExtra : MonoBehaviour
{
    public bool IsActive = true;
    [SerializeField]
    ItemInScene itemInScene;
    [SerializeField]
    float detectionRadius = 5f;
    [SerializeField]
    float forceMultiplier = 1f;
    [SerializeField]
    float ws;
    [SerializeField]
    float wa;
    [SerializeField]
    float wc;
    [SerializeField]
    bool lines=true;

    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsActive) return;

       Transform[] vecinos =  IslandPositions.instance.GetAll(itemInScene.itemName);
        Vector3 separacion = Vector3.zero;
        Vector3 alineacionMedia = Vector3.zero;
        Vector3 cohesionMedia = Vector3.zero;
        int numVecinos = 0;
        foreach (var vecino in vecinos)
        {
            if(vecino == this.transform) continue;
            if(Vector3.Distance(transform.position, vecino.position) > detectionRadius) continue;

            //estos son los considerados vecinos
            if (lines)
            {
                Debug.DrawLine(transform.position, vecino.position, Color.green);
            }
            //separacion
            separacion += (transform.position - vecino.position) / Vector3.Distance(transform.position, vecino.position);
            //alineacionMedia
            alineacionMedia +=  vecino.forward;
            //cohesionMedia
            cohesionMedia += vecino.position;

            numVecinos++;

        }
        if (numVecinos == 0)
        {
            return;
        }

        separacion /= (numVecinos);
        alineacionMedia /= (numVecinos);
        cohesionMedia /= (numVecinos);

        Vector3 alineacion = alineacionMedia - transform.forward;
        Vector3 cohesion = cohesionMedia - transform.position;
        Vector3 fuerza = separacion * ws + alineacion * wa + cohesion * wc;


        
        rb.AddForce(fuerza * forceMultiplier);

        //girar que mire hacia donde anda
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rb.linearVelocity.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
        }
    }

}
