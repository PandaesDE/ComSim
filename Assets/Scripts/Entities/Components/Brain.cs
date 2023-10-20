using System.Collections.Generic;
using UnityEngine;

public class Brain
{
    private Creature creature;

    public IConsumable activeFood;
    public Creature activeHunt;
    public Creature activeFlee;

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

    public void RemoveSpottedCreature(int ID)
    {
        if (spottedCreature.ContainsKey(ID))
            spottedCreature.Remove(ID);
    }
    #endregion
    #region Hunger
    public void AddFoodSource(GameObject g)
    {
        IConsumable food = g.GetComponent<IConsumable>();
        if (food != null) AddFoodSource(food);
    }

    public void AddFoodSource(IConsumable food)
    {
        int ID = food.gameObject.GetInstanceID();
        if (spottedFood.ContainsKey(ID)) return;
        spottedFood[ID] = food;
    }

    public void setActiveFoodSource()
    {
        activeFood = getNearestFoodSource();
    }

    public IConsumable getNearestFoodSource()
    {
        IConsumable closest = null;
        float minDistance = 100000f;
        if (spottedFood.Count <= 0) return null;
        foreach (KeyValuePair<int, IConsumable> keyValue in spottedFood)
        {
            if (!keyValue.Value.hasFood) continue;

            float distance = Vector3.Distance(keyValue.Value.gameObject.transform.position, creature.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = keyValue.Value;
            }
        }
        return closest;
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

    #region inactiveFood
    public void ActivateAll()
    {
        foreach (KeyValuePair<int, IConsumable> food in inactiveFood)
        {
            int ID = food.Key;
            inactiveFood.Remove(ID);
            AddFoodSource(food.Value);
        }
    }

    public void SetInactiveFoodSource(IConsumable food)
    {
        int ID = food.gameObject.GetInstanceID();

        /*Remove from FoodSourceList*/
        if (!spottedFood.ContainsKey(ID)) return;
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

    public Vector2 getNearestWaterSource()
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
