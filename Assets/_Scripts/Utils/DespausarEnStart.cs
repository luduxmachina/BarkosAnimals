using UnityEngine;

public class DespausarEnStart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MenuPause.ResumeStatic();
    }
}
