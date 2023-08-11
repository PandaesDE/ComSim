using UnityEngine;

public class Gamevariables : MonoBehaviour
{
    [SerializeField] private static readonly Vector2 _PlaygroundSize = new Vector2(100f, 100f);

    [SerializeField] public static string SEED = "";
    [SerializeField] public static int HUMAN_AMOUNT_START = 0;
    [SerializeField] public static int ANIMAL_AMOUNT_START = 0;
    [SerializeField] public static bool GAME_PAUSED = false;

    //Prefabs
    [SerializeField] public GameObject PREFAB_Human_Male;
    [SerializeField] public GameObject PREFAB_Human_Female;
    [SerializeField] public GameObject PREFAB_Animal;
    public static Vector2 playgroundSize
    {
        get { return _PlaygroundSize; }
    }

}
