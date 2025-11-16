using UnityEngine;
using UnityEngine.Rendering;

public class SuciedadSpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject suciedadPrefab;
    [SerializeField]
    float spawnInterval = 5f;
    [SerializeField]
    float randomAddedTimeRange = 2f;
    private float timeUntilSpawn = 0f;

    private void Update()
    {
        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn <= 0f)
        {
            SpawnSuciedad();
            timeUntilSpawn = spawnInterval + Random.Range(-randomAddedTimeRange, randomAddedTimeRange);
        }
    }
    private void SpawnSuciedad()
    {

    }

}
