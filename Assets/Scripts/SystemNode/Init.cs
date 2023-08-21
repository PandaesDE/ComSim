using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        initGameSettings();
        ConfigManager.LoadSettings();
    }

    private void initGameSettings()
    {
        Time.fixedDeltaTime = Gamevariables.TICKRATE;
    }
}
