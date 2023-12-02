/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Initialize generic settings
 *  
 *  References:
 *      Scene:
 *          - 
 *      Script:
 *          - 
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */

using UnityEngine;

public class Init : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        InitGameSettings();
        ConfigManager.LoadSettings();
    }

    private void InitGameSettings()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = Gamevariables.TICKRATE;
    }
}
