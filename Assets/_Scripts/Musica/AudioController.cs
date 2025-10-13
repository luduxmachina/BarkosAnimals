using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void ChangeVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }
}
