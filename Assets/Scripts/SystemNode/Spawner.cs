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
        spawnCreatures(PREFAB_Human ,Gamevariables.HUMAN_AMOUNT_START);
        spawnCreatures(PREFAB_Lion ,Gamevariables.LION_AMOUNT_START);
        spawnCreatures(PREFAB_Boar ,Gamevariables.BOAR_AMOUNT_START);
        spawnCreatures(PREFAB_Rabbit ,Gamevariables.RABBIT_AMOUNT_START);
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
    public void spawnCreatures(GameObject prefab, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject entity = spawnCreature(prefab, Util.Random.CoordinateInPlayground());
        }
    }

    public GameObject spawnCreature(GameObject prefab, Vector2 c)
    {
        return spawnCreature(prefab, (int)c.x, (int)c.y);
    }

    public GameObject spawnCreature(GameObject prefab, int posX, int posY)
    {
        GameObject instance = Instantiate(prefab, new Vector2((float)posX + .5f, (float)posY + .5f), Quaternion.identity);
        ObjectManager.addCreature(instance.GetComponent<Creature>());
        return instance;
    }
    #endregion
}
