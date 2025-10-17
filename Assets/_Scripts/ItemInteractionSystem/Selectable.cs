using UnityEngine;
using UnityEngine.Events;

public class Selectable : MonoBehaviour
{
    public UnityEvent OnSelected;
    public UnityEvent OnDeselected;

    public virtual void Selected()
    {
        OnSelected?.Invoke();
    }
    public virtual void Deselected()
    {
        OnDeselected?.Invoke();
    }
}
