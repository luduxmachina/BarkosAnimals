using System.Collections.Generic;
using System;
using UnityEngine;

public enum Restriction
{
    Duck,
    Herbivore
}


public class Quota
{
    int quotaValue;
    public int QuotaValue {
        get { return quotaValue; }
        set { quotaValue = value; }
    }



    Dictionary<Restriction, int> restrictions;
    public Dictionary<Restriction, int> Restrictions {
        get { return restrictions; }
    }

    public Quota()
    {
        restrictions = new Dictionary<Restriction, int>();

        foreach (Restriction restriction in Enum.GetValues(typeof(Restriction)))
        {
            restrictions[restriction] = 0;
        }

    }

    public Quota(int level)
    {
        restrictions = new Dictionary<Restriction, int>();

        foreach (Restriction restriction in Enum.GetValues(typeof(Restriction)))
        {
            restrictions[restriction] = 0;
        }

        if (level == 0)
        {
            quotaValue = 100;
            restrictions[Restriction.Herbivore] = 1;
        }
        else if (level == 1)
        {
            quotaValue = 100;
            restrictions[Restriction.Herbivore] = 1;
        }
        else if (level == 2)
        {
            quotaValue = 300;
            restrictions[Restriction.Herbivore] = 1;
        }
        else if (level == 3)
        {
            quotaValue = 500;
        }
    }

    public void AddPoints(int points)
    {
        this.quotaValue += points;
    }

    public void RemovePoints(int points)
    {
        this.quotaValue -= points;
    }

    public void AddRestictionPassed(Restriction restriction, int number)
    {
        this.restrictions[restriction] += number;
    }

    public void RemoveRestrictionPassed(Restriction restriction, int number)
    {
        this.restrictions[restriction] -= number;
    }

    public bool CheckIfPassed(int value, Dictionary<Restriction, int> restrictionsPass)
    {
        bool passed = true;

        if (quotaValue > value)
        {
            passed = false;
            return passed;
        }

        foreach (Restriction restriction in Enum.GetValues(typeof(Restriction)))
        {
            if (this.restrictions[restriction] > restrictionsPass[restriction])
            {
                passed = false;
            }


            if (!passed) break;
        }

        return passed;
    }

    public bool CheckIfPassed(Quota quota)
    {
        bool passed = true;

        if (this.quotaValue > quota.quotaValue)
        {
            passed = false;
        }
        foreach (Restriction restriction in Enum.GetValues(typeof(Restriction)))
        {
            if (this.restrictions[restriction] > quota.restrictions[restriction])
            {
                passed = false;
            }
            if (!passed) break;
        }

        return passed;
    }
}
