using UnityEngine;

public class Gamevariables
{
    //init Settings
    public static readonly Vector2Int playgroundSize = new Vector2Int(362, 200);
    public static readonly float TICKRATE = .5f;

    public static string SEED = "";
    public static int HUMAN_AMOUNT_START = 0;
    public static int LION_AMOUNT_START = 0;
    public static int BOAR_AMOUNT_START = 0;
    public static int RABBIT_AMOUNT_START = 0;
    public static bool GAME_PAUSED = false;

    //Time
    public static readonly int HOURS_PER_DAY = 24;
    public static readonly int MINUTES_PER_HOUR = 60;

    public static int MINUTES_PER_TICK = 1;

    //Light
    /* between 0 - 1*/
    public static float LIGHT_INTENSITY = 1f;

}
