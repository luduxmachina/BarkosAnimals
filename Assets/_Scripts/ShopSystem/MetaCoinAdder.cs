using UnityEngine;

public class MetaCoinAdder : MonoBehaviour
{
    [SerializeField]
    private bool addMetaCoinsOnStart = true;

    [SerializeField, HideIf("addMetaCoinsOnStart", false)]
    private int metaCoinsToAddOnStart = 8;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (addMetaCoinsOnStart)
        {
            AddMetaCoin(metaCoinsToAddOnStart);
        }
    }
    public void AddMetaCoin(int amount)
    {
        MetaCoinHandler.AddMetaCoins(amount);
    }
}
