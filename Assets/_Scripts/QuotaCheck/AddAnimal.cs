using System;
using UnityEngine;

public class AddAnimal : MonoBehaviour
{
    public void AddItemToQuota(Int32 num, InventoryItemDataObjects animal)
    {      

        GameFlowManager.instance.quotaChecker.UpdateCuote(animal);
    }
}
