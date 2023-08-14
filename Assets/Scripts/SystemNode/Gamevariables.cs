using UnityEngine;

public class Gamevariables
{
    [SerializeField] private static readonly Vector2 _PlaygroundSize = new Vector2(100f, 100f);

    [SerializeField] public static string SEED = "";
    [SerializeField] public static int HUMAN_AMOUNT_START = 0;
    [SerializeField] public static int ANIMAL_AMOUNT_START = 0;
    [SerializeField] public static bool GAME_PAUSED = false;

    //Time
    [SerializeField] public static readonly int TICKS_PER_HOUR = 20;
    [SerializeField] public static readonly int HOURS_PER_DAY = 24;

    //Prefabs

    public static Vector2 playgroundSize
    {
        get { return _PlaygroundSize; }
    }

}
