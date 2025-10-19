using UnityEngine;

public interface IGrabbable
{
    public GameObject gameObject { get; }
    public bool Grab(Transform grabbingTransform, IGrabber grabber);
    public bool Drop();


}
