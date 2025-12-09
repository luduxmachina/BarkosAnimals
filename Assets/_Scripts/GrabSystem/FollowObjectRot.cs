using UnityEngine;

public class FollowObjectRot : MonoBehaviour
{
    
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        
        if(target == null)
        {
           // Debug.LogWarning("Ey yooo)");
          DestroyImmediate(this.gameObject);
            return;
        }
        transform.rotation = target.rotation;

        // transform.position = target.position;
    }
}
