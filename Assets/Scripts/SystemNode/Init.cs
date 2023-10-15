/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Class Purposes:
 *  
 *  Class Infos:
 *      
 *  Class References:
 *      
 */

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
        Time.timeScale = 1;
        Time.fixedDeltaTime = Gamevariables.TICKRATE;
    }
}
