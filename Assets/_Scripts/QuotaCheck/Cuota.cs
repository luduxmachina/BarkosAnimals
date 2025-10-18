using System.Collections.Generic;
using System;
using UnityEngine;

public enum Restriction
{
    Herbivore,
    Carnivore,
    Duck
}


public class Quota
{
    int quotaValue = 0;

    Dictionary<Restriction, int> restrictions;

    public int QuotaValue { get; set; } = 0;
    public Dictionary<Restriction, int> Restrictions { get; set; }

    public Quota()
    {
        restrictions = new Dictionary<Restriction, int>();

        foreach (Restriction restriction in Enum.GetValues(typeof(Restriction)))
        {
            restrictions[restriction] = 0;
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


    public Quota(int level)
    {
        restrictions = new Dictionary<Restriction, int>();

        foreach (Restriction restriction in Enum.GetValues(typeof(Restriction)))
        {
            restrictions[restriction] = 0;
        }

        if (level == 1)
        {
            quotaValue = 100;
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
