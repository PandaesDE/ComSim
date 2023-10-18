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

public class Spawner : MonoBehaviour
{
    [SerializeField] public GameObject PREFAB_Human;
    [SerializeField] public GameObject PREFAB_Boar;
    [SerializeField] public GameObject PREFAB_Rabbit;
    [SerializeField] public GameObject PREFAB_Lion;

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

    #region DEBUG Functions

    /*This function is to test and verify the Entity Senses*/
    //private void sensesCheckSetup()
    //{
    //    int a = 20;
    //    Human hum = spawnHuman(new Vector2(a, 0));
    //    Boar ani = spawnAnimal(animalType.BOAR,new Vector2(-a, 0));
    //    hum.movement.setTarget(new Vector2(-a, 0));
    //    ani.movement.setTarget(new Vector2(a, 0));
    //}

    #endregion

    #region Animal
    public void spawnAnimals(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnAnimal(Util.Random.AnimalType(), Util.Random.CoordinateInPlayground());
        }
    }

    public void spawnBoars(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnAnimal(animalType.BOAR, Util.Random.CoordinateInPlayground());
        }
    }

    public void spawnRabbits(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnAnimal(animalType.RABBIT, Util.Random.CoordinateInPlayground());
        }
    }

    public void spawnLions(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnAnimal(animalType.LION, Util.Random.CoordinateInPlayground());
        }
    }

    public Boar spawnAnimal(animalType a, Vector2 c)
    {
        return spawnAnimal(a, (int)c.x, (int)c.y);
    }

    public Boar spawnAnimal(animalType a, int posX, int posY)
    {
        GameObject spawn = Instantiate(getCorrespondingPrefab(a), new Vector2((float)posX+.5f, (float)posY+.5f), Quaternion.identity);
        return spawn.GetComponent<Boar>();
    }

    private GameObject getCorrespondingPrefab(animalType a)
    {
        if (a.Equals(animalType.BOAR)) return PREFAB_Boar;
        if (a.Equals(animalType.LION)) return PREFAB_Lion;
        if (a.Equals(animalType.RABBIT)) return PREFAB_Rabbit;
        return null;
    }
    #endregion


    #region Human
    public void spawnHumans(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnHuman(new Vector2(0,0));

        }
    }
    public Human spawnHuman(Vector2 c)
    {
        return spawnHuman((int)c.x, (int)c.y, Util.Random.Gender());
    }

    public Human spawnHuman(int posX, int posY, gender g)
    {
        GameObject spawn = Instantiate(PREFAB_Human, new Vector2((float)posX + .5f, (float)posY + .5f), Quaternion.identity);
        return spawn.GetComponent<Human>();
    }
    #endregion


}
