using UnityEngine;

public class DetectorVision : MonoBehaviour
{
    [Header("Configuración del Cono")]
    public float radioVision = 10f;    // ditan vision
    [Range(0, 360)]
    public float anguloVision = 90f;   // amplitud de vison

    [Header("Configuración de Detección")]
    public LayerMask capaAmenazas;     // para detectar amenazas
    public LayerMask capaPan;   // para detectar panes

    [Header("Estado para la FSM")]
    public bool hayPeligro;    // percepción hay peligro
    public bool hayPan;    // percepción hay panes
    public bool hayObjetivosARango;
    public Transform transformPan; // Para coger el pan
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        // Por defecto no hay peligro hasta que demostremos lo contrario
        hayPeligro = false;
        hayPan = false;
        hayObjetivosARango = false;
        //amenazaDetectada = null;

        // 1. OBTENER POSIBLES OBJETIVOS (Por Distancia)
        // Genera una esfera invisible y nos devuelve todo lo que toque de la capa 'Amenazas'
        Collider[] amenazasEnRango = Physics.OverlapSphere(transform.position, radioVision, capaAmenazas);

        // 2. FILTRAR POR ÁNGULO
        foreach (var objetivo in amenazasEnRango)
        {
            Transform target = objetivo.transform;

            // Calculamos la dirección hacia el objetivo
            Vector3 direccionAlObjetivo = (target.position - transform.position).normalized;

            // Verificamos si la dirección está dentro del ángulo de visión frontal
            if (Vector3.Angle(transform.forward, direccionAlObjetivo) < anguloVision / 2)
            {
                // ¡AMENAZA CONFIRMADA!
                hayPeligro = true;
                //amenazaDetectada = target;

                // Si solo te importa detectar AL MENOS UNO, podemos salir del bucle ya.
                break;
            }
        }
    }

    //Para visualizar la visón en el editor
    private void OnDrawGizmos()
    {
        Gizmos.color = hayPeligro ? Color.red : Color.green; // Rojo si detecta, Verde si no

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
