using UnityEngine;

public class Suciedad : MonoBehaviour
{
    [SerializeField]
    int maxSuciedad = 50;
    [SerializeField, ReadOnly]
    int nivelSuciedad = 50;
    Vector3 originalLocalScale;

    private void Awake()
    {
        this.tag = "Suciedad";
    }
    private void Start()
    {
        nivelSuciedad = maxSuciedad;
        originalLocalScale = transform.localScale;
        Debug.Log("Original local scale: " + originalLocalScale);
    }
    public void Limpiar()
    {
        nivelSuciedad--;
        Debug.Log("OrinigalLocal scale: " + originalLocalScale);
        transform.localScale = (nivelSuciedad+20)/(float)maxSuciedad * originalLocalScale;
        Debug.Log("New local scale: " + transform.localScale);
        if (nivelSuciedad <= 0)
        {
            LimpiarAlrededor();
            Destroy(this.gameObject);
        }
    }

    private void LimpiarAlrededor()
    {

    }
}
