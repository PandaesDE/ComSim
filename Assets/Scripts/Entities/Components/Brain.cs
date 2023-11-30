using System.Collections.Generic;
using UnityEngine;

public class Brain
{
    private Creature creature;

    public IConsumable activeFood { get; private set; }
    public GameObject activeWater { get; private set; }
    public Creature activeHunt { get; private set; }
    public Creature activeFlee { get; private set; }
    public Creature activeMate { get; private set; }


    private Dictionary<int, IConsumable> spottedFoods;
    private Dictionary<int, IConsumable> inactiveFoods;
    private Dictionary<int, Creature> spottedCreatures;
    private Dictionary<int, Creature> spottedMates;          /*List of Same Species and opposite Gender*/
    private Dictionary<int, GameObject> spottedWaterSources;


    public Brain(Creature creature)
    {
        spottedFoods = new Dictionary<int, IConsumable>();
        inactiveFoods = new Dictionary<int, IConsumable>();
        spottedCreatures = new Dictionary<int, Creature>();
        spottedMates = new Dictionary<int, Creature>();
        spottedWaterSources = new Dictionary<int, GameObject>();
        this.creature = creature;
    }

    public void onStateChange()
    {
        activeFood = null;
        activeWater = null;
        activeHunt = null;
        activeFlee = null;
        activeMate = null;
    }

    #region Survival
    public void AddSpottedCreature(Creature c)
    {
        int ID = c.gameObject.GetInstanceID();
        if (spottedCreatures.ContainsKey(ID)) return;
        spottedCreatures[ID] = c;
    }

    public bool hasSpottedCreature()
    {
        return spottedCreatures.Count > 0;
    }

    public void setActiveHunt()
    {
        activeHunt = getNearestCreature();
    }

    public Creature getNearestCreature()
    {
        if (!hasSpottedCreature()) return null;

        Creature closest = null;
        float minDistance = Mathf.Infinity;
        List<int> missingIDs = new();

        foreach (KeyValuePair<int, Creature> keyValue in spottedCreatures)
        {
            //Food resource missing (consumed)
            if (keyValue.Value == null)
            {
                missingIDs.Add(keyValue.Key);
                continue;
            }

            float distance = Vector3.Distance(keyValue.Value.gameObject.transform.position, creature.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = keyValue.Value;
            }
        }
        RemoveSpottedCreatures(missingIDs);
        return closest;
    }

    public void RemoveSpottedCreatures(List<int> IDs)
    {
        for (int i = 0; i < IDs.Count; i++)
        {
            RemoveSpottedCreature(IDs[i]);
        }
    }

    public void RemoveSpottedCreature(int ID)
    {
        if (spottedCreatures.ContainsKey(ID))
            spottedCreatures.Remove(ID);
    }
    #endregion
    #region Hunger
    public bool HasFoodSource()
    {
        if (spottedFoods.Count <= 0)
        {
            if (inactiveFoods.Count <= 0)
            {
                return false;
            }
            ActivateAllInactiveFoodSources();
            return true;
        }
        return true;
    }
    public void AddFoodSource(GameObject g)
    {
        IConsumable food = g.GetComponent<IConsumable>();
        if (food != null) AddFoodSource(food);
    }

    public void AddFoodSource(IConsumable food)
    {
        int ID = food.gameObject.GetInstanceID();
        if (spottedFoods.ContainsKey(ID)) return;
        if (inactiveFoods.ContainsKey(ID)) return;
        spottedFoods[ID] = food;
    }

    public void setActiveFoodSource()
    {
        activeFood = getNearestFoodSource();
    }

    public void RemoveFoodSources(List<int> IDs)
    {
        for (int i = 0; i < IDs.Count; i++)
        {
            RemoveFoodSource(IDs[i]);
        }
    }

    public void RemoveFoodSource(IConsumable food)
    {
        RemoveFoodSource(food.gameObject.GetInstanceID());
    }

    public void RemoveFoodSource(int ID)
    {
        if (spottedFoods.ContainsKey(ID))
            spottedFoods.Remove(ID);
    }

