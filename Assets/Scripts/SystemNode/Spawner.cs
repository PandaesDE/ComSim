/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Class Purposes:
 *  
 *  Class Infos:
 *      
 *  Class References:
 *      
 */

using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    public GameObject PREFAB_Human;
    public GameObject PREFAB_Boar;
    public GameObject PREFAB_Rabbit;
    public GameObject PREFAB_Lion;

    private static Spawner _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<Spawner>();
        }
    }

    private void Start()
    {
        StartCoroutine(spawnEntitiesAfterTime(.1f));
    }

    /*  There is no particular reason to spawn all Entities after a given Time, the only reason why it's implemented that way,
     *  is the fact that without it the "evaluateVision" function within Creature.cs captures all Tilemap gameobjects with a 2DCollider.
     *  The reason for that is probably that all colliders are at position 0,0 from the start before the map got initialized.
     *  A wait for map initialized method did not work out as planed and so waiting to spawn all Entities seemed to be the most reliable and realistic way to go around this issue.
     */
    IEnumerator spawnEntitiesAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        spawnHumans(Gamevariables.HUMAN_AMOUNT_START);
        spawnLions(Gamevariables.LION_AMOUNT_START);
        spawnBoars(Gamevariables.BOAR_AMOUNT_START);
        spawnRabbits(Gamevariables.RABBIT_AMOUNT_START);
    }

    public static void spawnHumans(int amount)
    {
        spawnCreatures(_instance.PREFAB_Human, amount);
    }

    public static void spawnHumans(int amount, Vector2 position)
    {
        spawnCreatures(_instance.PREFAB_Human, amount, position);
    }

    public static Human spawnHuman()
    {
        return spawnHuman(Util.Random.CoordinateInPlayground());
    }

    public static Human spawnHuman(Vector2 position)
    {
        return spawnCreature(_instance.PREFAB_Human, position).GetComponent<Human>();
    }

    public static void spawnLions(int amount)
    {
        spawnCreatures(_instance.PREFAB_Lion, amount);
    }

    public static void spawnLions(int amount, Vector2 position)
    {
        spawnCreatures(_instance.PREFAB_Lion, amount, position);
    }

    public static Lion spawnLion()
    {
        return spawnLion(Util.Random.CoordinateInPlayground());
    }

    public static Lion spawnLion(Vector2 position)
    {
        return spawnCreature(_instance.PREFAB_Lion, position).GetComponent<Lion>();
    }

    public static void spawnBoars(int amount)
    {
        spawnCreatures(_instance.PREFAB_Boar, amount);
    }

    public static void spawnBoars(int amount, Vector2 position)
    {
        spawnCreatures(_instance.PREFAB_Boar, amount, position);
    }

    public static Boar spawBoar()
    {
        return spawBoar(Util.Random.CoordinateInPlayground());
    }

    public static Boar spawBoar(Vector2 position)
    {
        return spawnCreature(_instance.PREFAB_Boar, position).GetComponent<Boar>();
    }

    public static void spawnRabbits(int amount)
    {
        spawnCreatures(_instance.PREFAB_Rabbit, amount);
    }

    public static void spawnRabbits(int amount, Vector2 position)
    {
        spawnCreatures(_instance.PREFAB_Rabbit, amount, position);
    }

    public static Rabbit spawRabbit()
    {
        return spawRabbit(Util.Random.CoordinateInPlayground());
    }

    public static Rabbit spawRabbit(Vector2 position)
    {
        return spawnCreature(_instance.PREFAB_Rabbit, position).GetComponent<Rabbit>();
    }

    private static void spawnCreatures(GameObject prefab, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnCreature(prefab, Util.Random.CoordinateInPlayground());
        }
    }

    private static void spawnCreatures(GameObject prefab, int amount, Vector2 position)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnCreature(prefab, position);
        }
    }

    private static GameObject spawnCreature(GameObject prefab, Vector2 c)
    {
        return spawnCreature(prefab, (int)c.x, (int)c.y);
    }

    private static GameObject spawnCreature(GameObject prefab, int posX, int posY)
    {
        GameObject instance = Instantiate(prefab, new Vector2((float)posX + .5f, (float)posY + .5f), Quaternion.identity);
        instance.name = prefab.name;
        ObjectManager.addCreature(instance.GetComponent<Creature>());
        return instance;
    }
}
