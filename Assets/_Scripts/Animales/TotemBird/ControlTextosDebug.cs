using TMPro;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public DetectorVision visionPajaro;
    public TextMeshProUGUI textoPeligro;
    public TextMeshProUGUI textoPan;
    public TextMeshProUGUI textoADistancia;

    // Update is called once per frame
    void Update()
    {
        CambiarTexto();
    }
    public void CambiarTexto() 
    {
       /* if (visionPajaro.hayPeligro) { 
            textoPeligro.text = "Hay Peligro";
            textoPeligro.color = Color.red;
        }
        else
        {
            textoPeligro.text = "No hay Peligro";
            textoPeligro.color = Color.green;
        }

        if (visionPajaro.hayPan)
        {
            textoPan.text = "Hay Pan";
            textoPan.color = Color.green;
        }
        else
        {
            textoPeligro.text = "No hay Pan";
            textoPeligro.color = Color.red;
        }

        if (visionPajaro.hayObjetivosARango)
        {
            textoPeligro.text = "Hay objetos en mi rango";
            textoPeligro.color = Color.green;
        }
        else
        {
            textoPeligro.text = "No hay objetos en mi rango";
            textoPeligro.color = Color.red;
        }*/

    }
}
