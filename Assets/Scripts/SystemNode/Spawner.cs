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
        spawnHumans(new SpawnOptions(Gamevariables.HUMAN_AMOUNT_START, true));
        spawnLions(new SpawnOptions(Gamevariables.LION_AMOUNT_START, true));
        spawnBoars(new SpawnOptions(Gamevariables.BOAR_AMOUNT_START, true));
        spawnRabbits(new SpawnOptions(Gamevariables.RABBIT_AMOUNT_START, true));
    }

    public class SpawnOptions
    {
        public int amount = 1;
        public bool isMale = Util.Random.isMale();
        public bool isRandom = false;

        private Vector2 _position = Vector2.zero;
        public Vector2 position
        {
            get
            {
                if (isRandom)
                {
                    return randomPosition;
                }
                else
                {
                    return _position;
                }
            }
            set
            {
                _position = value;
            }
        }

        public Vector2 randomPosition { 
            get 
            { 
                return Util.Random.CoordinateInPlayground(); 
            } 
        }

        public SpawnOptions() { }
        public SpawnOptions(int amount, bool isRandom)
        {
            this.amount = amount;
            this.isRandom = isRandom;
        }

    }

#region Humans
    public static void spawnHumans(SpawnOptions so)
    {
        if (so.isRandom)
        {
            for (int i = 0; i < so.amount; i++)
            {
                spawnHuman(so.randomPosition, so.isMale);
            }
        }
        else
        {
            for (int i = 0; i < so.amount; i++)
            {
                spawnHuman(so.position, so.isMale);
            }
        }
    }

    private static Human spawnHuman(Vector2 position, bool ismale)
    {
        GameObject human = new GameObject();
        human.AddComponent<Human>()
            .buidGender(ismale);
        return spawnCreature(human, _instance._Spr_Lion, position).GetComponent<Human>();
    }
#endregion



#region Lions
    public static void spawnLions(SpawnOptions so)
    {
        if (so.isRandom)
        {
            for (int i = 0; i < so.amount; i++)
            {
                spawnLion(so.randomPosition, so.isMale);
            }
        }
        else
        {
            for (int i = 0; i < so.amount; i++)
            {
                spawnLion(so.position, so.isMale);
            }
        }
    }

    private static Lion spawnLion(Vector2 position, bool isMale)
    {
        GameObject lion = new GameObject();
        lion.AddComponent<Lion>()
            .buidGender(isMale);
        return spawnCreature(lion, _instance._Spr_Lion, position).GetComponent<Lion>();
    }
#endregion



#region Boars
    public static void spawnBoars(SpawnOptions so)
    {
        if (so.isRandom)
        {
            for (int i = 0; i < so.amount; i++)
            {
                spawnBoar(so.randomPosition, so.isMale);
            }
        }
        else
        {
            for (int i = 0; i < so.amount; i++)
            {
                spawnBoar(so.position, so.isMale);
            }
        }
    }

    private static Boar spawnBoar(Vector2 position, bool isMale)
    {
        GameObject boar = new GameObject();
        boar.AddComponent<Boar>()
            .buidGender(isMale);
        return spawnCreature(boar, _instance._Spr_Boar, position).GetComponent<Boar>();
    }
#endregion



#region Rabbits
    public static void spawnRabbits(SpawnOptions so)
    {
        if (so.isRandom)
        {
            for (int i = 0; i < so.amount; i++)
            {
                spawnRabbit(so.randomPosition, so.isMale);
            }
        }
        else
        {
            for (int i = 0; i < so.amount; i++)
            {
                spawnRabbit(so.position, so.isMale);
            }
        }
    }

    public static Rabbit spawnRabbit(Vector2 position, bool isMale)
    {
        GameObject rabbit = new GameObject();
        rabbit.AddComponent<Rabbit>()
            .buidGender(isMale);
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
