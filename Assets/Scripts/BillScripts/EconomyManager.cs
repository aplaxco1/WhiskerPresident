using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    // singleton instance
    public static EconomyManager Instance;
    // resource variables
    int food;
    int industry;
    int technology;
    // resource modifiers
    int food_rate;
    int industry_rate;
    int technology_rate;

    private void Awake()
    {
        // Double check that I'm not keeping two singleton instances
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // these functions are for modifying economy
    void ModifyFood(int amount)
    {
        food += amount;
    }

    void ModifyIndustry(int amount)
    {
        industry += amount;
    }

    void ModifyTechnology(int amount)
    {
        technology += amount;
    }

    void ModifyFoodRate(int amount)
    {
        food_rate += amount;
    }

    void ModifyIndustryRate(int amount)
    {
        industry_rate += amount;
    }

    void ModifyTechnologyRate(int amount)
    {
        technology_rate += amount;
    }

    // accessor functions
    int GetFood()
    {
        return food;
    }

    int GetIndustry()
    {
        return industry;
    }

    int GetTechnology()
    {
        return technology;
    }

    int GetFoodRate()
    {
        return food_rate;
    }

    int GetIndustryRate()
    {
        return industry_rate;
    }

    int GetTechnologyRate()
    {
        return technology_rate;
    }
}
