using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class WorldItemPlacer : MonoBehaviour
{
    public ItemNames itemName;
    public GridCreator gridCreator;
    
    [SerializeField] 
    private AllObjectTypesSO itemsDataBase;
    [SerializeField]
    private LayerMask heightCheckLayerMask;
    [SerializeField]
    private int numOfItems = 10;
    [SerializeField]
    private int attempts = 20;
    [SerializeField]
    private Transform parentObject;
    [SerializeField]
    private float heightOffset = 1f;

    private void Awake()
    {
        gridCreator.OnItemPlacing.AddListener(StartPlacingItemsInWorld);
    }

    public void StartPlacingItemsInWorld(Vector2 dimensions)
    {
        if (itemsDataBase.ConteinsItem(itemName))
        {
            for (int i = 0; i < numOfItems; i++)
            {
                bool placed = false;
                int attempt = 0;
                while (!placed && attempt < attempts)
                {
                    float x = Random.Range(-dimensions.x * 0.5f, dimensions.x * 0.5f);
                    float z = Random.Range(-dimensions.y * 0.5f, dimensions.y * 0.5f);
                    float y = GetHighestY(new Vector3(x, 0f, z)) + heightOffset;

                    Vector3 pos = new Vector3(x, y, z);
                    GameObject obj = itemsDataBase.GetObjectPrefab(itemName);

                    if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                    {
                        if (parentObject != null)
                        {
                            Instantiate(obj, hit.position, Quaternion.identity, parentObject);
                        }
                        else
                        {
                            Instantiate(obj, hit.position, Quaternion.identity);
                        }

                        placed = true;
                    }

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
