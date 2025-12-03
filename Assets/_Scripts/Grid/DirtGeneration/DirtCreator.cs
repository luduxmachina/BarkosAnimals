using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class DirtCreator : MonoBehaviour
{
    [SerializeField] 
    private GameObject dirtPrefab;
    [SerializeField] 
    private Transform dirtParent;
    
    [SerializeField]
    private Grid grid;

    [SerializeField] 
    private int placementTries = 5;
    [SerializeField] 
    private float yOffset = 0.1f;
    [SerializeField]
    private float cooldown = 5f;
    [SerializeField] 
    private float probabilityToSpreadNearOtherDirt = 0.6f;

    [SerializeField] 
    private float dirtAnimationTime = 0.3f;
    [SerializeField] 
    private float dirtStartScale = 0.1f;
    
    public UnityEvent<Vector2Int> onDirtPlacedGrid = new UnityEvent<Vector2Int>();
    public UnityEvent<Vector3> onDirtPlacedWorld = new UnityEvent<Vector3>();

    private HashSet<Vector2Int> dirtsPlaced = new HashSet<Vector2Int>();
    
    private Vector2Int gridSize = new Vector2Int(10,10);
    private bool startPlacing = false;
    private float timer = 0f;

    private void Update()
    {
        if(!startPlacing)
            return;

        timer += Time.deltaTime;
        if(timer > cooldown)
        {
            PlaceDirt();
            timer = 0f;
        }
    }

    public void StartPlacingDirt()
    {
        SetPlacingDirt(true);
    }

    public void StopPlacingDirt()
    {
        SetPlacingDirt(false);
    }

    public void SetPlacingDirt(bool placeDirt)
    {
        startPlacing =  placeDirt;
    }

    public void RemoveDirt(Vector3 dirtWorldPosition)
    {
        Vector3Int pos3 = grid.WorldToCell(dirtWorldPosition);
        Vector2Int pos = new Vector2Int(pos3.x, pos3.z);

        if (dirtsPlaced.Contains(pos))
        {
            dirtsPlaced.Remove(pos);
        }
        else
        {
            Debug.LogError($"No dirts placed at {dirtWorldPosition}");
        }
    }

    public void ConfigureGrid(CustomBoolMatrix boolMatrix)
    {
        gridSize.x = boolMatrix.GetRows();
        gridSize.y = boolMatrix.GetColums();
    }

    /// <summary>
    /// Checks how much dirt is near a specific world position
    /// </summary>
    /// <param name="worldPosition">The point to check near to</param>
    /// <param name="range">The range in witch it checks</param>
    /// <returns>The amount of dirt near the position at that range or less</returns>
    public int GetHowMuchDirtIsNear(Vector3 worldPosition, float range)
    {
        int count = 0;
        float rangeSqr = range * range;

        foreach (Vector2Int dirtPos in dirtsPlaced)
        {
            Vector3 posToCheck = new Vector3(dirtPos.x, 0f, dirtPos.y);
            
            if ((posToCheck - worldPosition).sqrMagnitude <= rangeSqr)
            {
                count++;
            }
        }

        return count;
    }


    private void PlaceDirt()
    {
        if (gridSize.sqrMagnitude < 0.001f)
            return;
        
        if (dirtsPlaced.Count <= 0) // No dirt in scene
        {
            PlaceDirtAlone();
        }
        else // Dirt in scene
        {
            if (Random.value < probabilityToSpreadNearOtherDirt)
            {
                PlaceDirtNextTo();
            }
            else
            {
                PlaceDirtAlone();
            }
        }
    }

    private void PlaceDirtNextTo()
    {
        int n = dirtsPlaced.Count;
        int randIndex = Random.Range(0, n);
        Vector2Int lookingAtPos = dirtsPlaced.ToArray()[randIndex];

        Vector2Int gridPos;
        int i = 0;
        do
        {
            gridPos = new Vector2Int(lookingAtPos.x + Random.Range(-1, 2), lookingAtPos.y + Random.Range(-1, 2));
            
            i++;
            if (i > placementTries)
                return;
        } while (dirtsPlaced.Contains(gridPos));
        
        PlaceDirtAt(gridPos);
    }

    private void PlaceDirtAlone()
    {
        int rows = gridSize.x;
        int cols = gridSize.y;
        
        int x = 0;
        int y = 0;

        Vector2Int gridPos;
        int i = 0;
        do
        {
            x = (int)Random.Range(-rows * 0.5f, rows * 0.5f);
            y = (int)Random.Range(-cols * 0.5f, cols * 0.5f);
            gridPos = new Vector2Int(x, y);
            
            i++;
            if(i > placementTries)
                return;
            
        } while (dirtsPlaced.Contains(gridPos));

        PlaceDirtAt(gridPos);
    }

    private void PlaceDirtAt(Vector2Int gridPos)
    {
        dirtsPlaced.Add(gridPos);

        Vector3 pos = grid.GetCellCenterWorld(new Vector3Int(gridPos.x, 0, gridPos.y));
        float y = pos.y + yOffset - grid.cellSize.y * 0.5f;
        pos = new Vector3(pos.x, y, pos.z);

        GameObject dirt;
        float randomY = Random.Range(0f, 360f);
        Quaternion randomRotation = Quaternion.Euler(0, randomY, 0);
        
        if (dirtPrefab != null)
        {
            dirt = Instantiate(dirtPrefab, pos, randomRotation, dirtParent);
        }
        else
        {
            dirt = Instantiate(dirtPrefab, pos, randomRotation);
        }


        dirt.AddComponent<DirtInstance>();
        DirtInstance  dirtInstance = dirt.GetComponent<DirtInstance>();
        dirtInstance.dirtCreator = this;
        dirtInstance.duration = dirtAnimationTime;
        dirtInstance.startScale =  dirtStartScale;
        
        onDirtPlacedGrid?.Invoke(gridPos);
        onDirtPlacedWorld?.Invoke(pos);
    }
}
