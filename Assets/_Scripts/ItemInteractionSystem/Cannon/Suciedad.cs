using UnityEngine;

public class Suciedad : MonoBehaviour
{
    [SerializeField]
    int maxSuciedad = 50;
    [SerializeField, ReadOnly]
    int nivelSuciedad = 50;
    Vector3 originalLocalScale;

    Stable stable;

    public void SetStable(Stable stable)
    {
        this.stable = stable;
    }

    private void Awake()
    {
        this.tag = "Suciedad";
    }
    private void Start()
    {
        nivelSuciedad = maxSuciedad;
        originalLocalScale = transform.localScale;
    }
    public void Limpiar()
    {
        nivelSuciedad--;
        transform.localScale = (nivelSuciedad+(maxSuciedad*0.5f))/(float)maxSuciedad * originalLocalScale;
        if (nivelSuciedad <= 0)
        {
            LimpiarEstablo();
            Destroy(this.gameObject);
        }
    }

    private void LimpiarEstablo()
    {
        foreach (AAnimalFase2 animal in stable.animalesReferecia)
        {
            animal.AMax = 0f;
        }
    }
}
