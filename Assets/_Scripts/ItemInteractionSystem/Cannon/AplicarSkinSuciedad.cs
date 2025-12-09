using UnityEngine;

public class AplicarSkinSuciedad : MonoBehaviour
{
    [SerializeField]
    AAnimalFase2 animalFase2;
    [SerializeField]
    Renderer rendererAnimal;
    [SerializeField]
    float empieceAplicarSuciedad = 0.2f;
    [SerializeField]
    float suciedadFinal = 1f;
    [SerializeField]
    float suciedadInicio = 0.0f;
    [SerializeField]
    float lastAppliedSuciedad = 0f;
    void Update()
    {
        float sucideadNormalizada = animalFase2.TimeWithoutShower();
       if(sucideadNormalizada < empieceAplicarSuciedad)
        {
            sucideadNormalizada = 0f;
            if(lastAppliedSuciedad == 0f)
            {
                return;
            }
            rendererAnimal.material.SetFloat("_Suciedad", sucideadNormalizada);
            lastAppliedSuciedad = sucideadNormalizada;

        }
        else
        {

            sucideadNormalizada = Mathf.Clamp(sucideadNormalizada, suciedadInicio, suciedadFinal);

            rendererAnimal.material.SetFloat("_Suciedad", sucideadNormalizada);
            lastAppliedSuciedad = sucideadNormalizada;
        }
    }
}
