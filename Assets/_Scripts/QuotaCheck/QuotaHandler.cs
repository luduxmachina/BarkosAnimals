using System;
using UnityEngine;

public class QuotaHandler : MonoBehaviour
{
    private QuotaChecker checker;

    private void Awake()
    {
        checker = GameFlowManager.instance.quotaChecker;  
    }


    public void UpdateCuota(Int32 a, InventoryItemDataObjects itemNuevo)
    {
        checker.UpdateCuote(itemNuevo);
    }
}
