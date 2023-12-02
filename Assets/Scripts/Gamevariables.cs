/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule N�rnberg
 *  
 *  Description:
 *      - interface for all generic game variables
 *  
 *  References:
 *      Scene:
 *          - scene independent
 *      Script:
 *          - 
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */

using System;
using UnityEngine;

public class Gamevariables
{
    //init Settings
    public static readonly Vector2Int PLAYGROUND_SIZE = new Vector2Int(360, 200);
    public static readonly float TICKRATE = .5f;

    public static int HumanAmountStart = 0;
    public static int LionAmountStart = 0;
    public static int BoarAmountStart = 0;
    public static int RabbitAmountStart = 0;
    public static bool GamePaused = false;

    //VISUALIZATION
    public static bool ShowTrail = true;
    public static int TrailLength = 15;
    public static Trail.ColorScheme TrailColor = Trail.ColorScheme.@default;

    //WORLD
    public static string Seed = "";
    public static PerlinSettingsObject PSO_Ground = new(-.5f, .6f, 8, 1, 0, 0, 100);
    public static PerlinSettingsObject PSO_Bush = new(.5f, 3f, 3, 1, 0, 0, 100);

    //Time
    public static readonly int HOURS_PER_DAY = 24;
    public static readonly int MINUTES_PER_HOUR = 60;

    public static int MinutesPerTick = 1;
    public static int MinutesPassed = 0;

    //Light
    /* between 0 - 1*/
    public static float LightIntensity = 1f;


    //Error - Values
    public static readonly Vector2 ERROR_VECTOR2 = Vector2.negativeInfinity;

    public enum z_layer
    {
        entity = -1,
        world = 0
    }

    //Debug
    public static readonly bool LOGGING_ENABLED = false;
}
