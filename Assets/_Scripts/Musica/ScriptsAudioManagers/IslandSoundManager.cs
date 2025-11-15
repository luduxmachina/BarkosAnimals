using System.Collections;
using UnityEngine;


public class IslandSoundManager : MonoBehaviour
{
    [Header("Música")]
    public AudioSource musicaIsla;
    public AudioSource musicaPausa;

    [Header("Efectos de Ambiente (SFX)")]
    public AudioSource sfxGaviota;
    public AudioSource sfxOlas;

    [Header("Tiempos Aleatorios (Min y Max segundos)")]
    public Vector2 tiempoGaviota = new Vector2(5f, 15f);
    public Vector2 tiempoOlas = new Vector2(3f, 8f);

    private bool estaEnPausa = false;


    void Start()
    {
        
        ActivarModoJuego();
    }  
    public void CambiarEstadoPausa(bool pausaActivada)
    {
        if (pausaActivada)
        {
            Debug.Log("Entrando en PAUSA..."); 
            ActivarModoPausa();
        }
        else
        {
            Debug.Log("Volviendo al JUEGO..."); 
            ActivarModoJuego();
        }
    }
    private void OnEnable()
    {
        MenuPause.OnPause.AddListener(ActivarModoPausa);
        MenuPause.OnResume.AddListener(ActivarModoJuego);
    }

    private void OnDisable()
    {
        MenuPause.OnPause.RemoveListener(ActivarModoPausa);
        MenuPause.OnResume.RemoveListener(ActivarModoJuego);
    }
    void ActivarModoJuego()
    {
        
        musicaPausa.Stop(); 

        if (!musicaIsla.isPlaying)
        {
            musicaIsla.Play(); // si no sonaba play
        }
        else
        {
            musicaIsla.UnPause(); // Si sonaba despausamos
        }

        //Activamos corutina de efectos
        StopAllCoroutines(); // Limpieza de otras corrutinas residuales
        StartCoroutine(ReproducirAleatorio(sfxGaviota, tiempoGaviota));
        StartCoroutine(ReproducirAleatorio(sfxOlas, tiempoOlas));
    }

    void ActivarModoPausa()
    {
        // 1. Música
        musicaIsla.Pause(); // Pausamos la isla (no Stop, para no perder el punto)

        if (!musicaPausa.isPlaying)
        {
            musicaPausa.Play();
        }

        // 2. Efectos
        StopAllCoroutines(); // Detienemos corutinas y los sonidos asociados
        sfxGaviota.Stop();   
        sfxOlas.Stop();
    }

    IEnumerator ReproducirAleatorio(AudioSource fuente, Vector2 tiempos)
    {
        while (!estaEnPausa) // Mientras NO estemos en pausa
        {
            float tiempoEspera = Random.Range(tiempos.x, tiempos.y);
            yield return new WaitForSeconds(tiempoEspera);

            // Doble chequeo antes de sonar
            if (!estaEnPausa && !fuente.isPlaying)
            {
                fuente.Play();
            }
        }
    }
}


