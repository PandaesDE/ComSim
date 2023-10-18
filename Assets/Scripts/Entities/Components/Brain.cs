using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain
{
    private Creature creature;

    public Dictionary<int, IConsumable> spottedFood;
    public IConsumable activeFood;
    public Dictionary<int, Creature> spottedCreature;
    public Creature activeHunt;
    public Creature activeFlee;
    public Dictionary<int, Vector2> spottedWater;
    public Dictionary<int, Vector2> spottedMate;


    public Brain(Creature creature)
    {
        spottedFood = new Dictionary<int, IConsumable>();
        spottedWater = new Dictionary<int, Vector2>();
        spottedMate = new Dictionary<int, Vector2>();
        this.creature = creature;
    }

    #region Hunger
    public void AddFoodSource(GameObject g)
    {
        if (spottedFood.ContainsKey(g.GetInstanceID())) return;
        IConsumable food = g.GetComponent<IConsumable>();
        spottedFood[g.GetInstanceID()] = food;
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
    public void AddPotentialMate(GameObject mate)
    {
        if (spottedMate.ContainsKey(mate.GetInstanceID())) return;
        Vector2Int mateCoords = Util.Conversion.Vector3ToVector2Int(mate.transform.position);
        spottedMate[mate.GetInstanceID()] = mateCoords;
    }

    public void RemovePotentialMate(int ID)
    {
        if (spottedMate.ContainsKey(ID))
            spottedMate.Remove(ID);
    }
    #endregion
}
