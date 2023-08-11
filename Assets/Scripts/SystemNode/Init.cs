using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        ConfigManager.LoadSettings();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
