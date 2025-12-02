using Unity.VisualScripting;
using UnityEngine;

public class CannonAgua : MonoBehaviour
{
    [SerializeField]
    WaterStreamMeshFijoInicial agua;
    float intensidadActual = 0f;

    float anchoInicialMax;
    float anchoFinalMax;
    float velocidadInicialMax = 12f;
     float anguloMax = 45f;
     float gravedadMax = -9.81f;
    private void Start()
    {
        anchoFinalMax = agua.anchoFinal;
        anchoInicialMax = agua.anchoInicio;
        velocidadInicialMax = agua.velocidadInicial;
        anguloMax = agua.angulo;
        gravedadMax = agua.gravedad;

        agua.gameObject.SetActive(false);
        

    }
    public void Fire() //se llama en fixed
    {
        if(intensidadActual <= 0f)
        {
            intensidadActual = 0.5f;
        }
        else
        {
            intensidadActual += 3*Time.fixedDeltaTime;
        }
        if(intensidadActual > 1.0f)
        {
            intensidadActual = 1.0f;
        }

        
    }
    public void FixedUpdate()
    {
        intensidadActual -= 2*Time.fixedDeltaTime;
       
        ShowWater();
    }
    void ShowWater()
    {
        if(intensidadActual <= 0f)
        {
            agua.gameObject.SetActive(false);
        }
        else
        {
            agua.gameObject.SetActive(true);
        }

        agua.anchoInicio = anchoInicialMax * intensidadActual;
        agua.anchoFinal = anchoFinalMax * intensidadActual;
        agua.velocidadInicial = velocidadInicialMax * intensidadActual;
        agua.angulo = anguloMax * intensidadActual;
        agua.gravedad = gravedadMax * intensidadActual;



        //tocar otros parametros
    }
}
