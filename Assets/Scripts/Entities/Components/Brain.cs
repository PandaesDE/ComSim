/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - stores values of every interactalbe Object
 *      - provides Add and Remove method for every dictionary
 *      - finds the closest object of every dictionary
 *      - Brain component of a creature
 *  
 *  References:
 *      Scene:
 *          - Indirectly (Component of Creature.cs) for simulation scene(s)
 *      Script:
 *          - One instance per creature
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */

using System.Collections.Generic;
using UnityEngine;

public class Brain
{
    private Creature _creature;

    public IConsumable ActiveFood { get; private set; }
    public GameObject ActiveWater { get; private set; }
    public Creature ActiveHunt { get; private set; }
    public Creature ActiveFlee { get; private set; }
    public Creature ActiveMate { get; private set; }


    private Dictionary<int, IConsumable> _spottedFoods;
    private Dictionary<int, IConsumable> _inactiveFoods;
    private Dictionary<int, Creature> _spottedCreatures;
    private Dictionary<int, Creature> _spottedMates;          /*List of Same Species and opposite Gender*/
    private Dictionary<int, GameObject> _spottedWaterSources;


    public Brain(Creature creature)
    {
        _spottedFoods = new Dictionary<int, IConsumable>();
        _inactiveFoods = new Dictionary<int, IConsumable>();
        _spottedCreatures = new Dictionary<int, Creature>();
        _spottedMates = new Dictionary<int, Creature>();
        _spottedWaterSources = new Dictionary<int, GameObject>();
        this._creature = creature;
    }

    public void OnStateChange()
    {
        ActiveFood = null;
        ActiveWater = null;
        ActiveHunt = null;
        ActiveFlee = null;
        ActiveMate = null;
    }

    #region SetActives
    public void SetActiveFoodSource()
    {
        ActiveFood = GetNearestFoodSource();
    }

    public void setActiveWaterSource()
    {
        ActiveWater = getNearestWaterSource();
    }

    public void SetActiveHunt()
    {
        SetActiveHunt(GetNearestCreature());
    }

    public void SetActiveHunt(Creature toHunt)
    {
        ActiveHunt = toHunt;
    }

    public void SetActiveFlee()
    {
        ActiveFlee = GetNearestCreature();
    }

    public void SetActiveMate()
    {
        ActiveMate = GetNearestMate();
    }
    #endregion

    #region Survival
    public void AddSpottedCreature(Creature c)
    {
        int ID = c.gameObject.GetInstanceID();
        if (_spottedCreatures.ContainsKey(ID)) return;
        _spottedCreatures[ID] = c;
    }

    public bool HasSpottedCreature()
    {
        return _spottedCreatures.Count > 0;
    }



    public Creature GetNearestCreature()
    {
        if (!HasSpottedCreature()) return null;

        Creature closest = null;
        float minDistance = Mathf.Infinity;
        List<int> missingIDs = new();

        foreach (KeyValuePair<int, Creature> keyValue in _spottedCreatures)
        {
            //Food resource missing (consumed)
            if (keyValue.Value == null)
            {
                missingIDs.Add(keyValue.Key);
                continue;
            }

            float distance = Vector3.Distance(keyValue.Value.gameObject.transform.position, _creature.transform.position);
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
        if (_spottedCreatures.ContainsKey(ID))
            _spottedCreatures.Remove(ID);
    }
    #endregion
    #region Hunger
    public bool HasFoodSource()
    {
        if (_spottedFoods.Count <= 0)
        {
            if (_inactiveFoods.Count <= 0)
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
        if (_spottedFoods.ContainsKey(ID)) return;
        if (_inactiveFoods.ContainsKey(ID)) return;
        _spottedFoods[ID] = food;
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
        if (_spottedFoods.ContainsKey(ID))
            _spottedFoods.Remove(ID);
    }

    private IConsumable GetNearestFoodSource()
    {
        if (_spottedFoods.Count <= 0) return null;

        IConsumable closest = null;
        float minDistance = Mathf.Infinity;
        List<int> missingIDs = new();
        List<int> inactiveIDs = new();

        foreach (KeyValuePair<int, IConsumable> keyValue in _spottedFoods)
        {

            //Food resource missing (consumed)
            if (keyValue.Value == null || keyValue.Value.IsConsumed)
            {
                missingIDs.Add(keyValue.Key);
                continue;
            }

            //no Food Resource left for now
            if (!keyValue.Value.HasFood)
            {
                inactiveIDs.Add(keyValue.Key);
                continue;
            }


            float distance = Vector3.Distance(keyValue.Value.gameObject.transform.position, _creature.transform.position);
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
        foreach (KeyValuePair<int, IConsumable> food in _inactiveFoods)
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
        if (!_inactiveFoods.ContainsKey(ID)) return;
        IConsumable food = _inactiveFoods[ID];
        _inactiveFoods.Remove(ID);
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
        ActiveFood = null;
        SetInactiveFoodSource(ID);
    }

    public void SetInactiveFoodSource(int ID)
    {
        /*Remove from FoodSourceList*/
        if (!_spottedFoods.ContainsKey(ID)) return;
        IConsumable food = _spottedFoods[ID];
        RemoveFoodSource(ID);

        /*Add to inactiveFood*/
        if (_inactiveFoods.ContainsKey(ID)) return;
        _inactiveFoods[ID] = food;
    }
    #endregion

    #endregion
    #region Thirst
    public void addWaterSource(GameObject water)
    {
        if (_spottedWaterSources.ContainsKey(water.GetInstanceID())) return;
        _spottedWaterSources.Add(water.GetInstanceID(), water);
    }

    public bool hasWaterSource()
    {
        return _spottedWaterSources.Count > 0;
    }

    private GameObject getNearestWaterSource()
    {
        GameObject closest = null;
        float minDistance = Mathf.Infinity;
        foreach (KeyValuePair<int, GameObject> keyValue in _spottedWaterSources)
        {
            float distance = Vector3.Distance(keyValue.Value.transform.position, _creature.transform.position);
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
        if (_spottedWaterSources.ContainsKey(ID))
            _spottedWaterSources.Remove(ID);
    }
    #endregion
    #region Social
    public void AddPotentialMate(Creature mate)
    {
        if (_spottedMates.ContainsKey(mate.gameObject.GetInstanceID())) return;
        if (mate == null) return;

        _spottedMates[mate.gameObject.GetInstanceID()] = mate;
    }

    public void RemovePotentialMate(int ID)
    {
        if (_spottedMates.ContainsKey(ID))
            _spottedMates.Remove(ID);
    }

    public bool HasPotentialMates()
    {
        return _spottedMates.Count > 0;
    }
    private Creature GetNearestMate()
    {
        
        if (!HasPotentialMates()) return null;
        
        Creature closest = null;
        float minDistance = Mathf.Infinity;

        List<int> missingMates = new();

        foreach (KeyValuePair<int, Creature> keyValue in _spottedMates)
        {
            //Mate missing (Died)
            if (keyValue.Value == null)
            {
                missingMates.Add(keyValue.Key);
                continue;
            }

            float distance = Vector3.Distance(keyValue.Value.gameObject.transform.position, _creature.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = keyValue.Value;
            }
        }

        RemovePotentialMates(missingMates);
        return closest;
    }

    private void RemovePotentialMates(List<int> IDs)
    {
        for (int i = 0; i < IDs.Count; i++)
        {
            RemovePotentialMate(IDs[i]);
        }
    }
    #endregion
}
