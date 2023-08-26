using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] public GameObject PREFAB_Human_Male;
    [SerializeField] public GameObject PREFAB_Human_Female;
    [SerializeField] public GameObject PREFAB_Animal;

    private void Awake()
    {
    }

    private void Start()
    {
        spawnHumans(Gamevariables.HUMAN_AMOUNT_START);
        spawnAnimals(Gamevariables.ANIMAL_AMOUNT_START);
    }

    #region DEBUG Functions

    private void sensesCheckSetup()
    {
        int a = 20;
        Human hum = spawnHuman(new Vector2(a, 0));
        Animal ani = spawnAnimal(new Vector2(-a, 0));
        hum.setTarget(new Vector2(-a, 0));
        ani.setTarget(new Vector2(a, 0));
    }

    #endregion

    #region Animal
    public void spawnAnimals(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnAnimal(Util.getRandomCoordinateInPlayground());
        }
    }

    public Animal spawnAnimal(Vector2 c)
    {
        return spawnAnimal((int)c.x, (int)c.y);
    }

    public Animal spawnAnimal(int posX, int posY)
    {
        GameObject spawn = null;
        spawn = Instantiate(PREFAB_Animal, new Vector2((float)posX+.5f, (float)posY+.5f), Quaternion.identity);
        return spawn.GetComponent<Animal>();
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
        return spawnHuman((int)c.x, (int)c.y, Util.getRandomGender());
    }

    public Human spawnHuman(int posX, int posY, gender g)
    {
        GameObject spawn = null;
        if (g == gender.MALE)
        {
            spawn = Instantiate(PREFAB_Human_Male, new Vector2((float)posX+.5f, (float)posY + .5f), Quaternion.identity);
        } else 
        {
            spawn = Instantiate(PREFAB_Human_Female, new Vector2((float)posX + .5f, (float)posY + .5f), Quaternion.identity);
        }
        return spawn.GetComponent<Human>();


    }
    #endregion


}
