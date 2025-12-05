using UnityEngine;

public class FollowObjectRot : MonoBehaviour
{
    
    public Transform target;

    // Update is called once per frame
    void Update()
    {
       // transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
