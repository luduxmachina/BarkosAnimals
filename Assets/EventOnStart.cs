using UnityEngine;
using UnityEngine.Events;

public class EventOnStart : MonoBehaviour
{
    public UnityEvent OnStart = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnStart?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
