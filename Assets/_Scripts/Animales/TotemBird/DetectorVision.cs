using System.Collections.Generic;
using UnityEngine;

public class DetectorVision : MonoBehaviour
{

    [Header("Configuración del Cono")]
    public float radioVision = 10f;    // ditan vision
    [Range(0, 360)]
    public float anguloVision = 90f;   // amplitud de vison



    [Header("Estado para la FSM")]
    public bool hayObjetivosARango;
    public Transform transformObjetivo; // Para coger el pan
    public Color colorVision = Color.green;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [HideInInspector]
    public List<ItemNames> objetivosDetectar = new List<ItemNames>();
    private void Awake()
    {

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Detectar();
    }

    //función que lleva la visión
    void Detectar()
    {


        hayObjetivosARango = false;
        //amenazaDetectada = null;

        // 1. OBTENER POSIBLES OBJETIVOS (Por Distancia)
        // Genera una esfera invisible y nos devuelve todo lo que toque de la capa 'Amenazas'
        Collider[] objetosEnRango = Physics.OverlapSphere(transform.position, radioVision);

        // 2. FILTRAR POR ÁNGULO
        foreach (var objetivo in objetosEnRango)
        {

                Transform target = objetivo.transform;

                // Calculamos la dirección hacia el objetivo
                Vector3 direccionAlObjetivo = (target.position - transform.position).normalized;

                // Verificamos si la dirección está dentro del ángulo de visión frontal
                if (Vector3.Angle(transform.forward, direccionAlObjetivo) < anguloVision / 2)
                {
                    
                    var temp= objetivo.GetComponentInChildren<ItemInScene>();
                if (temp != null && objetivosDetectar.Contains(temp.itemName))
                {
                    // Si hemos llegado hasta aquí, el objetivo está dentro del ángulo de visión
                    hayObjetivosARango = true;
                    transformObjetivo = target;
                    break; // Salimos del bucle ya que hemos encontrado un objetivo válido
                }



            }
      
            
        }
    }
   
    //Para visualizar la visón en el editor
    private void OnDrawGizmos()
    {
        if (hayObjetivosARango)
        {
            Gizmos.color = colorVision;
        }
        else
        {
            Gizmos.color= Color.gray;
        }
        // Dibujar el círculo de distancia
        Gizmos.DrawWireSphere(transform.position, radioVision);

        // Dibujar las líneas del ángulo
        Vector3 lineaIzquierda = DirFromAngle(-anguloVision / 2);
        Vector3 lineaDerecha = DirFromAngle(anguloVision / 2);

        Gizmos.DrawLine(transform.position, transform.position + lineaIzquierda * radioVision);
        Gizmos.DrawLine(transform.position, transform.position + lineaDerecha * radioVision);
    }

    // Función para dibujar más fácil los ángulos
    private Vector3 DirFromAngle(float angleInDegrees)
    {
        angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

}
