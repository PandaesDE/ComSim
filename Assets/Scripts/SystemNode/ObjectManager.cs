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



using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static Dictionary<int,Human> AllHumans { get; private set; }
    public static Dictionary<int,Lion> AllLions {get; private set; }
    public static Dictionary<int,Boar> AllBoars {get; private set; }
    public static Dictionary<int,Rabbit> AllRabbits {get; private set; }
    public static Dictionary<int, Corpse> AllCorpses { get; private set; }
    public static int AllCreatureCount
    {
        get
        {
            return AllHumans.Count + AllLions.Count + AllBoars.Count + AllRabbits.Count;
        }
    }

    private void Awake()
    {
        if (AllCorpses != null)
        {
            /*this class has already been assigned*/
            return;
        }
        AllHumans = new();
        AllLions = new();
        AllBoars = new();
        AllRabbits = new();
        AllCorpses = new();
    }

    #region Creatures
    public static void AddCreature(Creature toAdd)
    {
        if (toAdd.TryGetComponent<Human>(out Human h))
            AllHumans.Add(h.GetInstanceID(), h);
        else if (toAdd.TryGetComponent<Lion>(out Lion l))
            AllLions.Add(l.GetInstanceID(), l);
        else if (toAdd.TryGetComponent<Boar>(out Boar b))
            AllBoars.Add(b.GetInstanceID(), b);
        else if (toAdd.TryGetComponent<Rabbit>(out Rabbit r))
            AllRabbits.Add(r.GetInstanceID(), r);
        else
            throwUnknownCreatureException(toAdd);
    }

    public static void AddCreatures(List<Creature> toAddList)
    {
        for (int i = 0; i < toAddList.Count; i++)
        {
            AddCreature(toAddList[i]);
        }
    }

    public static void DeleteAllCreatures()
    {
        DeleteCreatureList(AllHumans);
        DeleteCreatureList(AllLions);
        DeleteCreatureList(AllBoars);
        DeleteCreatureList(AllRabbits);

        static void DeleteCreatureList<T>(Dictionary<int, T> creatureDict) where T : Creature
        {
            foreach (Creature c in creatureDict.Values)
            {
                Destroy(c.gameObject);
            }
            creatureDict.Clear();
        }
    }



    public static void DeleteCreature(Creature toDelete)
    {
        if (toDelete.TryGetComponent<Human>(out Human h))
            DeleteCreature(AllHumans, h);
        else if (toDelete.TryGetComponent<Lion>(out Lion l))
            DeleteCreature(AllLions, l);
        else if (toDelete.TryGetComponent<Boar>(out Boar b))
            DeleteCreature(AllBoars, b);
        else if (toDelete.TryGetComponent<Rabbit>(out Rabbit r))
            DeleteCreature(AllRabbits, r);
        else
            throwUnknownCreatureException(toDelete);

        static void DeleteCreature<T>(Dictionary<int,T> creatureDict, T toDelete) where T : Creature
        {
            if (!creatureDict.ContainsKey(toDelete.GetInstanceID()))
            {
                return; //already deleted
            }

            creatureDict.Remove(toDelete.GetInstanceID());
            Destroy(toDelete.gameObject);
        }
    }


    public static void ChangeTrailColor()
    {

        foreach (Creature c in GetAllCreatures().Values)
        {
            c.Trail.SetColor();
        }
    }

    private static Dictionary<int, Creature> GetAllCreatures()
    {
        Dictionary<int, Creature> creatures = new();
        MergeDictionaries(AllHumans, creatures);
        MergeDictionaries(AllLions, creatures);
        MergeDictionaries(AllBoars, creatures);
        MergeDictionaries(AllRabbits, creatures);
        return creatures;

        static Dictionary<int, Creature> MergeDictionaries<T>(Dictionary<int, T> source, Dictionary<int, Creature> destination) where T : Creature
        {
            foreach (var kvp in source)
            {
                destination[kvp.Key] = kvp.Value;
            }
            return destination;
        }
    }
    #endregion

    #region Corpses
    public static void AddCorpse(Corpse toAdd)
    {
        AllCorpses.Add(toAdd.GetInstanceID(), toAdd);
    }

    public static void DeleteCorpse(Corpse toDelete)
    {
        if (!AllCorpses.ContainsKey(toDelete.GetInstanceID()))
        {
            return; //already deleted
        }

        AllCorpses.Remove(toDelete.GetInstanceID());
        Destroy(toDelete.gameObject);
    }

    public static void DeleteAllCorpses()
    {
        foreach (Corpse c in AllCorpses.Values)
        {
            Destroy(c.gameObject);
        }
        AllCorpses.Clear();
    }
    #endregion

    #region Exceptions
    private static void throwUnknownCreatureException(Creature c)
    {
        throw new Exception($"No valid Creature Type: {c}");
    }
    #endregion
}
