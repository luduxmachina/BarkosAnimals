using System.Collections.Generic;
using UnityEngine;

public class Suciedad : MonoBehaviour
{
    [SerializeField]
    int maxSuciedad = 50;
    [SerializeField, ReadOnly]
    int nivelSuciedad = 50;
    Vector3 originalLocalScale;
    [SerializeField]
    List<SpriteRenderer> otrosSpritesSuciedad;

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
        if(nivelSuciedad== maxSuciedad / 3 || nivelSuciedad == 2*maxSuciedad / 3) //quitar otros sprites para ir reduciendo visualmente la suciedad
        {
            if(otrosSpritesSuciedad.Count > 0)
            {
                otrosSpritesSuciedad[0].gameObject.SetActive(false);
                otrosSpritesSuciedad.RemoveAt(0);

            }
        }
        if (nivelSuciedad <= 0)
        {
            Destroy(this.gameObject);
        }
    }

}
