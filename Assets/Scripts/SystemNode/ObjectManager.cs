/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - This Class keeps track of all Creature instances, which are used for iterative changes and statistical purposes.
 *      - This Class also handles all Object destructions
 *  
 *  References:
 *      Scene:
 *          - Simulation scene(s)
 *          - EditorMapGeneration
 *      Script:
 *          - 
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */



using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private static Dictionary<int,Creature> _s_allCreatures;
    private static Dictionary<int,Corpse> _s_allCorpses;

    private void Awake()
    {
        if (_s_allCreatures != null || _s_allCorpses != null)
        {
            /*this class has already been assigned*/
            return;
        }
        _s_allCreatures = new();
        _s_allCorpses = new();
    }

    #region Creatures
    public static void AddCreature(Creature toAdd)
    {
        _s_allCreatures.Add(toAdd.GetInstanceID(), toAdd);
    }

    public static void AddCreatures(List<Creature> toAddList)
    {
        for (int i = 0; i < toAddList.Count; i++)
        {
            Creature creature = toAddList[i];
            _s_allCreatures.Add(creature.GetInstanceID(), creature);
        }
    }

    public static void DeleteAllCreatures()
    {
        foreach (Creature c in _s_allCreatures.Values)
        {
            Destroy(c.gameObject);
        }
        _s_allCreatures.Clear();
    }

    public static void DeleteCreature(Creature toDelete)
    {
        if (!_s_allCreatures.ContainsKey(toDelete.GetInstanceID()))
        {
            UnregisteredObjectException($"{toDelete}");
        }

        _s_allCreatures.Remove(toDelete.GetInstanceID());
        Destroy(toDelete.gameObject);
    }

    public static void ChangeTrailColor()
    {
        foreach (Creature c in _s_allCreatures.Values)
        {
            c.Trail.SetColor();
        }
    }
    #endregion

    #region Corpses
    public static void AddCorpse(Corpse toAdd)
    {
        _s_allCorpses.Add(toAdd.GetInstanceID(), toAdd);
    }

    public static void DeleteCorpse(Corpse toDelete)
    {
        if (!_s_allCorpses.ContainsKey(toDelete.GetInstanceID()))
        {
            UnregisteredObjectException($"{toDelete}");
        }

        _s_allCorpses.Remove(toDelete.GetInstanceID());
        Destroy(toDelete.gameObject);
    }

    public static void DeleteAllCorpses()
    {
        foreach (Corpse c in _s_allCorpses.Values)
        {
            Destroy(c.gameObject);
        }
        _s_allCorpses.Clear();
    }
    #endregion

    #region Exceptions
    private static void UnregisteredObjectException(string unregisteredObject)
    {
        throw new System.Exception($"UNREGISTERED CREATURE: This should never happen \n {unregisteredObject}");
    }
    #endregion
}
