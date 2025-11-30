using UnityEngine;
using UnityEngine.AI;

public class WorldItemPlacer : MonoBehaviour
{
    [SerializeField] 
    private AllObjectTypesSO itemsDataBase;
    [SerializeField]
    private LayerMask heightCheckLayerMask;
    [SerializeField]
    private int numOfItemsOfEach = 10;
    [SerializeField]
    private int attempts = 20;
    [SerializeField]
    private Transform parentObject;
    [SerializeField]
    private float heightOffset = 1f;

    public void StartPlacingItemsInWorld(Vector2 dimensions)
    {
        foreach (var item in itemsDataBase.itemsData)
        {
            for (int i = 0; i < numOfItemsOfEach; i++)
            {
                bool placed = false;
                int attempt = 0;
                while (!placed && attempt < attempts)
                {
                    float x = Random.Range(-dimensions.x * 0.5f, dimensions.x * 0.5f);
                    float z = Random.Range(-dimensions.y * 0.5f, dimensions.y * 0.5f);
                    float y = GetHighestY(new Vector3(x, 0f, z)) + heightOffset;
                
                    Vector3 pos = new Vector3(x, y, z);
                    GameObject obj = item.Prefab;
                
                    if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                    {
                        if(parentObject != null)
                        {
                            Instantiate(obj, hit.position, Quaternion.identity, parentObject);
                        }
                        else
                        {
                            Instantiate(obj, hit.position, Quaternion.identity);
                        }
                        
                        placed =  true;
                    }
                
                    // obj.transform.position = pos;
                    attempt++;
                }
                
            }
        }
    }
    
    private float GetHighestY(Vector3 position)
    {
        Vector3 start = new Vector3(position.x, 10000f, position.z);
        Vector3 direction = Vector3.down;

        RaycastHit hit;

        if (Physics.Raycast(start, direction, out hit, Mathf.Infinity, heightCheckLayerMask))
        {
            return hit.point.y;   // Y del punto de colisi�n
        }

        return 0; // No golpe� nada
    }
}
