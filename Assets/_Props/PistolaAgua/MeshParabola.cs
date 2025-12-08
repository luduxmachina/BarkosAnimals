using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WaterStreamMeshFijoInicial : MonoBehaviour
{
    [Header("Tiro parabólico relativo a la pistola")]
    public float velocidadInicial = 12f;
    [Range(0f, 90f)] public float angulo = 45f;
    public float gravedad = -9.81f;
    public float alturaInicial = 0f;

    [Header("Mesh")]
    [Range(2, 200)] public int resolucion = 40;
    public float anchoInicio = 0.05f;
    public float anchoFinal = 0.2f;

    [Header("Ondulaciones")]
    public float amplitudBase = 0.02f;
    public float amplitudVariacion = 0.01f;
    public float frecuenciaBase = 3f;
    public float frecuenciaVariacion = 1.5f;
    public float velocidadOscilacion = 1f;

    [Header("Zona fija inicial")]
    [Range(0f, 0.5f)] public float tFijo = 0.1f; // porcentaje inicial que queda quieto

    private Mesh mesh;

    void OnEnable()
    {
        mesh = new Mesh();
        mesh.name = "WaterStreamMeshFijoInicial";
        GetComponent<MeshFilter>().sharedMesh = mesh;
        GenerarMesh();
    }

    void Update()
    {
        GenerarMesh();
    }

    void GenerarMesh()
    {
        if (mesh == null) return;

        mesh.Clear();

        Vector3[] vertices = new Vector3[resolucion * 2];
        Vector2[] uvs = new Vector2[resolucion * 2];
        int[] triangles = new int[(resolucion - 1) * 6];

        float rad = angulo * Mathf.Deg2Rad;
        float vy = velocidadInicial * Mathf.Sin(rad);
        float vz = velocidadInicial * Mathf.Cos(rad);
        float y0 = alturaInicial;

        // Tiempo máximo de vuelo
        float a = 0.5f * gravedad;
        float b = vy;
        float c = y0;
        float disc = b * b - 4 * a * c;
        float tMax = 1f;

        if (disc >= 0)
        {
            float sqrt = Mathf.Sqrt(disc);
            float t1 = (-b + sqrt) / (2 * a);
            float t2 = (-b - sqrt) / (2 * a);
            tMax = Mathf.Max(t1, t2);
            if (tMax <= 0) tMax = 1f;
        }

        float t = Time.time * velocidadOscilacion;
        float amplitud = amplitudBase + Mathf.Sin(t) * amplitudVariacion;
        float frecuencia = frecuenciaBase + Mathf.Sin(t * 0.7f) * frecuenciaVariacion;

        Vector3 origen = transform.position;

        for (int i = 0; i < resolucion; i++)
        {
            float tNorm = i / (float)(resolucion - 1);
            float tVuelo = tNorm * tMax;

            // Posición del chorro
            Vector3 localPos = new Vector3(
                0f,
                y0 + vy * tVuelo + 0.5f * gravedad * tVuelo * tVuelo,
                vz * tVuelo
            );

            // Tangente y perpendicular
            Vector3 tangent = new Vector3(0f, vy + gravedad * tVuelo, vz).normalized;
            Vector3 perp = Vector3.Cross(tangent, Vector3.up).normalized;

            float ancho = Mathf.Lerp(anchoInicio, anchoFinal, tNorm);
            Vector3 offset = Vector3.zero;

            // Solo aplicar ondulación si estamos más allá de la zona fija inicial
            if (tNorm > tFijo)
            {
                float tOndulado = (tNorm - tFijo) / (1f - tFijo); // normalizado 0-1
                offset = perp * Mathf.Sin(tOndulado * frecuencia * Mathf.PI * 2 + t) * amplitud;
            }

            vertices[i * 2 + 0] = localPos + perp * ancho + offset;
            vertices[i * 2 + 1] = localPos - perp * ancho + offset;

            uvs[i * 2 + 0] = new Vector2(tNorm, 0);
            uvs[i * 2 + 1] = new Vector2(tNorm, 1);
        }

        int tris = 0;
        for (int i = 0; i < resolucion - 1; i++)
        {
            int a0 = i * 2;
            int a1 = a0 + 1;
            int b0 = a0 + 2;
            int b1 = a0 + 3;

            triangles[tris++] = a0;
            triangles[tris++] = b0;
            triangles[tris++] = a1;

            triangles[tris++] = a1;
            triangles[tris++] = b0;
            triangles[tris++] = b1;
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
