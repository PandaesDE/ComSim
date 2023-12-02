/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - This class handles all entity instantiations
 *  
 *  References:
 *      Scene:
 *          - Simulation scene(s)
 *      Script:
 *          - 
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
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
        SpawnHumans(new SpawnOptions(Gamevariables.HumanAmountStart, true));
        SpawnLions(new SpawnOptions(Gamevariables.LionAmountStart, true));
        SpawnBoars(new SpawnOptions(Gamevariables.BoarAmountStart, true));
        SpawnRabbits(new SpawnOptions(Gamevariables.RabbitAmountStart, true));
    }

#region Humans
    public static List<Human> SpawnHumans(SpawnOptions so)
    {
        List<Human> outp = new();
        if (so.RandomSpawn)
        {
            for (int i = 0; i < so.Amount; i++)
            {
                outp.Add(SpawnHuman(so.RandomPosition, so.IsMale));
            }
        }
        else
        {
            for (int i = 0; i < so.Amount; i++)
            {
                outp.Add(SpawnHuman(so.Position, so.IsMale));
            }
        }
        return outp;
    }

    private static Human SpawnHuman(Vector2 position, bool ismale)
    {
        GameObject human = new()
        {
            name = "Human"
        };
        human.AddComponent<Human>()
            .BuildGender(ismale);
        return SpawnCreature(human, _instance._Spr_Lion, position).GetComponent<Human>();
    }
#endregion



#region Lions
    public static List<Lion> SpawnLions(SpawnOptions so)
    {
        List<Lion> outp = new();
        if (so.RandomSpawn)
        {
            for (int i = 0; i < so.Amount; i++)
            {
                outp.Add(SpawnLion(so.RandomPosition, so.IsMale));
            }
        }
        else
        {
            for (int i = 0; i < so.Amount; i++)
            {
                outp.Add(SpawnLion(so.Position, so.IsMale));
            }
        }
        return outp;
    }

    private static Lion SpawnLion(Vector2 position, bool isMale)
    {
        GameObject lion = new()
        {
            name = "Lion"
        };
        lion.AddComponent<Lion>()
            .BuildGender(isMale);
        return SpawnCreature(lion, _instance._Spr_Lion, position).GetComponent<Lion>();
    }
#endregion



#region Boars
    public static List<Boar> SpawnBoars(SpawnOptions so)
    {
        List<Boar> outp = new();
        if (so.RandomSpawn)
        {
            for (int i = 0; i < so.Amount; i++)
            {
                outp.Add(SpawnBoar(so.RandomPosition, so.IsMale));
            }
        }
        else
        {
            for (int i = 0; i < so.Amount; i++)
            {
                outp.Add(SpawnBoar(so.Position, so.IsMale));
            }
        }
        return outp;
    }

    private static Boar SpawnBoar(Vector2 position, bool isMale)
    {
        GameObject boar = new()
        {
            name = "Boar"
        };
        boar.AddComponent<Boar>()
            .BuildGender(isMale);
        return SpawnCreature(boar, _instance._Spr_Boar, position).GetComponent<Boar>();
    }
#endregion



#region Rabbits
    public static List<Rabbit> SpawnRabbits(SpawnOptions so)
    {
        List<Rabbit> outp = new();
        if (so.RandomSpawn)
        {
            for (int i = 0; i < so.Amount; i++)
            {
                outp.Add(SpawnRabbit(so.RandomPosition, so.IsMale));
            }
        }
        else
        {
            for (int i = 0; i < so.Amount; i++)
            {
                outp.Add(SpawnRabbit(so.Position, so.IsMale));
            }
        }
        return outp;
    }

    private static Rabbit SpawnRabbit(Vector2 position, bool isMale)
    {
        GameObject rabbit = new()
        {
            name = "Rabbit"
        };
        rabbit.AddComponent<Rabbit>()
            .BuildGender(isMale);
        return SpawnCreature(rabbit, _instance._Spr_Rabbit, position).GetComponent<Rabbit>();
    }
#endregion


    public static GameObject MakeCorpse(Creature creature)
    {
        GameObject corpse_GO = Instantiate(creature.gameObject); //make copy
        corpse_GO.name = $"Corpse - {creature.name}";
        ObjectManager.DeleteCreature(creature);                  //deltes old creature gameObject


        corpse_GO.AddComponent<Corpse>();
        Corpse c = corpse_GO.GetComponent<Corpse>();
        c.SetWeight(corpse_GO.GetComponent<Creature>().Weight);
        Destroy(corpse_GO.GetComponent<Creature>());
        Destroy(corpse_GO.GetComponent<LineRenderer>());


        SpriteRenderer sr = corpse_GO.GetComponent<SpriteRenderer>();
        sr.sprite = _instance._Spr_Corpse;

        ObjectManager.AddCorpse(c);

        return corpse_GO;
    }

    private static GameObject SpawnCreature(GameObject creature_GO, Sprite spr, Vector2 c)
    {
        return SpawnCreature(creature_GO, spr, (int)c.x, (int)c.y);
    }

    private static GameObject SpawnCreature(GameObject creature_GO, Sprite spr, int posX, int posY)
    {
        creature_GO = AddEntityComponents(creature_GO, spr, posX, posY);
        ObjectManager.AddCreature(creature_GO.GetComponent<Creature>());
        return creature_GO;
    }

    private static GameObject AddEntityComponents(GameObject creature_GO, Sprite spr, int posX, int posY)
    {
        creature_GO.transform.position = new Vector3(posX + .5f, posY + .5f, (float)Gamevariables.z_layer.entity);
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
