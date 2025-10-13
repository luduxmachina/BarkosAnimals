using System.CodeDom.Compiler;
using System.Collections.Generic;
using UnityEngine;


public class Quota
{
    int quotaValue;
    Dictionary<AnimalType, int> animals = new Dictionary<AnimalType, int>();

    public Quota(int level)
    {

    }

}

public class QuotaChecker
{
    public QuotaChecker()
    {

    }

    Dictionary <string, int> animalValues = new Dictionary<string, int>()
    {
        {"Duck", 50},
        {"Snake", 100},
        {"Pangolin", 80},
    };

    public void GenerateCuote(int level)
    {

    }

    public void NewCuote()
    {

    }

    public void CheckCuote()
    {

    }
}
