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
    [SerializeField] private Material _2Dlit;
    [SerializeField] private Sprite _spr_Human_Male;
    [SerializeField] private Sprite _spr_Human_Female;
    [SerializeField] private Sprite _Spr_Lion;
    [SerializeField] private Sprite _Spr_Boar;
    [SerializeField] private Sprite _Spr_Rabbit;

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

    public class SpawnOptions
    {
        public int amount = 1;
        public bool isMale = Util.Random.isMale();
        public Vector2 position = Util.Random.CoordinateInPlayground();
    }

    #region Humans
    public static void spawnHumans(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnHuman();
        }
    }

    public static void spawnHumans(int amount, Vector2 position)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnHuman(position);
        }
    }

    public static void spawnHumans(int amount, Vector2 position, bool isMale)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnHuman(position, isMale);
        }
    }

    public static Human spawnHuman()
    {
        return spawnHuman(Util.Random.CoordinateInPlayground());
    }

    public static Human spawnHuman(Vector2 position)
    {
        return spawnHuman(position, Util.Random.isMale());
    }

    public static Human spawnHuman(Vector2 position, bool ismale)
    {
        GameObject human = new GameObject();
        human.AddComponent<Human>()
            .buidGender(ismale);
        return spawnCreature(human, _instance._Spr_Lion, position).GetComponent<Human>();
    }
#endregion

#region Lions
    public static void spawnLions(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnLion();
        }
    }

    public static void spawnLions(int amount, Vector2 position)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnLion(position);
        }
    }

    public static Lion spawnLion()
    {
        return spawnLion(Util.Random.CoordinateInPlayground());
    }

    public static Lion spawnLion(Vector2 position)
    {
        GameObject lion = new GameObject();
        lion.AddComponent<Lion>();
        return spawnCreature(lion, _instance._Spr_Lion, position).GetComponent<Lion>();
    }
#endregion

#region Boars
    public static void spawnBoars(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnBoar();
        }
    }

    public static void spawnBoars(int amount, Vector2 position)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnBoar(position);
        }
    }

    public static Boar spawnBoar()
    {
        return spawnBoar(Util.Random.CoordinateInPlayground());
    }

    public static Boar spawnBoar(Vector2 position)
    {
        GameObject boar = new GameObject();
        boar.AddComponent<Boar>();
        return spawnCreature(boar, _instance._Spr_Boar, position).GetComponent<Boar>();
    }
#endregion

#region Rabbits
    public static void spawnRabbits(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnRabbit();
        }
    }

    public static void spawnRabbits(int amount, Vector2 position)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnRabbit(position);
        }
    }

    public static Rabbit spawnRabbit()
    {
        return spawnRabbit(Util.Random.CoordinateInPlayground());
    }

    public static Rabbit spawnRabbit(Vector2 position)
    {
        GameObject rabbit = new GameObject();
        rabbit.AddComponent<Rabbit>();
        return spawnCreature(rabbit, _instance._Spr_Rabbit, position).GetComponent<Rabbit>();
    }
    #endregion

    private static GameObject spawnCreature(GameObject creature_GO, Sprite spr, Vector2 c)
    {
        return spawnCreature(creature_GO, spr, (int)c.x, (int)c.y);
    }

    private static GameObject spawnCreature(GameObject creature_GO, Sprite spr, int posX, int posY)
    {
        GameObject instance = Instantiate(creature_GO, new Vector2((float)posX + .5f, (float)posY + .5f), Quaternion.identity);
        instance.name = creature_GO.name;
        SpriteRenderer sr = instance.AddComponent<SpriteRenderer>();
        sr.material = _instance._2Dlit;
        sr.sprite = spr;
        instance.AddComponent<Rigidbody2D>();
        instance.AddComponent<BoxCollider2D>();
        ObjectManager.addCreature(instance.GetComponent<Creature>());
        return instance;
    }
}
