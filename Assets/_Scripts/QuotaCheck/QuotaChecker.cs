using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using UnityEngine;

public class QuotaChecker
{
    Quota quota;

    Quota quotaPassed = new Quota();

    Dictionary<ItemNames, int> animalValues = new Dictionary<ItemNames, int>()
    {
        {ItemNames.Duck, 50 }
    };    


    bool isQuotaPass = false;

    private List<QuotaUiInterface> quotaUIs = new List<QuotaUiInterface>();
    public void AddNewUI (QuotaUiInterface quotaUi)
    {
        quotaUIs.Add(quotaUi);
    }


    /// <summary>
    /// This function set a new value for Quota
    /// </summary>
    /// <param name="quota"></param>
    /// <returns></returns>
    public Quota GenerateCuote(Quota quota)
    {
        this.quota = quota;
        return quota;
    }

    /// <summary>
    /// Create a new value for quota.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    /// 
    public Quota GenerateCuote(int level)
    {
        quota = new Quota(level);
        return quota;
    }

    /// <summary>
    /// Returns the value of the actual quota.
    /// </summary>
    /// <returns></returns>
    public Quota GetQuota() { return quota; }

    /// <summary>
    /// This function updates the state of the quote with the data provided. Is espected to receve a type of animal and the number of that animal obtained.
    /// </summary>
    /// <param name="animal"></param>
    /// <param name="number"></param>
    public void UpdateCuote(InventoryItemDataObjects animal)
    {

        switch (animal.Name)
        {
            case ItemNames.Duck:
                quotaPassed.AddRestictionPassed(Restriction.Duck, animal.Count);
                quotaPassed.AddRestictionPassed(Restriction.Herbivore, animal.Count);

                break;

            //case AnimalType.Pangolin:
            //    quotaPassed.AddRestictionPassed(Restriction.Herbivore, number);
            //
            //    break;
            //
            //
            //case AnimalType.Snake:
            //    //quotaPassed.AddRestictionPassed(Restriction.Carnivore, number);
            //    break;

            default:
                return;
        }


        quotaPassed.AddPoints(animalValues[animal.Name]);

        this.isQuotaPass = this.quota.CheckIfPassed(quotaPassed);

        foreach (var item in quotaUIs)
        {
            item.UpdateQuotaPassed(quotaPassed, isQuotaPass);
        }
    }

    /// <summary>
    /// This function returns if the quota is completed.
    /// </summary>
    /// <returns> 
    /// It returns a boolean that indicate if the quota is completed
    /// </returns>
    public bool IsQuotaPass()
    {
        return isQuotaPass;
    }
}