    private IConsumable getNearestFoodSource()
    {
        if (spottedFoods.Count <= 0) return null;

        IConsumable closest = null;
        float minDistance = Mathf.Infinity;
        List<int> missingIDs = new();
        List<int> inactiveIDs = new();

        foreach (KeyValuePair<int, IConsumable> keyValue in spottedFoods)
        {

            //Food resource missing (consumed)
            if (keyValue.Value == null || keyValue.Value.isConsumed)
            {
                missingIDs.Add(keyValue.Key);
                continue;
            }

            //no Food Resource left for now
            if (!keyValue.Value.hasFood)
            {
                inactiveIDs.Add(keyValue.Key);
                continue;
            }


            float distance = Vector3.Distance(keyValue.Value.gameObject.transform.position, creature.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = keyValue.Value;
            }
        }
        RemoveFoodSources(missingIDs);
        SetInactiveFoodSources(inactiveIDs);
        return closest;
    }

    #region inactiveFood
    public void ActivateAllInactiveFoodSources()
    {
        List<int> nowActiveIDs = new();
        foreach (KeyValuePair<int, IConsumable> food in inactiveFoods)
        {
            int ID = food.Key;
            nowActiveIDs.Add(ID);
            AddFoodSource(food.Value);
        }

        for (int i = 0; i < nowActiveIDs.Count; i++)
        {
            ReactivateFoodSource(nowActiveIDs[i]);
        }
    }

    public void ReactivateFoodSource(int ID)
    {
        if (!inactiveFoods.ContainsKey(ID)) return;
        IConsumable food = inactiveFoods[ID];
        inactiveFoods.Remove(ID);
        AddFoodSource(food);
    }

    public void SetInactiveFoodSources(List<int> IDs)
    {
        for (int i = 0; i < IDs.Count; i++)
        {
            SetInactiveFoodSource(IDs[i]);
        }
    }

    public void SetInactiveFoodSource(IConsumable food)
    {
        int ID = food.gameObject.GetInstanceID();
        activeFood = null;
        SetInactiveFoodSource(ID);
    }

    public void SetInactiveFoodSource(int ID)
    {
        /*Remove from FoodSourceList*/
        if (!spottedFoods.ContainsKey(ID)) return;
        IConsumable food = spottedFoods[ID];
        RemoveFoodSource(ID);

        /*Add to inactiveFood*/
        if (inactiveFoods.ContainsKey(ID)) return;
        inactiveFoods[ID] = food;
    }
    #endregion

    #endregion
    #region Thirst
    public void addWaterSource(GameObject water)
    {
        if (spottedWaterSources.ContainsKey(water.GetInstanceID())) return;
        spottedWaterSources.Add(water.GetInstanceID(), water);
    }

    public bool hasWaterSource()
    {
        return spottedWaterSources.Count > 0;
    }

    public void setActiveWaterSource()
    {
        activeWater = getNearestWaterSource();
    }

    private GameObject getNearestWaterSource()
    {
        GameObject closest = null;
        float minDistance = Mathf.Infinity;
        foreach (KeyValuePair<int, GameObject> keyValue in spottedWaterSources)
        {
            float distance = Vector3.Distance(keyValue.Value.transform.position, creature.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = keyValue.Value;
            }
        }
        return closest;
    }

    public void RemoveWaterSource(int ID)
    {
        if (spottedWaterSources.ContainsKey(ID))
            spottedWaterSources.Remove(ID);
    }
    #endregion
    #region Social
    public void AddPotentialMate(Creature mate)
    {
        if (spottedMates.ContainsKey(mate.gameObject.GetInstanceID())) return;
        if (mate == null) return;

        spottedMates[mate.gameObject.GetInstanceID()] = mate;
    }

    public void setActiveMate()
    {
        activeMate = getNearestMate();
    }

    public void RemovePotentialMate(int ID)
    {
        if (spottedMates.ContainsKey(ID))
            spottedMates.Remove(ID);
    }

    public bool hasPotentialMates()
    {
        return spottedMates.Count > 0;
    }
    private Creature getNearestMate()
    {
        
        if (!hasPotentialMates()) return null;
        
        Creature closest = null;
        float minDistance = Mathf.Infinity;

        foreach (KeyValuePair<int, Creature> keyValue in spottedMates)
        {
            //Mate missing (Died)
            if (keyValue.Value == null)
            {
                RemovePotentialMate(keyValue.Key);
                continue;
            }

            float distance = Vector3.Distance(keyValue.Value.gameObject.transform.position, creature.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = keyValue.Value;
            }
        }
        return closest;
    }
    #endregion
}
