using System.Collections.Generic;
using UnityEngine;

public class Brain : Component
{
    private Creature creature;

    public IConsumable activeFood { get; private set; }
    public Vector2 activeWater { get; private set; }
    public Creature activeHunt { get; private set; }
    public Creature activeFlee { get; private set; }
    public Creature activeMate { get; private set; }


    private Dictionary<int, IConsumable> spottedFood;
    private Dictionary<int, IConsumable> inactiveFood;
    private Dictionary<int, Creature> spottedCreature;
    private Dictionary<int, Creature> spottedMate;
    private Dictionary<int, Vector2> spottedWater;


    public Brain(Creature creature)
    {
        spottedFood = new Dictionary<int, IConsumable>();
        inactiveFood = new Dictionary<int, IConsumable>();
        spottedCreature = new Dictionary<int, Creature>();
        spottedMate = new Dictionary<int, Creature>();
        spottedWater = new Dictionary<int, Vector2>();
        this.creature = creature;
    }

    #region Survival
    public void AddSpottedCreature(Creature c)
    {
        int ID = c.gameObject.GetInstanceID();
        if (spottedCreature.ContainsKey(ID)) return;
        spottedCreature[ID] = c;
    }

    public bool hasSpottedCreature()
    {
        return spottedCreature.Count > 0;
    }

    public void setActiveHunt()
    {
        activeHunt = getNearestCreature();
    }

    public Creature getNearestCreature()
    {
        if (!hasSpottedCreature()) return null;

        Creature closest = null;
        float minDistance = 100000f;
        List<int> missingIDs = new();

        foreach (KeyValuePair<int, Creature> keyValue in spottedCreature)
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
        if (spottedCreature.ContainsKey(ID))
            spottedCreature.Remove(ID);
    }
    #endregion
    #region Hunger
    public bool HasFoodSource()
    {
        if (spottedFood.Count <= 0)
        {
            if (inactiveFood.Count <= 0)
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
        if (spottedFood.ContainsKey(ID)) return;
        if (inactiveFood.ContainsKey(ID)) return;
        spottedFood[ID] = food;
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
        if (spottedFood.ContainsKey(ID))
            spottedFood.Remove(ID);
    }

    private IConsumable getNearestFoodSource()
    {
        if (spottedFood.Count <= 0) return null;

        IConsumable closest = null;
        float minDistance = Mathf.Infinity;
        List<int> missingIDs = new();
        List<int> inactiveIDs = new();

        foreach (KeyValuePair<int, IConsumable> keyValue in spottedFood)
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
        foreach (KeyValuePair<int, IConsumable> food in inactiveFood)
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
        if (!inactiveFood.ContainsKey(ID)) return;
        IConsumable food = inactiveFood[ID];
        inactiveFood.Remove(ID);
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

        SetInactiveFoodSource(ID);
    }

    public void SetInactiveFoodSource(int ID)
    {
        /*Remove from FoodSourceList*/
        if (!spottedFood.ContainsKey(ID)) return;
        IConsumable food = spottedFood[ID];
        RemoveFoodSource(ID);

        /*Add to inactiveFood*/
        if (inactiveFood.ContainsKey(ID)) return;
        inactiveFood[ID] = food;
    }
    #endregion

    #endregion
    #region Thirst
    public void addWaterSource(GameObject water)
    {
        if (spottedWater.ContainsKey(water.GetInstanceID())) return;
        Vector2Int waterCoords = Util.Conversion.Vector3ToVector2Int(water.transform.position);
        spottedWater.Add(water.GetInstanceID(), waterCoords);
    }

    public bool hasWaterSource()
    {
        return spottedWater.Count > 0;
    }

    public void setActiveWaterSource()
    {
        activeWater = getNearestWaterSource();
    }

    private Vector2 getNearestWaterSource()
    {
        Vector2 closest = Gamevariables.ERROR_VECTOR2;
        float minDistance = Mathf.Infinity;
        foreach (KeyValuePair<int, Vector2> keyValue in spottedWater)
        {
            float distance = Vector3.Distance(keyValue.Value, creature.transform.position);
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
        if (spottedWater.ContainsKey(ID))
            spottedWater.Remove(ID);
    }
    #endregion
    #region Social
    public void AddPotentialMate(Creature mate)
    {
        if (spottedMate.ContainsKey(mate.gameObject.GetInstanceID())) return;
        if (mate == null) return;

        spottedMate[mate.gameObject.GetInstanceID()] = mate;
    }



    public void RemovePotentialMate(int ID)
    {
        if (spottedMate.ContainsKey(ID))
            spottedMate.Remove(ID);
    }
    #endregion
}
