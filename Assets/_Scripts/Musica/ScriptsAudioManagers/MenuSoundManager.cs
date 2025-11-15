using UnityEngine;
using System.Collections;

public class MenuSoundManager : MonoBehaviour
{
    [Header("Música")]
    public AudioSource musicaPantallaInicio;
    public AudioSource musicaPantallaAjustes;

    private bool estaEnPausa = false;


    void Start()
    {

        ActivarMusicaInicio();
    }
    public void CambiarEstadoAjustes()
    {
        bool pausaActivada = this.estaEnPausa;
        pausaActivada = !pausaActivada;
        if (pausaActivada)
        {
            Debug.Log("Entrando menu ajustes...");
            ActivarMusicaAjustes();
        }
        else
        {
            Debug.Log("Volviendo a la pantalla inicio...");
            ActivarMusicaInicio();
        }
    }

    public void ActivarMusicaInicio()
    {
        musicaPantallaAjustes.Stop();
        musicaPantallaInicio.Play(); 
        
    }

    public void ActivarMusicaAjustes()
    {
        
        musicaPantallaInicio.Stop(); 

        if (!musicaPantallaAjustes.isPlaying)
        {
            musicaPantallaAjustes.Play();
        }

       
        
    }

   
}
