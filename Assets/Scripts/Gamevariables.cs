using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Gamevariables
{
    [SerializeField]
    private static readonly Vector2 _PlaygroundSize = new Vector2(100f, 100f);


    //Prefabs
    private static GameObject _PREFAB_Human;


    //Getter & Setter
    public static Vector2 playgroundSize
    {
        get
        {
            return _PlaygroundSize;
        }
    }

    public static GameObject PREFAB_Human
    {
        get
        {
            return _PREFAB_Human;
        }
    }

    public static void initializePrefabs(PrefabBundle b)
    {
        _PREFAB_Human = b.Human;
    }

}
