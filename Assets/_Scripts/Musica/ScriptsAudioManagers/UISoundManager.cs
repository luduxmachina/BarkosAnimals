using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    [Header("Efectos UI")]
    public AudioSource pulsaBotonSound;
    public AudioSource ConfirmarSound;
    public AudioSource CancelarSound;
    public AudioSource MaximizarSound;
    public AudioSource MinimizarSound;

    public void ActivarSonidoBoton()
    {

        pulsaBotonSound.Play();

    }
    public void ActivarSonidoMaximizar()
    {

        MaximizarSound.Play();

    }
    public void ActivarSonidoMinimizar()
    {

        MinimizarSound.Play();

    }
    public void ActivarSonidoConfirmar()
    {

        ConfirmarSound.Play();

    }
    public void ActivarSonidoCancelar()
    {

        CancelarSound.Play();

    }

}
