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

    #region Animal
    public void spawnAnimals(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnAnimal(Util.getRandomCoordinateInPlayground());
        }
    }

    public void spawnAnimal(Vector2 c)
    {
        spawnAnimal((int)c.x, (int)c.y);
    }

    public void spawnAnimal(int posX, int posY)
    {
        Instantiate(PREFAB_Animal, new Vector2((float)posX+.5f, (float)posY+.5f), Quaternion.identity);
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
    public void spawnHuman(Vector2 c)
    {
        spawnHuman((int)c.x, (int)c.y, Util.getRandomGender());
    }

    public void spawnHuman(int posX, int posY, gender g)
    {
        if (g == gender.MALE)
        {
            Instantiate(PREFAB_Human_Male, new Vector2((float)posX+.5f, (float)posY + .5f), Quaternion.identity);
            return;
        }
        if (g == gender.FEMALE)
        {
            Instantiate(PREFAB_Human_Female, new Vector2((float)posX + .5f, (float)posY + .5f), Quaternion.identity);
            return;
        }
    }
    #endregion


}
