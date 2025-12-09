using System.Linq;
using UnityEngine;

public class BloqueAgua : CannonAgua
{
    [SerializeField]
    Transform[] bloquesAgua;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Vector3[] originalScales;
    protected override void Start()
    {
        originalScales = bloquesAgua.Select(b => b.localScale).ToArray();
    }
    protected override void ShowWater()
    {

            for(int i= 0; i < bloquesAgua.Length; i++)
            {
                Transform child = bloquesAgua[i];
                if(i < Mathf.RoundToInt(intensidadActual * bloquesAgua.Length))
                {
                    child.gameObject.SetActive(true);
                    child.localScale = originalScales[i] * (intensidadActual);
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
    

      
    }
}
