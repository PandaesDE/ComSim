using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Spawner
{

    public static void spawnHumans(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnHuman(0, 0);

        }
    }
    public static void spawnHuman(int posX, int posY)
    {
        GameObject.Instantiate(Gamevariables.PREFAB_Human, new Vector2(posX, posY), Quaternion.identity);
    }

}
