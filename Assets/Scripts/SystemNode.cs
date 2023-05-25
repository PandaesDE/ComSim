using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemNode : MonoBehaviour
{
    private Spawner spawner;

    private void Awake()
    {
        spawner = GetComponent<Spawner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        spawner.spawnHumans(25);
        spawner.spawnAnimals(25);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
