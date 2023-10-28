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

public class Gamevariables
{
    //init Settings
    public static readonly Vector2Int playgroundSize = new Vector2Int(360, 200);
    public static readonly float TICKRATE = .5f;

    public static int HUMAN_AMOUNT_START = 0;
    public static int LION_AMOUNT_START = 0;
    public static int BOAR_AMOUNT_START = 0;
    public static int RABBIT_AMOUNT_START = 0;
    public static bool GAME_PAUSED = false;

    //VISUALIZATION
    public static bool SHOW_TRAIL = true;
    public static int TRAIL_LENGTH = 150;

    //WORLD
    public static string SEED = "";
    public static PerlinSettingsObject PSO_GROUND = new PerlinSettingsObject(-.5f, .6f, 8, 1, 0, 0, 100);
    public static PerlinSettingsObject PSO_BUSH = new PerlinSettingsObject(.5f, 3f, 3, 1, 0, 0, 100);

    //Time
    public static readonly int HOURS_PER_DAY = 24;
    public static readonly int MINUTES_PER_HOUR = 60;

    public static int MINUTES_PER_TICK = 1;

    //Light
    /* between 0 - 1*/
    public static float LIGHT_INTENSITY = 1f;


    //Error - Values
    public static Vector2 ERROR_VECTOR2 = Vector2.positiveInfinity;
}
