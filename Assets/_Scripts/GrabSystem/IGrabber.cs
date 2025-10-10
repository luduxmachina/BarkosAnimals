using UnityEngine;

public interface IGrabber 
{
    public GameObject gameObject { get; }
    public void StopGrabbing();
}
