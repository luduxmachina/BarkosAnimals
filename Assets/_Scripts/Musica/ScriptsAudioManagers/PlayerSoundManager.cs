using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [Header("Efectos Barkito")]
    public AudioSource MovementSound;
    public AudioSource JumpSound;
    public AudioSource PickUpSound;
    public AudioSource DashSound;


    public void StartMovementLoop()
    {
        // Lógica clave: Solo arrancar el sonido si NO está sonando ya.
        if (MovementSound != null && !MovementSound.isPlaying)
        {
            MovementSound.Play();
        }
    }

    public void StopMovementLoop()
    {
        // Detener el sonido cuando el personaje para.
        MovementSound?.Stop();
    }
    public void ActivarSonidoSalto()
    {

        JumpSound.Play();

    }
    public void ActivarSonidoDash()
    {

        DashSound.Play();

    }
    public void ActivarSonidoPickUp()
    {

        PickUpSound.Play();

    }
}
