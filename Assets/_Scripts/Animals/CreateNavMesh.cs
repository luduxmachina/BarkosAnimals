
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;

public class CreateNavMesh : MonoBehaviour
{
    public NavMeshSurface navMesh;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMesh = GetComponent<NavMeshSurface>();
        navMesh.BuildNavMesh();
    }
}
