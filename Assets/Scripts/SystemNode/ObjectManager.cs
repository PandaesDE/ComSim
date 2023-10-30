/*This Class keeps track of all Creature instances, which are used for iterative changes and statistical purposes*/


using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private static List<Creature> allCreatures;

    private void Awake()
    {
        allCreatures = new();
    }

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
        foreach(Creature c in allCreatures)
        {
            Destroy(c);
        }
        allCreatures.Clear();
    }

    public static void changeTrailColor()
    {
        /*What if creature died?*/
        foreach(Creature c in allCreatures)
        {
            c.trail.setColor();
        }
    }

    public static void deleteCreature(Creature c)
    {
        if (!allCreatures.Contains(c))
        {
            Debug.LogError($"UNREGISTERED CREATURE: This should never happen \n {c}");
        }

        allCreatures.Remove(c);
        Destroy(c.gameObject);
    }
}
