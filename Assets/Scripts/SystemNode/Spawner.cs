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
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] private Material _2Dlit;
    [SerializeField] private Sprite _spr_Human_Male;
    [SerializeField] private Sprite _spr_Human_Female;
    [SerializeField] private Sprite _Spr_Lion;
    [SerializeField] private Sprite _Spr_Boar;
    [SerializeField] private Sprite _Spr_Rabbit;
    [SerializeField] private Sprite _Spr_Corpse;

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

#region Humans
    public static List<Human> spawnHumans(SpawnOptions so)
    {
        List<Human> outp = new();
        if (so.isRandom)
        {
            for (int i = 0; i < so.amount; i++)
            {
                outp.Add(spawnHuman(so.randomPosition, so.isMale));
            }
        }
        else
        {
            for (int i = 0; i < so.amount; i++)
            {
                outp.Add(spawnHuman(so.position, so.isMale));
            }
        }
        return outp;
    }

    private static Human spawnHuman(Vector2 position, bool ismale)
    {
        GameObject human = new GameObject();
        human.name = "Human";
        human.AddComponent<Human>()
            .build_Gender(ismale);
        return spawnCreature(human, _instance._Spr_Lion, position).GetComponent<Human>();
    }
#endregion



#region Lions
    public static List<Lion> spawnLions(SpawnOptions so)
    {
        List<Lion> outp = new();
        if (so.isRandom)
        {
            for (int i = 0; i < so.amount; i++)
            {
                outp.Add(spawnLion(so.randomPosition, so.isMale));
            }
        }
        else
        {
            for (int i = 0; i < so.amount; i++)
            {
                outp.Add(spawnLion(so.position, so.isMale));
            }
        }
        return outp;
    }

    private static Lion spawnLion(Vector2 position, bool isMale)
    {
        GameObject lion = new GameObject();
        lion.name = "Lion";
        lion.AddComponent<Lion>()
            .build_Gender(isMale);
        return spawnCreature(lion, _instance._Spr_Lion, position).GetComponent<Lion>();
    }
#endregion



#region Boars
    public static List<Boar> spawnBoars(SpawnOptions so)
    {
        List<Boar> outp = new();
        if (so.isRandom)
        {
            for (int i = 0; i < so.amount; i++)
            {
                outp.Add(spawnBoar(so.randomPosition, so.isMale));
            }
        }
        else
        {
            for (int i = 0; i < so.amount; i++)
            {
                outp.Add(spawnBoar(so.position, so.isMale));
            }
        }
        return outp;
    }

    private static Boar spawnBoar(Vector2 position, bool isMale)
    {
        GameObject boar = new GameObject();
        boar.name = "Boar";
        boar.AddComponent<Boar>()
            .build_Gender(isMale);
        return spawnCreature(boar, _instance._Spr_Boar, position).GetComponent<Boar>();
    }
#endregion



#region Rabbits
    public static List<Rabbit> spawnRabbits(SpawnOptions so)
    {
        List<Rabbit> outp = new();
        if (so.isRandom)
        {
            for (int i = 0; i < so.amount; i++)
            {
                outp.Add(spawnRabbit(so.randomPosition, so.isMale));
            }
        }
        else
        {
            for (int i = 0; i < so.amount; i++)
            {
                outp.Add(spawnRabbit(so.position, so.isMale));
            }
        }
        return outp;
    }

    private static Rabbit spawnRabbit(Vector2 position, bool isMale)
    {
        GameObject rabbit = new GameObject();
        rabbit.name = "Rabbit";
        rabbit.AddComponent<Rabbit>()
            .build_Gender(isMale);
        return spawnCreature(rabbit, _instance._Spr_Rabbit, position).GetComponent<Rabbit>();
    }
#endregion


    public static GameObject spawnCorpse(Creature creature)
    {
        GameObject corpse_GO = Instantiate(creature.gameObject); //make copy
        corpse_GO.name = $"Corpse - {creature.name}";
        ObjectManager.deleteCreature(creature);                  //deltes old creature gameObject


        corpse_GO.gameObject.AddComponent<Corpse>();
        Corpse c = corpse_GO.GetComponent<Corpse>();
        c.setWeight(corpse_GO.GetComponent<Creature>().weight);
        Destroy(corpse_GO.GetComponent<Creature>());
        Destroy(corpse_GO.GetComponent<LineRenderer>());


        SpriteRenderer sr = corpse_GO.GetComponent<SpriteRenderer>();
        sr.sprite = _instance._Spr_Corpse;

        ObjectManager.addCorpse(c);

        return corpse_GO.gameObject;
    }

    private static GameObject spawnCreature(GameObject creature_GO, Sprite spr, Vector2 c)
    {
        return spawnCreature(creature_GO, spr, (int)c.x, (int)c.y);
    }

    private static GameObject spawnCreature(GameObject creature_GO, Sprite spr, int posX, int posY)
    {
        creature_GO = addEntityComponents(creature_GO, spr, posX, posY);
        ObjectManager.addCreature(creature_GO.GetComponent<Creature>());
        return creature_GO;
    }

    private static GameObject addEntityComponents(GameObject creature_GO, Sprite spr, int posX, int posY)
    {
        creature_GO.transform.position = new Vector3(posX + .5f, posY + .5f, (float)Gamevariables.z_layer.ENTITY);
        SpriteRenderer sr = creature_GO.AddComponent<SpriteRenderer>();
        sr.material = _instance._2Dlit;
        sr.sprite = spr;
        LineRenderer lr = creature_GO.AddComponent<LineRenderer>();
        lr.material = _instance._2Dlit;
        Rigidbody2D rb2d = creature_GO.AddComponent<Rigidbody2D>();
        rb2d.bodyType = RigidbodyType2D.Kinematic;
        creature_GO.AddComponent<BoxCollider2D>();
        return creature_GO;
    }
}
