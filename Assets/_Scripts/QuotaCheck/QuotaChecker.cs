using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using UnityEngine;

public class QuotaChecker
{
    Quota quota;

    Quota quotaPassed = new Quota();

    Dictionary<AnimalType, int> animalValues = new Dictionary<AnimalType, int>()
    {
        {AnimalType.Duck, 50 },
        {AnimalType.Pangolin, 100},
        {AnimalType.Snake, 200},
    };    


    bool isQuotaPass = false;


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
    /// This function updates the state of the quote with the data provided. Is espected to receve a type of animal and the number of that animal obtained.
    /// </summary>
    /// <param name="animal"></param>
    /// <param name="number"></param>
    public void UpdateCuote(AnimalType animal, int number)
    {
        quotaPassed.AddPoints(animalValues[animal]);
        
        switch (animal)
        {
            case AnimalType.Duck:
                //quotaPassed.AddRestictionPassed(Restriction.Duck, number);
                quotaPassed.AddRestictionPassed(Restriction.Herbivore, number);

                break;

            case AnimalType.Pangolin:
                quotaPassed.AddRestictionPassed(Restriction.Herbivore, number);

                break;


            case AnimalType.Snake:
                //quotaPassed.AddRestictionPassed(Restriction.Carnivore, number);
                break;

            default:
                break;
        }


        this.isQuotaPass = this.quota.CheckIfPassed(quotaPassed);        
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
