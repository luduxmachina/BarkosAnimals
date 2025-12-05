using UnityEngine;

public class ShowMobileInput : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
#if UNITY_WEBGL
        if(Application.isMobilePlatform == false)
        {
            gameObject.SetActive(false);
        }
        return;
#endif
        gameObject.SetActive(false);
    }
}
