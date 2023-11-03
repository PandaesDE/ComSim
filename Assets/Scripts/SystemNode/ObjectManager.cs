/*This Class keeps track of all Creature instances, which are used for iterative changes and statistical purposes. This Class also handles all Object destructions*/


using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private static List<Creature> allCreatures;
    private static List<Corpse> allCorpses;

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
        allCreatures.Add(toAdd);
    }

    public static void addCreatures(List<Creature> toAdd)
    {
        allCreatures.AddRange(toAdd);
    }

    public static void deleteAllCreatures()
    {
        foreach (Creature c in allCreatures)
        {
            Destroy(c.gameObject);
        }
        allCreatures.Clear();
    }

    public static void deleteCreature(Creature toDelete)
    {
        if (!allCreatures.Contains(toDelete))
        {
            UnregisteredObjectException($"{toDelete}");
        }

        allCreatures.Remove(toDelete);
        Destroy(toDelete.gameObject);
    }

    public static void changeTrailColor()
    {
        foreach (Creature c in allCreatures)
        {
            c.trail.setColor();
        }
    }
    #endregion

    #region Corpses
    public static void addCorpse(Corpse toAdd)
    {
        allCorpses.Add(toAdd);
    }

    public static void deleteCorpse(Corpse toDelete)
    {
        if (!allCorpses.Contains(toDelete))
        {
            UnregisteredObjectException($"{toDelete}");
        }

        allCorpses.Remove(toDelete);
        Destroy(toDelete.gameObject);
    }
    #endregion

    #region Exceptions
    private static void UnregisteredObjectException(string unregisteredObject)
    {
        throw new System.Exception($"UNREGISTERED CREATURE: This should never happen \n {unregisteredObject}");
    }
    #endregion
}
