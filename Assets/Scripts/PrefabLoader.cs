using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabLoader : MonoBehaviour
{
    /* This Class needs to be a MonoBehaviour to get prefab instances without Resource.Load
     * This Class is used to initialize all GameVariable Prefabs 
     */
    public GameObject PREFAB_Human;

    // Start is called before the first frame update
    void Start()
    {
        Gamevariables.initializePrefabs(new PrefabBundle(PREFAB_Human));
    }

}
