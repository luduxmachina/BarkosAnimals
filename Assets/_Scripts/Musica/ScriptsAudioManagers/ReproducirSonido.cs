using UnityEngine;

public class ReproducirSonido : MonoBehaviour
{
    public AudioSource SonidoAReproducir;

    public void ActivarSonidoBoton()
    {

        SonidoAReproducir.Play();

    }
}
