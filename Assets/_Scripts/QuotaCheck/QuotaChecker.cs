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

    public Quota GenerateCuote(int level)
    {
        quota = new Quota(level);

        return quota;
    }

    public void CheckCuote(AnimalType animal, int number)
    {
        quotaPassed.AddPoints(animalValues[animal]);
        
        switch (animal)
        {
            case AnimalType.Duck:
                quotaPassed.AddRestictionPassed(Restriction.Duck, number);
                quotaPassed.AddRestictionPassed(Restriction.Herbivore, number);

                break;

            case AnimalType.Pangolin:
                quotaPassed.AddRestictionPassed(Restriction.Herbivore, number);

                break;


            case AnimalType.Snake:
                quotaPassed.AddRestictionPassed(Restriction.Carnivore, number);
                break;

            default:
                break;
        }


        this.isQuotaPass = this.quota.CheckIfPassed(quotaPassed);        
    }

    public bool IsQuotaPass()
    {
        return isQuotaPass;
    }
}
