
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class LazyDetector : MonoBehaviour, ITargeter
{
    private enum DetectionShape
    {
        Sphere,
        Box,
    }
    [Header("Physics")]
    [SerializeField] float detectionRange = 2f;
    [SerializeField] DetectionShape shape = DetectionShape.Sphere;

    [Header("Settings")]
    [SerializeField] bool filterByTag = false;
    [SerializeField, HideIf("filterByTag", false)] string targetTag = "";
    [SerializeField] bool useOtherTags = false;
    [SerializeField, HideIf("useOtherTags", false)] string[] otherTags = new string[0];
    [SerializeField] bool filterByLayer = false;
    [SerializeField, HideIf("filterByLayer", false)] LayerMask targetLayerMask = ~0;


    public GameObject[] GetAllTargets()
    {

        GameObject[] targets = GetPhysicsTargets();
        
        if(filterByTag)
        {
            targets = ApplyTagFilters(targets);
        }
        if(filterByLayer)
        {
            targets = ApplyLayerFilters(targets);
        }

        return targets;
    }

    public GameObject GetTarget()
    {
        return GetClosestItem(GetAllTargets());
    }
    /// <summary>
    /// En LazyDetector, no usais HasTarget y luego GetTarget porque va a hacer dos detecciones fisicas. Usad solo GetTarget y manejad el posible null
    /// </summary>
    /// <returns></returns>
    public bool HasTarget()
    {
        return GetAllTargets().Length > 0;
    }
    private GameObject[] GetPhysicsTargets()
    {
        Collider[] hits = new Collider[0];
        if (shape == DetectionShape.Sphere)
        {
            hits = Physics.OverlapSphere(transform.position, detectionRange, layerMask);
        }
        else if (shape == DetectionShape.Box)
        {
            hits = Physics.OverlapBox(transform.position, Vector3.one * detectionRange * 0.5f, Quaternion.identity, layerMask);
        }
        GameObject[] gameObjects = hits.Select(c => c.gameObject).ToArray();


        return gameObjects;
    }
    private GameObject[] ApplyTagFilters(GameObject[] targets)
    {
        List<GameObject> filtered = new List<GameObject>();
        foreach (var target in targets)
        {

            if (target.CompareTag(targetTag))
            {
                filtered.Add(target);
                break;
            }
            else if (useOtherTags)
            {
                if (otherTags.Contains(target.tag))
                {
                    filtered.Add(target);
                    break;

                }
            }

        }
        return filtered.ToArray();
    }
    private GameObject[] ApplyLayerFilters(GameObject[] targets)
    {
     

        List<GameObject> filtered = new List<GameObject>();
        foreach (var target in targets)
        {
            if (((1 << target.layer) & targetLayerMask) != 0)
            {
                filtered.Add(target);
            }
        }
        return filtered.ToArray();
    }
    private GameObject GetClosestItem(GameObject[] targets)
    {
        GameObject closestItem = targets[0];
        float closestDistance = Mathf.Infinity;

 
        Vector3 cubePosition = transform.position;

   
        foreach (GameObject item in targets)
        {
           
    
            float distanceToCube = Vector3.Distance(cubePosition, item.transform.position);

         
            if (distanceToCube < closestDistance)
            {
                closestDistance = distanceToCube;
                closestItem = item;
            }
        }


        return closestItem; 
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (shape == DetectionShape.Sphere)
        {
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
        else if (shape == DetectionShape.Box)
        {
            Gizmos.DrawWireCube(transform.position, Vector3.one * detectionRange);
        }
    }



}
