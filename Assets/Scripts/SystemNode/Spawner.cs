using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Gamevariables gameVariables;

    private void Awake()
    {
        gameVariables = GetComponent<Gamevariables>();
    }

    private void Start()
    {
        spawnHumans(Gamevariables.AMOUNT_SPAWN_HUMAN);
        spawnAnimals(Gamevariables.AMOUNT_SPAWN_ANIMAL);
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
        spawnAnimal(c.x, c.y);
    }

    public void spawnAnimal(float posX, float posY)
    {
        Instantiate(gameVariables.PREFAB_Animal, new Vector2(posX, posY), Quaternion.identity);
    }
    #endregion


    #region Human
    public void spawnHumans(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnHuman(0, 0);

        }
    }
    public void spawnHuman(int posX, int posY)
    {
        spawnHuman(posX, posY, Util.getRandomGender());
    }

    public void spawnHuman(int posX, int posY, gender g)
    {
        if (g == gender.MALE)
        {
            Instantiate(gameVariables.PREFAB_Human_Male, new Vector2(posX, posY), Quaternion.identity);
            return;
        }
        if (g == gender.FEMALE)
        {
            Instantiate(gameVariables.PREFAB_Human_Female, new Vector2(posX, posY), Quaternion.identity);
            return;
        }
    }
    #endregion


}
