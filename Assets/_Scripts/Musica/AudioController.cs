using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    private void Start()
    {
        float volume = Settings.Instance.GetVolume();
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    public void ChangeVolume(float volume)
    {
        if(Settings.Instance.IsMuted()) return;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        Settings.Instance.NewVolume(volume);
    }
 
}
