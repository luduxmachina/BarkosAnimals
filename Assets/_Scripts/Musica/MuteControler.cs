using UnityEngine;
using UnityEngine.Audio;

public class MuteControler : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void ToggleMute(bool isMuted)
    {
        if (isMuted)
        {
            Settings.Instance.ToggleMute(isMuted);
            audioMixer.SetFloat("MasterVolume", -80f); // Mute
        }
        else
        {
            float volume;
            Settings.Instance.ToggleMute(isMuted);
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(Settings.Instance.GetVolume()) * 20);
        }
    }

}
