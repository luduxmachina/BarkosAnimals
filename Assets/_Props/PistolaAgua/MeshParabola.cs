using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WaterStreamMeshFijoInicial : MonoBehaviour
{
    [Header("Tiro parabólico")]
    public float velocidadInicial = 12f;
    [Range(-90f, 90f)] public float angulo = 0f; // Ajustado para sumar a la rotación del objeto
    public float gravedad = -9.81f;
    public float alturaInicial = 0f;

    [Header("Configuración del Tubo")]
    public int segmentosRadiales = 8;
    public float radioInicial = 0.1f;
    public float radioFinal = 0.2f;

    [Header("Mesh")]
    [Range(2, 200)] public int resolucion = 40;
    public float tiempoSimulacion = 2.0f; // Cuánto tiempo dura el chorro en el aire

    [Header("Ondulaciones")]
    public float amplitudBase = 0.02f;
    public float frecuenciaBase = 3f;
    public float velocidadOscilacion = 5f;

    // ESTA ES LA LISTA QUE FALTABA
    private List<Vector3> puntosDeLaParabola = new List<Vector3>();

    private Mesh mesh;
    private MeshFilter meshFilter;

    void OnEnable()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter.sharedMesh == null)
        {
            mesh = new Mesh();
            mesh.name = "WaterStreamCylinderMesh";
            meshFilter.sharedMesh = mesh;
        }
        else
        {
            mesh = meshFilter.sharedMesh;
        }
    }

    void Update()
    {
        // 1. Primero calculamos la física (dónde están los puntos)
        CalcularTrayectoria();

        // 2. Luego dibujamos el tubo alrededor de esos puntos
        GenerarMesh();
    }

    // --- NUEVA FUNCIÓN: CALCULA LA FÍSICA ---
    void CalcularTrayectoria()
    {
        puntosDeLaParabola.Clear();

        // Calculamos el vector de velocidad inicial basándonos en la rotación del objeto + el ángulo extra
        float anguloEnRadianes = -angulo * Mathf.Deg2Rad; // Negativo para que positivo sea hacia arriba

        // Dirección base del objeto (hacia adelante)
        Vector3 forward = transform.forward;
        Vector3 up = transform.up;

        // Rotamos el vector forward en el eje local X para aplicar el ángulo de disparo
        Quaternion rotacionDisparo = Quaternion.AngleAxis(angulo, -transform.right);
        Vector3 velocidadVector = rotacionDisparo * forward * velocidadInicial;

        Vector3 startPos = transform.position + (Vector3.up * alturaInicial);

        for (int i = 0; i < resolucion; i++)
        {
            float t = ((float)i / (resolucion - 1)) * tiempoSimulacion;

            // Fórmula física básica: Pos = V0*t + 0.5*g*t^2
            Vector3 desplazamiento = (velocidadVector * t) + (0.5f * new Vector3(0, gravedad, 0) * (t * t));

            // Añadimos las ONDULACIONES (Wobble effect)
            if (Application.isPlaying && amplitudBase > 0)
            {
                // Un pequeño movimiento en el eje derecho local para simular turbulencia
                float wobble = Mathf.Sin((Time.time * velocidadOscilacion) + (t * frecuenciaBase)) * amplitudBase;
                desplazamiento += transform.right * wobble;
            }

            puntosDeLaParabola.Add(startPos + desplazamiento);
        }
    }

    // --- TU FUNCIÓN DE MALLA (CORREGIDA) ---
    void GenerarMesh()
    {
        if (puntosDeLaParabola.Count < 2) return;

        // Limpiamos la malla anterior
        mesh.Clear();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangulos = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int i = 0; i < puntosDeLaParabola.Count; i++)
        {
            float t = (float)i / (puntosDeLaParabola.Count - 1);
            float radioActual = Mathf.Lerp(radioInicial, radioFinal, t);

            // Calcular dirección
            Vector3 direccion;
            if (i < puntosDeLaParabola.Count - 1)
            {
                direccion = (puntosDeLaParabola[i + 1] - puntosDeLaParabola[i]).normalized;
            }
            else
            {
                direccion = (puntosDeLaParabola[i] - puntosDeLaParabola[i - 1]).normalized;
            }

            // Evitar error si la dirección es cero (puede pasar al inicio)
            if (direccion == Vector3.zero) direccion = transform.forward;

            Quaternion rotacion = Quaternion.LookRotation(direccion);

            // Generar anillo
            for (int s = 0; s <= segmentosRadiales; s++)
            {
                float anguloRad = (float)s / segmentosRadiales * Mathf.PI * 2;

                float x = Mathf.Cos(anguloRad) * radioActual;
                float y = Mathf.Sin(anguloRad) * radioActual;

                Vector3 posLocal = new Vector3(x, y, 0);
                Vector3 posFinal = puntosDeLaParabola[i] + (rotacion * posLocal);

                // Convertir al espacio local del objeto para que el mesh se mueva con la pistola
                // (Opcional: si quieres que el chorro sea world-space, deja posFinal tal cual y pon el transform del objeto en 0,0,0)
                // Para simplificar, aquí usaremos World Space relativo visualmente, 
                // pero asignamos al mesh. Como el objeto se mueve, necesitamos restar la posición del objeto
                // para que los vértices sean locales al transform.

                vertices.Add(transform.InverseTransformPoint(posFinal));

                uvs.Add(new Vector2((float)s / segmentosRadiales, t));
            }
        }

        // Generar Triángulos
        for (int i = 0; i < puntosDeLaParabola.Count - 1; i++)
        {
            for (int s = 0; s < segmentosRadiales; s++)
            {
                int actual = (i * (segmentosRadiales + 1)) + s;
                int siguiente = actual + (segmentosRadiales + 1);

                triangulos.Add(actual);
                triangulos.Add(siguiente);
                triangulos.Add(actual + 1);

                triangulos.Add(siguiente);
                triangulos.Add(siguiente + 1);
                triangulos.Add(actual + 1);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangulos.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds(); // Importante para que no desaparezca si miras de lado
    }
}