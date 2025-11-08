using UnityEngine;
using UnityEngine.Events;

public interface IGrabbable
{
    public GameObject gameObject { get; }
    public bool Grab(Transform grabbingTransform, IGrabber grabber);
    public bool Drop();
    public UnityEvent OnGrab { get; }
    public UnityEvent OnDrop { get; }
    public IGrabber currentGrabber { get; }


}
