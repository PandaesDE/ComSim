/*This Class keeps track of all Creature instances, which are used for iterative changes and statistical purposes. This Class also handles all Object destructions*/


using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private static Dictionary<int,Creature> allCreatures;
    private static Dictionary<int,Corpse> allCorpses;

    private void Awake()
    {
        if (allCreatures != null || allCorpses != null)
        {
            /*this class has already been assigned*/
            return;
        }
        allCreatures = new();
        allCorpses = new();
    }

    #region Creatures
    public static void addCreature(Creature toAdd)
    {
        allCreatures.Add(toAdd.GetInstanceID(), toAdd);
    }

    public static void addCreatures(List<Creature> toAddList)
    {
        for (int i = 0; i < toAddList.Count; i++)
        {
            Creature creature = toAddList[i];
            allCreatures.Add(creature.GetInstanceID(), creature);
        }
    }

    public static void deleteAllCreatures()
    {
        foreach (Creature c in allCreatures.Values)
        {
            Destroy(c.gameObject);
        }
        allCreatures.Clear();
    }

    public static void deleteCreature(Creature toDelete)
    {
        if (!allCreatures.ContainsKey(toDelete.GetInstanceID()))
        {
            UnregisteredObjectException($"{toDelete}");
        }

        allCreatures.Remove(toDelete.GetInstanceID());
        Destroy(toDelete.gameObject);
    }

    public static void changeTrailColor()
    {
        foreach (Creature c in allCreatures.Values)
        {
            c.trail.setColor();
        }
    }
    #endregion

    #region Corpses
    public static void addCorpse(Corpse toAdd)
    {
        allCorpses.Add(toAdd.GetInstanceID(), toAdd);
    }

    public static void deleteCorpse(Corpse toDelete)
    {
        if (!allCorpses.ContainsKey(toDelete.GetInstanceID()))
        {
            UnregisteredObjectException($"{toDelete}");
        }

        allCorpses.Remove(toDelete.GetInstanceID());
        Destroy(toDelete.gameObject);
    }

    public static void deleteAllCorpses()
    {
        foreach (Corpse c in allCorpses.Values)
        {
            Destroy(c.gameObject);
        }
        allCorpses.Clear();
    }
    #endregion

    #region Exceptions
    private static void UnregisteredObjectException(string unregisteredObject)
    {
        throw new System.Exception($"UNREGISTERED CREATURE: This should never happen \n {unregisteredObject}");
    }
    #endregion
}
