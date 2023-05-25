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

    }

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

    public void spawnHuman(int posX, int posY, gender g) { 
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

}
