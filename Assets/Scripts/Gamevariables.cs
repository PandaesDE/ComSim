using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamevariables : MonoBehaviour
{
    [SerializeField] private static readonly Vector2 _PlaygroundSize = new Vector2(100f, 100f);

    //Prefabs
    [SerializeField] public GameObject PREFAB_Human_Male;
    [SerializeField] public GameObject PREFAB_Human_Female;
    [SerializeField] public GameObject PREFAB_Animal;
    public static Vector2 playgroundSize
    {
        get { return _PlaygroundSize; }
    }

}
