using UnityEngine;


public class SuciedadSpawner : MonoBehaviour
{
    [SerializeField]
    Sprite[] suciedadSprites;
    [SerializeField]
    float suciedadMediaSpawn = 50f;
    [SerializeField]
    float RandomOffsetMedia = 5f;
    [SerializeField]
    Stable stable;
    [SerializeField]
    private GameObject suciedadPrefab;
    [SerializeField]
    private Transform centroEstablo;
    [SerializeField, Tooltip("para spawnear en este cuadrado las suciedades")]
    private float dimensionEstablo;
    [SerializeField]
    float tiempoEntreDosSuciedades = 10.0f;
    float currentTime;
    bool spawned=false;
    private void Start()
    {
        suciedadMediaSpawn += Random.Range(-RandomOffsetMedia, RandomOffsetMedia);
    }
    private void Update()
    {
        if (spawned)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= tiempoEntreDosSuciedades)
            {
                currentTime = 0;
                spawned = false;
            }
        }
        else
        {
            float media=0.0f;
            foreach (AAnimalFase2 animal in stable.animalesReferecia)
            {
                media += animal.TimeWithoutEating();
            }
            media /= stable.animalesReferecia.Count;
            if (media >= suciedadMediaSpawn)
            {
                SpawnSuciedad();
                currentTime = 0;
                spawned = true;
            }
        }
    }
    public void SpawnSuciedad()
    {
        Vector3 pos = centroEstablo.position + new Vector3(Random.Range(-dimensionEstablo/2, dimensionEstablo/2), 0, Random.Range(-dimensionEstablo/2, dimensionEstablo/2));
        GameObject suciedad = Instantiate(suciedadPrefab, pos, Quaternion.identity);
        suciedad.GetComponent<Suciedad>().SetStable(stable);
        if(suciedadSprites.Length == 0) return;
        int randomIndex= Random.Range(0, suciedadSprites.Length);
        suciedad.GetComponentInChildren<SpriteRenderer>().sprite = suciedadSprites[randomIndex];

    }

}
